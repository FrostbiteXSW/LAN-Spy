using SharpPcap;
using SharpPcap.WinPcap;
using System;
using System.Net;

namespace LAN_Spy {
    public abstract class BasicClass {
        /// <summary>
        ///     获取可用网络设备列表。
        /// </summary>
        public CaptureDeviceList DeviceList { get; } = CaptureDeviceList.Instance;
        
        /// <summary>
        ///     当前使用的设备编号。
        /// </summary>
        private int _curDevIndex;

        /// <summary>
        ///     获取或设置当前使用的设备编号。
        /// </summary>
        public int CurDevIndex {
            get => _curDevIndex;
            set {
                _curDevIndex = value;
                GetNetInfo();
            }
        }
        
        /// <summary>
        ///     当前选中设备的IPv4地址。
        /// </summary>
        protected IPAddress Ipv4Address = new IPAddress(new byte[] {0, 0, 0, 0});

        /// <summary>
        ///     当前选中设备所在网段的网络号。
        /// </summary>
        protected IPAddress NetworkNumber = new IPAddress(new byte[] {0, 0, 0, 0});
        
        /// <summary>
        ///     当前选中设备所在网段的广播地址。
        /// </summary>
        protected IPAddress BroadcastAddress = new IPAddress(new byte[] {0, 0, 0, 0});
        
        /// <summary>
        ///     获取当前设备所在网络的IPv4地址、网络号和广播地址。
        /// </summary>
        /// <exception cref="InvalidOperationException">未能获得有效的IPv4地址或子网掩码。</exception>
        /// <exception cref="FormatException">无效的子网掩码。</exception>
        protected void GetNetInfo() {
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
            Ipv4Address = new IPAddress(ipAddress);
            NetworkNumber = new IPAddress(minAddress);
            BroadcastAddress = new IPAddress(maxAddress);
        }
        
        /// <summary>
        ///     重置为初始状态。
        /// </summary>
        protected void Reset() {
            Ipv4Address = new IPAddress(new byte[] {0, 0, 0, 0});
            NetworkNumber = new IPAddress(new byte[] {0, 0, 0, 0});
            BroadcastAddress = new IPAddress(new byte[] {0, 0, 0, 0});
        }
    }
}
