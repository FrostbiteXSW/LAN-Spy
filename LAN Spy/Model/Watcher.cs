using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
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
        public ReadOnlyCollection<TcpLink> TcpLinks {
            get {
                lock (_tcpLinks) {
                    return _tcpLinks.AsReadOnly();
                }
            }
        }

        /// <summary>
        ///     打开设备并开始监听网路连接。
        /// </summary>
        /// <exception cref="InvalidOperationException">已有一项监听工作正在进行。</exception>
        public void StartWatching() {
            // 判断是否存在未停止的监听操作
            if (_device != null)
                throw new InvalidOperationException("已有一项监听工作正在进行。");

            // 打开设备
            _device = DeviceList[CurDevIndex];
            _device.Open();
            _device.OnPacketArrival += Device_OnPacketArrival;
            _device.StartCapture();

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
                        if (ether.Type != EthernetPacketType.IpV4) continue;

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

                        // 保存TCP连接信息
                        lock (_tcpLinks) {
                            // 查找连接信息
                            var tcpLink = _tcpLinks.Find(item => Equals(item.Src, srcAddress) && Equals(item.Dst, dstAddress));
                            if (tcpLink == null)
                                _tcpLinks.Add(new TcpLink(srcAddress, dstAddress));
                            else
                                tcpLink.UpdateTime();
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
                    Thread.Sleep(120 * 1000);
                    lock (_tcpLinks) {
                        foreach (var tcpLink in _tcpLinks) {
                            var timeSpan = DateTime.Now - tcpLink.Time;
                            if (timeSpan.TotalSeconds >= 300)
                                _tcpLinks.Remove(tcpLink);
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
            var waitTime = 60 * 1000;
            while (waitTime >= 0) {
                waitTime = -waitTime;
                foreach (var watchThread in _watchThreads)
                    if (watchThread.IsAlive) {
                        Thread.Sleep(100);
                        if ((waitTime = -waitTime - 100) == 0)
                            throw new TimeoutException("等待线程结束超时。");
                        break;
                    }
            }
            _watchThreads.Clear();

            // 向过期连接丢弃线程发送终止信号
            if (_dropOutdatedLinksThread.IsAlive)
                _dropOutdatedLinksThread.Abort();

            // 等待过期连接丢弃线程终止
            waitTime = 60 * 1000;
            while (true) {
                if (!_dropOutdatedLinksThread.IsAlive) break;
                Thread.Sleep(100);
                waitTime -= 100;
                if (waitTime == 0)
                    throw new TimeoutException("等待线程结束超时。");
            }
            _dropOutdatedLinksThread = null;

            // 关闭设备
            _device.StopCapture();
            _device.OnPacketArrival -= Device_OnPacketArrival;
            _device.Close();
            _device = null;

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

        /// <summary>
        ///     网路上一组TCP连接的两端主机。
        /// </summary>
        public class TcpLink {
            /// <summary>
            ///     初始化 <see cref="TcpLink" /> 类的实例。
            /// </summary>
            /// <param name="src">连接的起点，推荐以当前子网的主机作为起点。</param>
            /// <param name="dst">连接的终点，推荐以非当前子网的主机作为终点。</param>
            public TcpLink(IPAddress src, IPAddress dst) {
                Src = src;
                Dst = dst;
                UpdateTime();
            }

            /// <summary>
            ///     连接的起点。
            /// </summary>
            public IPAddress Src { get; }

            /// <summary>
            ///     连接的终点，
            /// </summary>
            public IPAddress Dst { get; }

            /// <summary>
            ///     最后检测到连接的时间，并非确切的连接开始时间，而是侦测到连接时的本地时间。
            /// </summary>
            public DateTime Time { get; private set; }

            /// <summary>
            ///     更新最后检测到连接的时间。
            /// </summary>
            public void UpdateTime() {
                Time = DateTime.Now;
            }
        }
    }
}