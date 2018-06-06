﻿using System;
using PacketDotNet;
using PacketDotNet.Utils;
using SharpPcap;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace LAN_Spy {
    /// <summary>
    ///     子网主机扫描器。
    /// </summary>
    public class Scanner : BasicClass {
        /// <summary>
        ///     获取当前选中设备所在网段的所有可用主机IP地址数量。
        /// </summary>
        public double AddressCount {
            get {
                byte[] minAddress = NetworkNumber.GetAddressBytes(),
                    maxAddress = BroadcastAddress.GetAddressBytes();
                double count = maxAddress[3] - minAddress[3] + 1;
                for (int j = 0; j < 3; j++)
                    count *= maxAddress[j] - minAddress[j] + 1;
                return count - 2;
            }
        }

        /// <summary>
        ///     已知主机列表。
        /// </summary>
        private readonly List<Host> _hostList = new List<Host>();

        /// <summary>
        ///     获取已知主机列表的只读封装。
        /// </summary>
        public ReadOnlyCollection<Host> HostList {
            get {
                lock (_hostList)
                    return _hostList.AsReadOnly();
            }
        }

        /// <summary>
        ///     Device_OnPacketArrival方法中使用，缓存获得的ARP原始数据包。
        /// </summary>
        private readonly Queue<RawCapture> _rawArpCaptures = new Queue<RawCapture>();

        /// <summary>
        ///     尝试搜寻目前局域网内的所有设备。
        /// </summary>
        public void ScanForTarget() {
            // 计算可用主机地址范围
            GetNetInfo();

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
            byte[] minAddress = NetworkNumber.GetAddressBytes(),
                maxAddress = BroadcastAddress.GetAddressBytes(),
                tempAddress = minAddress;
            List<IPAddress> ipAddresses = new List<IPAddress>();
            List<Thread> sendThreads = new List<Thread>();
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
                    sendThreads.Add(sendThread);
                    ipAddresses = new List<IPAddress>();
                }
                int i = 3;
                while (i >= 0 && tempAddress[i] == 255) {
                    tempAddress[i] = 0;
                    i--;
                }
                tempAddress[i]++;
            }

            // 最后一个发送线程
            Thread lastsendThread = new Thread(ScanPacketSendThread);
            lastsendThread.Start(ipAddresses);
            sendThreads.Add(lastsendThread);

            // 等待数据包发送完成
            bool flag = true;
            while (flag) {
                flag = false;
                foreach (var sendThread in sendThreads)
                    if (sendThread.IsAlive) {
                        flag = true;
                        Thread.Sleep(100);
                        break;
                    }
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
            lock (_hostList) {
                _hostList.Sort((a, b) => string.CompareOrdinal(a.IPAddress.ToString(), b.IPAddress.ToString()));
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

                // 如果分析线程异常终止，则结束监听
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
                lock (_hostList) {
                    _hostList.Sort((a, b) => string.CompareOrdinal(a.IPAddress.ToString(), b.IPAddress.ToString()));
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
                        lock (_hostList) {
                            Host host = _hostList.Find(item => item.PhysicalAddress.ToString().Equals(arp.SenderHardwareAddress.ToString()));
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
        ///     重置扫描器到初始状态。
        /// </summary>
        public new void Reset() {
            lock (_hostList) {
                _hostList.Clear();
            }
            lock (_rawArpCaptures) {
                _rawArpCaptures.Clear();
            }
            base.Reset();
        }
    }
}