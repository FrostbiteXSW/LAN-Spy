﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                var hostListCopy = new List<Host>();
                lock (_hostList) {
                    hostListCopy.AddRange(_hostList);
                }
                return hostListCopy.AsReadOnly();
            }
        }

        /// <summary>
        ///     尝试搜寻目前局域网内的所有设备。
        /// </summary>
        /// <exception cref="TimeoutException">等待线程结束超时。</exception>
        public void ScanForTarget() {
            // 获取当前设备
            var device = DeviceList[CurDevName];

            // 创建分析线程
            const int analyzeThreadsCount = 4;
            var analyzeThreads = new Thread[analyzeThreadsCount];
            for (var i = 0; i < analyzeThreadsCount; i++)
                analyzeThreads[i] = new Thread(ScanPacketAnalyzeThread);

            // 创建发包线程队列
            var sendThreads = new List<Thread>();

            try {
                // arp头起始位置向后偏移6字节后，取2字节内容即为arp包类型
                device = StartCapture("arp [6:2] = 2");

                // 启动分析线程
                for (var i = 0; i < analyzeThreadsCount; i++)
                    analyzeThreads[i].Start();

                // 去除网络号和广播地址，产生地址集合
                byte[] minAddress = NetworkNumber.GetAddressBytes(),
                       maxAddress = BroadcastAddress.GetAddressBytes(),
                       tempAddress = minAddress;
                var ipAddresses = new List<IPAddress>();
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
                var lastSendThread = new Thread(ScanPacketSendThread);
                lastSendThread.Start(ipAddresses);
                sendThreads.Add(lastSendThread);

                // 等待数据包发送完成
                new WaitTimeoutChecker((int) (60 * 1000 * Math.Log(AddressCount, 254))).ThreadSleep(500, () => sendThreads.Any(item => item.IsAlive));

                // 等待接收目标机反馈消息
                Thread.Sleep((int) (8 * 1000 * Math.Log(AddressCount, 254)));
            }
            finally {
                // 终止发包线程
                foreach (var sendThread in sendThreads)
                    if (sendThread.IsAlive)
                        sendThread.Abort();

                // 终止分析线程
                foreach (var analyzeThread in analyzeThreads)
                    if (analyzeThread.IsAlive)
                        analyzeThread.Abort();

                // 清理缓冲区及其他内容
                lock (_hostList) {
                    _hostList.Sort(Host.SortMethod);
                }
                StopCapture(device);
                ClearCaptures();
            }
        }

        /// <summary>
        ///     被动监测目前局域网内的所有设备（导致阻塞）。
        /// </summary>
        /// <exception cref="TimeoutException">等待分析线程终止超时。</exception>
        public void SpyForTarget() {
            // 获取当前设备
            var device = DeviceList[CurDevName];

            // 创建分析线程
            var analyzeThreads = new Thread[8];

            try {
                // 开始扫描
                device = StartCapture("arp");

                // 初始化分析线程
                for (var i = 0; i < 8; i++) {
                    analyzeThreads[i] = new Thread(ScanPacketAnalyzeThread);
                    analyzeThreads[i].Start();
                }

                // 如果分析线程异常终止，则结束监听
                while (analyzeThreads.All(analyzeThread => analyzeThread.IsAlive))
                    Thread.Sleep(800);
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
                new WaitTimeoutChecker(30000).ThreadSleep(500, () => analyzeThreads.Any(analyzeThread => analyzeThread.IsAlive));

                // 清理缓冲区及其他内容
                lock (_hostList) {
                    _hostList.Sort(Host.SortMethod);
                }
                StopCapture(device);
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
                var device = DeviceList[CurDevName];

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
                    ProtocolAddressType = EthernetPacketType.IPv4
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
                            if (_hostList.All(item => !item.PhysicalAddress.ToString().Equals(arp.SenderHardwareAddress.ToString())))
                                // 添加新的主机记录
                                _hostList.Add(new Host(arp.SenderProtocolAddress, arp.SenderHardwareAddress));
                            else
                                // 更新已有主机记录
                                _hostList.Find(item => item.PhysicalAddress.ToString().Equals(arp.SenderHardwareAddress.ToString())).IPAddress = arp.SenderProtocolAddress;
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

        /// <inheritdoc />
        /// <summary>
        ///     重置主机列表及数据包缓冲区。
        /// </summary>
        public override void Reset() {
            lock (_hostList) {
                _hostList.Clear();
            }
            ClearCaptures();
        }

        /// <inheritdoc />
        /// <summary>
        ///     此方法继承自父类，而不进行任何动作。
        /// </summary>
        public override void Stop() { }
    }
}