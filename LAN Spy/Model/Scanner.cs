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

namespace LAN_Spy.Model {
    /// <summary>
    ///     子网主机扫描器。
    /// </summary>
    public class Scanner : BasicClass {
        /// <summary>
        ///     已知主机列表。
        /// </summary>
        private readonly List<Host> _hostList = new List<Host>();

        /// <summary>
        ///     获取当前选中设备所在网段的所有可用主机IP地址数量。
        /// </summary>
        public double AddressCount {
            get {
                byte[] minAddress = NetworkNumber.GetAddressBytes(),
                    maxAddress = BroadcastAddress.GetAddressBytes();
                double count = maxAddress[3] - minAddress[3] + 1;
                for (var j = 0; j < 3; j++)
                    count *= maxAddress[j] - minAddress[j] + 1;
                return count - 2;
            }
        }

        /// <summary>
        ///     获取已知主机列表的只读封装。
        /// </summary>
        public ReadOnlyCollection<Host> HostList {
            get {
                lock (_hostList) {
                    return _hostList.AsReadOnly();
                }
            }
        }

        /// <summary>
        ///     尝试搜寻目前局域网内的所有设备。
        /// </summary>
        /// <exception cref="TimeoutException">等待线程结束超时。</exception>
        public void ScanForTarget() {
            // 获取当前设备
            var device = DeviceList[CurDevIndex];

            // 绑定抓包事件处理方法
            device.OnPacketArrival += Device_OnPacketArrival;

            // 打开设备并开始扫描
            device.Open();

            // arp头起始位置向后偏移6字节后，取2字节内容即为arp包类型
            device.Filter = "arp [6:2] = 2";
            device.StartCapture();

            // 创建分析线程
            var analyzeThreads = new Thread[4];
            for (var i = 0; i < 4; i++) {
                analyzeThreads[i] = new Thread(ScanPacketAnalyzeThread);
                analyzeThreads[i].Start();
            }

            // 去除网络号和广播地址，产生地址集合
            byte[] minAddress = NetworkNumber.GetAddressBytes(),
                maxAddress = BroadcastAddress.GetAddressBytes(),
                tempAddress = minAddress;
            var ipAddresses = new List<IPAddress>();
            var sendThreads = new List<Thread>();
            tempAddress[3]++;
            while (!(tempAddress[0] == maxAddress[0]
                     && tempAddress[1] == maxAddress[1]
                     && tempAddress[2] == maxAddress[2]
                     && tempAddress[3] == maxAddress[3])) {
                ipAddresses.Add(new IPAddress(tempAddress));
                if (ipAddresses.Count >= (AddressCount / 8 >= 254 ? 254 : AddressCount / 8)) {
                    // 创建发包线程
                    var sendThread = new Thread(ScanPacketSendThread);
                    sendThread.Start(ipAddresses);
                    sendThreads.Add(sendThread);
                    ipAddresses = new List<IPAddress>();
                }
                var i = 3;
                while (i >= 0 && tempAddress[i] == 255) {
                    tempAddress[i] = 0;
                    i--;
                }
                tempAddress[i]++;
            }

            // 最后一个发送线程
            var lastsendThread = new Thread(ScanPacketSendThread);
            lastsendThread.Start(ipAddresses);
            sendThreads.Add(lastsendThread);

            // 等待数据包发送完成
            var waitTime = (int) (60 * 1000 * Math.Log(AddressCount, 254));
            while (waitTime >= 0) {
                waitTime = -waitTime;
                foreach (var sendThread in sendThreads)
                    if (sendThread.IsAlive) {
                        Thread.Sleep(100);
                        if ((waitTime = -waitTime - 100) <= 0)
                            throw new TimeoutException("等待线程结束超时。");
                        break;
                    }
            }

            // 等待接收目标机反馈消息
            Thread.Sleep((int) (8 * 1000 * Math.Log(AddressCount, 254)));

            // 接收完成，终止分析线程
            foreach (var analyzeThread in analyzeThreads)
                analyzeThread.Abort();

            // 清理缓冲区及其他内容
            lock (_hostList) {
                _hostList.Sort((a, b) => string.CompareOrdinal(a.IPAddress.ToString(), b.IPAddress.ToString()));
            }
            device.StopCapture();
            device.OnPacketArrival -= Device_OnPacketArrival;
            device.Close();
            ClearCaptures();
        }

        /// <summary>
        ///     被动监测目前局域网内的所有设备（导致阻塞）。
        /// </summary>
        /// <exception cref="TimeoutException">等待分析线程终止超时。</exception>
        public void SpyForTarget() {
            // 获取当前设备
            var device = DeviceList[CurDevIndex];

            // 创建分析线程
            var analyzeThreads = new Thread[8];

            try {
                // 绑定抓包事件处理方法
                device.OnPacketArrival += Device_OnPacketArrival;

                // 打开设备并开始扫描
                device.Open();
                device.Filter = "arp";
                device.StartCapture();

                // 初始化分析线程
                for (var i = 0; i < 8; i++) {
                    analyzeThreads[i] = new Thread(ScanPacketAnalyzeThread);
                    analyzeThreads[i].Start();
                }

                // 如果分析线程异常终止，则结束监听
                while (true) {
                    if (analyzeThreads.Any(analyzeThread => !analyzeThread.IsAlive)) break;
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

                // 等待分析线程终止
                var waitTime = 30 * 1000;
                while (true) {
                    if (analyzeThreads.All(analyzeThread => !analyzeThread.IsAlive)) break;
                    Thread.Sleep(500);
                    if ((waitTime -= 500) == 0)
                        throw new TimeoutException("等待分析线程终止超时。");
                }

                // 清理缓冲区及其他内容
                lock (_hostList) {
                    _hostList.Sort((a, b) => string.CompareOrdinal(a.IPAddress.ToString(), b.IPAddress.ToString()));
                }
                device.StopCapture();
                device.OnPacketArrival -= Device_OnPacketArrival;
                device.Close();
                ClearCaptures();
            }
        }

        /// <summary>
        ///     设备扫描发包线程。
        /// </summary>
        /// <param name="obj">地址列表。</param>
        private void ScanPacketSendThread(object obj) {
            try {
                // 获取当前设备
                var device = DeviceList[CurDevIndex];

                // 获取地址列表
                var ipAddresses = (List<IPAddress>) obj;

                // 构建包信息
                var ether = new EthernetPacket(device.MacAddress,
                    new PhysicalAddress(new byte[] {255, 255, 255, 255, 255, 255}),
                    EthernetPacketType.Arp);
                var arp = new ARPPacket(ARPOperation.Request,
                    new PhysicalAddress(new byte[] {0, 0, 0, 0, 0, 0}),
                    new IPAddress(new byte[] {0, 0, 0, 0}),
                    device.MacAddress,
                    Ipv4Address) {
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
            catch (ThreadAbortException) { }
        }

        /// <summary>
        ///     设备扫描分析线程。
        /// </summary>
        private void ScanPacketAnalyzeThread() {
            try {
                while (true) {
                    // 从队列中请求一个数据包
                    RawCapture packet;
                    if ((packet = NextRawCapture) != null) {
                        // 分析数据包中的数据
                        var ether = new EthernetPacket(new ByteArraySegment(packet.Data));
                        var arp = (ARPPacket) ether.PayloadPacket;
                        lock (_hostList) {
                            var host = _hostList.Find(item => item.PhysicalAddress.ToString().Equals(arp.SenderHardwareAddress.ToString()));
                            if (host == null)
                                // 添加新的主机记录
                                _hostList.Add(new Host(arp.SenderProtocolAddress, arp.SenderHardwareAddress));
                            else
                                // 更新已有主机记录
                                host.IPAddress = arp.SenderProtocolAddress;
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
        ///     重置主机列表及数据包缓冲区。
        /// </summary>
        public void Reset() {
            lock (_hostList) {
                _hostList.Clear();
            }
            ClearCaptures();
        }
    }
}