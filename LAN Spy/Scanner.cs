using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using PacketDotNet;
using PacketDotNet.Utils;
using SharpPcap;
using SharpPcap.WinPcap;

namespace LAN_Spy {
    /// <summary>
    ///     子网主机扫描器。
    /// </summary>
    public class Scanner {
        /// <summary>
        ///     获取可用网络设备列表。
        /// </summary>
        public CaptureDeviceList DeviceList { get; } = CaptureDeviceList.Instance;

        /// <summary>
        ///     获取或设置当前使用的设备编号。
        /// </summary>
        public int CurDevIndex;
        
        /// <summary>
        ///     当前选中设备的IPv4地址。
        /// </summary>
        private IPAddress _ipv4Address = new IPAddress(new byte[] {0, 0, 0, 0});

        /// <summary>
        ///     当前选中设备所在网段的网络号。
        /// </summary>
        private IPAddress _networkNumber = new IPAddress(new byte[] {0, 0, 0, 0});
        
        /// <summary>
        ///     当前选中设备所在网段的广播地址。
        /// </summary>
        private IPAddress _broadcastAddress = new IPAddress(new byte[] {0, 0, 0, 0});

        /// <summary>
        ///     获取当前选中设备所在网段的所有可用主机IP地址数量。
        /// </summary>
        public double AddressCount {
            get {
                byte[] minAddress = _networkNumber.GetAddressBytes(),
                    maxAddress = _broadcastAddress.GetAddressBytes();
                double count = maxAddress[3] - minAddress[3] + 1;
                for (int j = 0; j < 3; j++)
                    count *= maxAddress[j] - minAddress[j] + 1;
                return count - 2;
            }
        }

        /// <summary>
        ///     已知主机列表。
        /// </summary>
        private List<Host> _hostList = new List<Host>();
        /// <summary>
        ///     获取已知主机列表的只读封装。
        /// </summary>
        public ReadOnlyCollection<Host> HostList => _hostList.AsReadOnly();

        /// <summary>
        ///     Device_OnPacketArrival方法中使用，缓存获得的ARP原始数据包。
        /// </summary>
        private readonly Queue<RawCapture> _rawArpCaptures = new Queue<RawCapture>();

        /// <summary>
        ///     尝试搜寻目前局域网内的所有设备。
        /// </summary>
        public void ScanForTarget() {
            // 计算可用主机地址范围
            CalculateAddressRange();

            // 获取当前设备
            var device = DeviceList[CurDevIndex];

            // 绑定抓包事件处理方法
            device.OnPacketArrival += Device_OnPacketArrival;
            
            // 打开设备并开始扫描
            if (!device.Started) {
                device.Open();
                // arp头起始位置向后偏移6字节后，取2字节内容即为arp包类型
                device.Filter = "arp [6:2] = 2";
                device.StartCapture();
            }
            
            // 创建分析线程
            Thread[] analyzeThreads = new Thread[4];
            for (int i = 0; i < 4; i++) {
                analyzeThreads[i] = new Thread(ScanPacketAnalyzeThread);
                analyzeThreads[i].Start();
            }
            
            // 去除网络号和广播地址，产生地址集合
            byte[] minAddress = _networkNumber.GetAddressBytes(),
                maxAddress = _broadcastAddress.GetAddressBytes(),
                tempAddress = minAddress;
            List<IPAddress> ipAddresses = new List<IPAddress>();
            tempAddress[3]++;
            while (!(tempAddress[0] == maxAddress[0]
                     && tempAddress[1] == maxAddress[1]
                     && tempAddress[2] == maxAddress[2]
                     && tempAddress[3] == maxAddress[3])) {
                ipAddresses.Add(new IPAddress(tempAddress));
                if (ipAddresses.Count >= (AddressCount / 8 >= 30 ? 30 : AddressCount / 8)) {
                    // 创建发包线程
                    Thread sendThread = new Thread(ScanPacketSendThread);
                    sendThread.Start(ipAddresses);
                    ipAddresses = new List<IPAddress>();
                }
                int i = 3;
                while (i >= 0 && tempAddress[i] == 255) {
                    tempAddress[i] = 0;
                    i--;
                }
                tempAddress[i]++;
            }

            // 等待接收目标机反馈消息
            Thread.Sleep(8 * 1000);

            // 接收完成，终止分析线程
            foreach (var analyzeThread in analyzeThreads)
                analyzeThread.Abort();

            // 清理缓冲区及其他内容
            lock (_rawArpCaptures) {
                _rawArpCaptures.Clear();
            }
            device.OnPacketArrival -= Device_OnPacketArrival;
            device.StopCapture();
            device.Close();
        }

        /// <summary>
        ///     被动监测目前局域网内的所有设备（导致阻塞）。
        /// </summary>
        /// <exception cref="ThreadAbortException"></exception>
        public void SpyForTarget() {
            // 获取当前设备
            var device = DeviceList[CurDevIndex];

            // 创建分析线程
            Thread[] analyzeThreads = new Thread[8];

            try {
                // 绑定抓包事件处理方法
                device.OnPacketArrival += Device_OnPacketArrival;

                // 打开设备并开始扫描
                if (!device.Started) {
                    device.Open();
                    device.Filter = "arp";
                    device.StartCapture();
                }

                // 初始化分析线程
                for (int i = 0; i < 8; i++) {
                    analyzeThreads[i] = new Thread(ScanPacketAnalyzeThread);
                    analyzeThreads[i].Start();
                }

                while (true) {
                    if (analyzeThreads.Any(analyzeThread => !analyzeThread.IsAlive)) return;
                    Thread.Sleep(800);
                }
            }
            catch (ThreadAbortException) {
                // 终止分析线程
                foreach (var analyzeThread in analyzeThreads)
                    analyzeThread.Abort();
            }
            finally {
                // 终止分析线程
                foreach (var analyzeThread in analyzeThreads)
                    if (analyzeThread.IsAlive)
                        analyzeThread.Abort();

                // 清理缓冲区及其他内容
                lock (_rawArpCaptures) {
                    _rawArpCaptures.Clear();
                }
                device.OnPacketArrival -= Device_OnPacketArrival;
                device.StopCapture();
                device.Close();
            }
        }

        /// <summary>
        ///     ScanForTarget方法中使用，抓包事件处理方法。
        /// </summary>
        /// <param name="sender">事件发送者。</param>
        /// <param name="e">事件参数。</param>
        private void Device_OnPacketArrival(object sender, CaptureEventArgs e) {
            lock (_rawArpCaptures) _rawArpCaptures.Enqueue(e.Packet);
        }

        /// <summary>
        ///     设备扫描发包线程。
        /// </summary>
        private void ScanPacketSendThread(object obj) {
            // 获取当前设备
            var device = DeviceList[CurDevIndex];

            // 获取地址列表
            List<IPAddress> ipAddresses = (List<IPAddress>) obj;

            // 构建包信息
            EthernetPacket ether = new EthernetPacket(device.MacAddress,
                new PhysicalAddress(new byte[] {255, 255, 255, 255, 255, 255}),
                EthernetPacketType.Arp);
            ARPPacket arp = new ARPPacket(ARPOperation.Request,
                new PhysicalAddress(new byte[] {0, 0, 0, 0, 0, 0}),
                new IPAddress(new byte[] {0, 0, 0, 0}),
                device.MacAddress,
                _ipv4Address) {
                HardwareAddressType = LinkLayers.Ethernet,
                ProtocolAddressType = EthernetPacketType.IpV4
            };
            ether.PayloadPacket = arp;
            arp.ParentPacket = ether;

            // 根据目标地址信息发送ARP请求
            foreach (var targetAddress in ipAddresses) {
                arp.TargetProtocolAddress = targetAddress;
                arp.UpdateCalculatedValues();
                device.SendPacket(ether);
            }
        }
        
        /// <summary>
        ///     设备扫描分析线程。
        /// </summary>
        private void ScanPacketAnalyzeThread() {
            try {
                while (true) {
                    RawCapture packet = null;

                    // 从队列中请求一个数据包
                    lock (_rawArpCaptures) {
                        if (_rawArpCaptures.Count > 0) {
                            packet = _rawArpCaptures.Peek();
                            _rawArpCaptures.Dequeue();
                        }
                    }

                    if (packet != null) {
                        // 分析数据包中的数据
                        EthernetPacket ether = new EthernetPacket(new ByteArraySegment(packet.Data));
                        ARPPacket arp = (ARPPacket) ether.PayloadPacket;
                        Host host = _hostList.Find(item => item.PhysicalAddress.Equals(arp.SenderHardwareAddress));
                        if (host == null)
                            // 添加新的主机记录
                            _hostList.Add(new Host(arp.SenderProtocolAddress, arp.SenderHardwareAddress));
                        else
                            // 更新已有主机记录
                            host.IPAddress = arp.SenderProtocolAddress;
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
        ///     计算当前选中设备所在网段的可用主机IP地址范围。
        /// </summary>
        /// <exception cref="InvalidOperationException">未能获得有效的IPv4地址或子网掩码。</exception>
        /// <exception cref="FormatException">无效的子网掩码。</exception>
        public void CalculateAddressRange() {
            // 获取当前设备
            WinPcapDevice device = (WinPcapDevice) DeviceList[CurDevIndex];
            
            // 设备首选IPv4地址
            byte[] ipAddress = null;
            // 设备IPv4地址子网掩码
            byte[] netMask = null;

            // 获取首选IPv4地址及子网掩码
            foreach (var address in device.Addresses) {
                if (address.Addr.sa_family != 2) continue;
                ipAddress = address.Addr.ipAddress.GetAddressBytes();
                netMask = address.Netmask.ipAddress.GetAddressBytes();
                break;
            }

            // 检查是否获得了有效的IPv4地址及子网掩码
            if (ipAddress == null || netMask == null) 
                throw new InvalidOperationException("未能获得有效的IPv4地址或子网掩码。");

            // 子网掩码查错——基本格式
            bool flag = false;
            for (int i = 0; i < 4; i++) {
                if (flag && netMask[i] != 0) throw new FormatException("无效的子网掩码。");
                if (flag || netMask[i] == 255) continue;
                byte b = netMask[i];
                while (b != 0) {
                    if ((b & 128) == 128) b <<= 1;
                    else throw new FormatException("无效的子网掩码。");
                }
                flag = true;
            }

            // 子网掩码查错——数据有效性
            if (netMask[3] > 252) throw new FormatException("无效的子网掩码。");

            // 记录可能最小地址和最大地址
            byte[] minAddress = new byte[4], maxAddress = new byte[4];

            // 通过子网掩码计算最小最大地址
            for (int i = 0; i < 4; i++) {
                minAddress[i] = (byte) (ipAddress[i] & netMask[i]);
                maxAddress[i] = (byte) (ipAddress[i] | 255 - netMask[i]);
            }

            // 保存到IP地址、网络号和广播地址
            _ipv4Address = new IPAddress(ipAddress);
            _networkNumber = new IPAddress(minAddress);
            _broadcastAddress = new IPAddress(maxAddress);
        }

        /// <summary>
        ///     重置扫描器到初始状态。
        /// </summary>
        public void Reset() {
            _hostList.Clear();
            lock (_rawArpCaptures) {
                _rawArpCaptures.Clear();
            }
            _ipv4Address = new IPAddress(new byte[] {0, 0, 0, 0});
            _networkNumber = new IPAddress(new byte[] {0, 0, 0, 0});
            _broadcastAddress = new IPAddress(new byte[] {0, 0, 0, 0});
        }
    }
}
