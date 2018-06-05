using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
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
        public int CurDevIndex { get; set; }

        /// <summary>
        ///     CalculateAddressRange方法中使用，记录当前选中设备所在网段的所有可用主机IP地址。
        /// </summary>
        private readonly List<IPAddress> _ipAddresses = new List<IPAddress>();

        /// <summary>
        ///     获取当前选中设备所在网段的所有可用主机IP地址数量。
        /// </summary>
        public int AddressCount => _ipAddresses.Count;

        /// <summary>
        ///     Device_OnPacketArrival方法中使用，缓存获得的ARP原始数据包。
        /// </summary>
        private readonly List<RawCapture> _rawArpCaptures = new List<RawCapture>();

        /// <summary>
        ///     尝试搜寻目前局域网内的所有设备。
        /// </summary>
        public void ScanForTarget() {
            // 获取当前设备
            var device = DeviceList[CurDevIndex];

            // 绑定抓包事件处理方法
            device.OnPacketArrival += Device_OnPacketArrival;
            
            // 打开设备并开始扫描
            device.Open(DeviceMode.Normal, 1000);
            device.Filter = "arp";
            device.StartCapture();
            
            // 创建发包线程
            Thread[] sendThreads = new Thread[8];
            for (int i = 0; i < 8; i++) {
                sendThreads[i] = new Thread(ScanPacketSendThread);
                sendThreads[i].Start();
            }
            
            // 创建分析线程
            Thread[] analyzeThreads = new Thread[4];
            for (int i = 0; i < 4; i++) {
                analyzeThreads[i] = new Thread(ScanPacketAnalyzeThread);
                analyzeThreads[i].Start();
            }
        }

        /// <summary>
        ///     ScanForTarget方法中使用，抓包事件处理方法。
        /// </summary>
        /// <param name="sender">事件发送者。</param>
        /// <param name="e">事件参数。</param>
        private void Device_OnPacketArrival(object sender, CaptureEventArgs e) {
            _rawArpCaptures.Add(e.Packet);
        }

        /// <summary>
        ///     设备扫描发包线程。
        /// </summary>
        private void ScanPacketSendThread() {
            // 获取当前设备
            var device = DeviceList[CurDevIndex];
            
            throw new NotImplementedException();
        }
        
        /// <summary>
        ///     设备扫描分析线程。
        /// </summary>
        private void ScanPacketAnalyzeThread() {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     计算当前选中设备所在网段的可用主机IP地址范围。
        /// </summary>
        public void CalculateAddressRange() {
            // 重置当前IP地址列表
            _ipAddresses.Clear();

            // 获取当前设备
            WinPcapDevice device = (WinPcapDevice) DeviceList[CurDevIndex];

            // 设备IPv4地址
            var ipAddress = device.Addresses[1].Addr.ipAddress.GetAddressBytes();
            // 设备IPv4地址子网掩码
            var netMask = device.Addresses[1].Netmask.ipAddress.GetAddressBytes();

            // 子网掩码查错——基本格式
            bool flag = false;
            for (int i = 0; i < 4; i++) {
                if (flag) {
                    if (netMask[i] != 0) throw new FormatException("无效的子网掩码。");
                }
                else if (netMask[i] != 255) {
                    byte b = netMask[i];
                    while (b != 0) {
                        if ((b & 128) == 1) b <<= 1;
                        else throw new FormatException("无效的子网掩码。");
                    }
                    flag = true;
                }
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

            // 去除网络号和广播地址，产生地址集合
            var tempAddress = minAddress;
            tempAddress[3]++;
            while (!(tempAddress[0] == maxAddress[0]
                     && tempAddress[1] == maxAddress[1]
                     && tempAddress[2] == maxAddress[2]
                     && tempAddress[3] == maxAddress[3])) {
                _ipAddresses.Add(new IPAddress(tempAddress));
                int i = 3;
                while (i >= 0 && tempAddress[i] == 255) {
                    tempAddress[i] = 0;
                    i--;
                }
                if (i < 0) return;
                tempAddress[i]++;
            }
        }
    }
}
