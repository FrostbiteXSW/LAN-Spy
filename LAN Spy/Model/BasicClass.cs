using System;
using System.Collections.Generic;
using System.Net;
using SharpPcap;
using SharpPcap.WinPcap;

namespace LAN_Spy.Model {
    public abstract class BasicClass {
        /// <summary>
        ///     缓存获得的原始数据包。
        /// </summary>
        private readonly Queue<RawCapture> _rawCaptures = new Queue<RawCapture>();

        /// <summary>
        ///     当前选中设备所在网段的广播地址。
        /// </summary>
        private IPAddress _broadcastAddress = new IPAddress(new byte[] {0, 0, 0, 0});

        /// <summary>
        ///     当前使用的设备编号。
        /// </summary>
        private int _curDevIndex = -1;

        /// <summary>
        ///     当前选中设备的网关地址。
        /// </summary>
        private IPAddress _gatewayAddress = new IPAddress(new byte[] {0, 0, 0, 0});

        /// <summary>
        ///     当前选中设备的IPv4地址。
        /// </summary>
        private IPAddress _ipv4Address = new IPAddress(new byte[] {0, 0, 0, 0});

        /// <summary>
        ///     当前选中设备所在网段的子网掩码。
        /// </summary>
        private IPAddress _netmask = new IPAddress(new byte[] {0, 0, 0, 0});

        /// <summary>
        ///     当前选中设备所在网段的网络号。
        /// </summary>
        private IPAddress _networkNumber = new IPAddress(new byte[] {0, 0, 0, 0});

        /// <summary>
        ///     获取可用网络设备列表，在模块中进行抓包发包作业时请使用此对象。
        /// </summary>
        protected CaptureDeviceList DeviceList { get; } = CaptureDeviceList.New();

        /// <summary>
        ///     获取或设置当前使用的设备编号。
        /// </summary>
        public int CurDevIndex {
            get {
                if (_curDevIndex == -1)
                    throw new ArgumentNullException($"CurDevIndex");
                return _curDevIndex;
            }
            set {
                _curDevIndex = value;
                GetNetInfo();
            }
        }

        /// <summary>
        ///     获取下一个原始数据包并将其从缓存中移除。
        /// </summary>
        protected RawCapture NextRawCapture {
            get {
                lock (_rawCaptures) {
                    if (_rawCaptures.Count <= 0) return null;
                    var packet = _rawCaptures.Peek();
                    _rawCaptures.Dequeue();
                    return packet;
                }
            }
        }

        /// <summary>
        ///     获取当前选中设备的IPv4地址。
        /// </summary>
        public IPAddress Ipv4Address => new IPAddress(_ipv4Address.GetAddressBytes());

        /// <summary>
        ///     获取当前选中设备的网关地址。
        /// </summary>
        public IPAddress GatewayAddress => new IPAddress(_gatewayAddress.GetAddressBytes());

        /// <summary>
        ///     获取当前选中设备所在网段的网络号。
        /// </summary>
        public IPAddress NetworkNumber => new IPAddress(_networkNumber.GetAddressBytes());

        /// <summary>
        ///     获取当前选中设备所在网段的子网掩码。
        /// </summary>
        public IPAddress Netmask => new IPAddress(_netmask.GetAddressBytes());

        /// <summary>
        ///     获取当前选中设备所在网段的广播地址。
        /// </summary>
        public IPAddress BroadcastAddress => new IPAddress(_broadcastAddress.GetAddressBytes());

        /// <summary>
        ///     通用抓包事件处理方法。
        /// </summary>
        /// <param name="sender">事件发送者。</param>
        /// <param name="e">事件参数。</param>
        protected void Device_OnPacketArrival(object sender, CaptureEventArgs e) {
            lock (_rawCaptures) {
                _rawCaptures.Enqueue(e.Packet);
            }
        }

        /// <summary>
        ///     获取当前设备所在网络的IPv4地址、网络号和广播地址。
        /// </summary>
        /// <exception cref="InvalidOperationException">未能获得有效的IPv4地址或子网掩码。</exception>
        /// <exception cref="FormatException">无效的子网掩码。</exception>
        private void GetNetInfo() {
            // 获取当前设备
            var device = (WinPcapDevice) DeviceList[CurDevIndex];

            // 保存设备网关地址
            _gatewayAddress = device.Interface.GatewayAddress;

            // 设备首选IPv4地址
            byte[] ipAddress = null;
            // 设备IPv4地址子网掩码
            byte[] netmask = null;

            // 获取首选IPv4地址及子网掩码
            foreach (var address in device.Addresses) {
                if (address.Addr.sa_family != 2) continue;
                ipAddress = address.Addr.ipAddress.GetAddressBytes();
                netmask = address.Netmask.ipAddress.GetAddressBytes();
                break;
            }

            // 检查是否获得了有效的IPv4地址及子网掩码
            if (ipAddress == null || netmask == null)
                throw new InvalidOperationException("未能获得有效的IPv4地址或子网掩码。");

            // 子网掩码查错——基本格式
            var flag = false;
            for (var i = 0; i < 4; i++) {
                if (flag && netmask[i] != 0) throw new FormatException("无效的子网掩码。");
                if (flag || netmask[i] == 255) continue;
                var b = netmask[i];
                while (b != 0)
                    if ((b & 128) == 128)
                        b <<= 1;
                    else
                        throw new FormatException("无效的子网掩码。");
                flag = true;
            }

            // 子网掩码查错——数据有效性
            if (netmask[3] > 252) throw new FormatException("无效的子网掩码。");

            // 记录可能最小地址和最大地址
            byte[] minAddress = new byte[4], maxAddress = new byte[4];

            // 通过子网掩码计算最小最大地址
            for (var i = 0; i < 4; i++) {
                minAddress[i] = (byte) (ipAddress[i] & netmask[i]);
                maxAddress[i] = (byte) (ipAddress[i] | (255 - netmask[i]));
            }

            // 保存到IP地址、网络号、子网掩码和广播地址
            _ipv4Address = new IPAddress(ipAddress);
            _networkNumber = new IPAddress(minAddress);
            _broadcastAddress = new IPAddress(maxAddress);
            _netmask = new IPAddress(netmask);
        }

        /// <summary>
        ///     清空抓包缓冲区。
        /// </summary>
        protected void ClearCaptures() {
            lock (_rawCaptures) {
                _rawCaptures.Clear();
            }
        }
    }
}