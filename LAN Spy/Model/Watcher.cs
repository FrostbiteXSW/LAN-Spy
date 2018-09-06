using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using LAN_Spy.Model.Classes;
using PacketDotNet;
using PacketDotNet.Utils;
using SharpPcap;

namespace LAN_Spy.Model {
    /// <summary>
    ///     网路流量监视器。
    /// </summary>
    public class Watcher : BasicClass {
        /// <summary>
        ///     监测到的Tcp连接列表。
        /// </summary>
        private readonly List<TcpLink> _tcpLinks = new List<TcpLink>();

        /// <summary>
        ///     监测到的可能存在，仍等待进一步确认的Tcp连接列表，键值表示监测到的次数。
        /// </summary>
        private readonly List<KeyValuePair<TcpLink, int>> _possibleTcpLinks = new List<KeyValuePair<TcpLink, int>>();

        /// <summary>
        ///     监视线程句柄。
        /// </summary>
        private readonly List<Thread> _watchThreads = new List<Thread>();

        /// <summary>
        ///     当前使用的设备句柄。
        /// </summary>
        private ICaptureDevice _device;

        /// <summary>
        ///     过期连接丢弃线程句柄。
        /// </summary>
        private Thread _dropOutdatedLinksThread;

        /// <summary>
        ///     获取监测到的Tcp连接列表。
        /// </summary>
        public IReadOnlyList<TcpLink> TcpLinks {
            get {
                var tcpLinksCopy = new List<TcpLink>();
                lock (_tcpLinks) {
                    tcpLinksCopy.AddRange(_tcpLinks);
                }
                tcpLinksCopy.Sort((a, b) =>
                    string.CompareOrdinal(a.Src.ToString() + " " + a.Dst.ToString(),
                        b.Src.ToString() + " " + b.Dst.ToString()));
                return tcpLinksCopy.AsReadOnly();
            }
        }

        /// <summary>
        ///     打开设备并开始监听网路连接，如果模块已在工作状态则不会有效果。
        /// </summary>
        /// <exception cref="InvalidOperationException">已有一项监听工作正在进行。</exception>
        public void StartWatching() {
            // 判断是否存在未停止的监听操作
            if (_device != null)
                throw new InvalidOperationException("已有一项监听工作正在进行。");

            // 打开设备
            _device = StartCapture();

            // 创建监听线程
            for (var i = 0; i < 8; i++) {
                var watchThread = new Thread(WatchThread);
                watchThread.Start();
                _watchThreads.Add(watchThread);
            }

            // 创建超时检测线程
            _dropOutdatedLinksThread = new Thread(DropOutdatedLinksThread);
            _dropOutdatedLinksThread.Start();
        }

        /// <summary>
        ///     数据包监视分析线程。
        /// </summary>
        private void WatchThread() {
            try {
                while (true) {
                    // 从队列中请求一个数据包
                    RawCapture packet;
                    if ((packet = NextRawCapture) != null) {
                        // 分析数据包中的数据
                        var ether = new EthernetPacket(new ByteArraySegment(packet.Data));
                        if (ether.Type != EthernetPacketType.IPv4) continue;

                        // 分析IPv4数据包
                        var ipv4 = (IPv4Packet) ether.PayloadPacket;
                        if (ipv4.Protocol != IPProtocolType.TCP) continue;

                        // 分析TCP数据包
                        var tcp = (TcpPacket) ipv4.PayloadPacket;
                        if (!tcp.Ack) continue;

                        // 获取IP数据包内的源地址、目标地址以及设备上的子网掩码
                        byte[] src = ipv4.SourceAddress.GetAddressBytes(),
                            dst = ipv4.DestinationAddress.GetAddressBytes(),
                            netmask = Netmask.GetAddressBytes();

                        // 通过子网掩码计算网络号
                        for (var i = 0; i < 4; i++) {
                            src[i] = (byte) (src[i] & netmask[i]);
                            dst[i] = (byte) (dst[i] & netmask[i]);
                        }

                        // 判断IP数据包内的源网络号和目标网络号哪个在当前子网内，并将其作为源地址。
                        IPAddress srcNetworkNumber = new IPAddress(src), srcAddress, dstAddress;
                        if (Equals(srcNetworkNumber, NetworkNumber)) {
                            srcAddress = ipv4.SourceAddress;
                            dstAddress = ipv4.DestinationAddress;
                        }
                        else {
                            srcAddress = ipv4.DestinationAddress;
                            dstAddress = ipv4.SourceAddress;
                        }

                        // 保存或更新可能的TCP连接信息
                        lock (_possibleTcpLinks) {
                            if (_possibleTcpLinks.All(item => !Equals(item.Key.Src, srcAddress) || !Equals(item.Key.Dst, dstAddress))) {
                                // 此连接不存在于可能的TCP连接列表中
                                lock (_tcpLinks) {
                                    // 查找连接信息
                                    var tcpLink = _tcpLinks.Find(item => Equals(item.Src, srcAddress) && Equals(item.Dst, dstAddress));
                                    if (tcpLink is null) {
                                        // 此连接也不存在于TCP连接列表中
                                        if (!tcp.Fin && !tcp.Rst)
                                            // 增加新连接到列表中
                                            _possibleTcpLinks.Add(new KeyValuePair<TcpLink, int>(new TcpLink(srcAddress, dstAddress), 1));
                                    }
                                    else {
                                        // 此连接存在于TCP连接列表中
                                        if (!tcp.Fin && !tcp.Rst)
                                            // 连接仍然存活，更新最后发现时间
                                            tcpLink.UpdateTime();
                                        else 
                                            // 检测到Fin或Rst标志且Ack标志激活，则此连接的终止已被接受，移除连接
                                            _tcpLinks.Remove(tcpLink);
                                    }
                                }
                            }
                            else {
                                // 此连接存在于可能的TCP连接列表中
                                var tcpLink = _possibleTcpLinks.Find(item => Equals(item.Key.Src, srcAddress) && Equals(item.Key.Dst, dstAddress));
                                
                                // 暂时移除此连接，如果连接仍然存活则之后再更新连接信息并重新加入
                                _possibleTcpLinks.Remove(tcpLink);
                                
                                // 检测到Fin或Rst标志且Ack标志激活，则此连接的终止已被接受，永久移除此连接
                                if (tcp.Fin || tcp.Rst) continue;

                                // 连接仍然存活，更新最后发现时间及发现次数
                                if (tcpLink.Value == 2)
                                    // 已经监测到此连接两次，可以确定连接存在
                                    _tcpLinks.Add(tcpLink.Key);
                                else {
                                    // 连接可信度仍然不高，继续监测
                                    tcpLink = new KeyValuePair<TcpLink, int>(tcpLink.Key, tcpLink.Value + 1);
                                    _possibleTcpLinks.Add(tcpLink);
                                }
                            }
                        }
                    }
                    else {
                        // 队列尚未获得数据，挂起等待
                        Thread.Sleep(100);
                    }
                }
            }
            catch (ThreadAbortException) { }
        }

        /// <summary>
        ///     检测并自动丢弃过期TCP连接信息，超时时间为5分钟。
        /// </summary>
        private void DropOutdatedLinksThread() {
            try {
                while (true) {
                    Thread.Sleep(15 * 1000);
                    // 清理TCP连接列表
                    lock (_tcpLinks) {
                        for (var i = 0; i < _tcpLinks.Count; i++) {
                            var timeSpan = DateTime.Now - _tcpLinks[i].Time;
                            if (timeSpan.TotalSeconds >= 30)
                                _tcpLinks.RemoveAt(i);
                        }
                    }

                    // 清理可能的TCP连接列表
                    lock (_possibleTcpLinks) {
                        for (var i = 0; i < _possibleTcpLinks.Count; i++) {
                            var timeSpan = DateTime.Now - _possibleTcpLinks[i].Key.Time;
                            if (timeSpan.TotalSeconds >= 30)
                                _possibleTcpLinks.RemoveAt(i);
                        }
                    }
                }
            }
            catch (ThreadAbortException) { }
        }

        /// <summary>
        ///     停止监听网路连接并关闭设备。
        /// </summary>
        /// <exception cref="TimeoutException">等待线程结束超时。</exception>
        public void StopWatching() {
            // 向监听线程发送终止信号
            foreach (var watchThread in _watchThreads)
                if (watchThread.IsAlive)
                    watchThread.Abort();

            // 等待监听线程终止
            var sleeper = new WaitTimeoutChecker(30000);
            while (_watchThreads.Any(item => item.IsAlive))
                sleeper.ThreadSleep(100);
            _watchThreads.Clear();

            // 向过期连接丢弃线程发送终止信号
            if (!(_dropOutdatedLinksThread is null) && _dropOutdatedLinksThread.IsAlive) {
                _dropOutdatedLinksThread.Abort();

                // 等待过期连接丢弃线程终止
                sleeper = new WaitTimeoutChecker(30000);
                while (_dropOutdatedLinksThread.IsAlive)
                    sleeper.ThreadSleep(100);
                _dropOutdatedLinksThread = null;
            }

            // 关闭设备
            if (!(_device is null)) {
                StopCapture(_device);
                _device = null;
            }

            // 清理缓冲区
            ClearCaptures();
        }

        /// <summary>
        ///     重置TCP连接列表为空并清空数据包缓冲区。
        /// </summary>
        public void Reset() {
            lock (_tcpLinks) {
                _tcpLinks.Clear();
            }
            ClearCaptures();
        }
    }
}