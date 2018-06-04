using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PacketDotNet;
using SharpPcap;
using SharpPcap.WinPcap;
using LAN_Spy;

namespace TestProject {
    [TestClass]
    public class UnitTest1 {
        /// <summary>
        ///     测试通过 <see cref="WinPcapDevice"/> 获取IP地址、子网掩码和MAC地址。
        /// </summary>
        [TestMethod]
        public void TestGetAddresses() {
            WinPcapDevice device = (WinPcapDevice)CaptureDeviceList.Instance[2];

            // 设备IPv6地址
            Trace.WriteLine(device.Addresses[0].Addr.ipAddress);

            // 设备IPv4地址
            Trace.WriteLine(device.Addresses[1].Addr.ipAddress);
            // 设备IPv4地址子网掩码
            Trace.WriteLine(device.Addresses[1].Netmask.ipAddress);

            // 设备MAC地址
            Trace.WriteLine(device.Addresses[2].Addr.hardwareAddress);

            /* Addr.sa_family的含义（按照在Addresses里的顺序先后排列）：
             * 23：IPv6
             * 2：IPv4
             * 0：MAC
             */
        }

        /// <summary>
        ///     测试 <see cref="Scanner"/> 的 CalculateAddressRange 方法。
        /// </summary>
        [TestMethod]
        public void TestCalculateAddressRange() {
            Scanner scanner = new Scanner {CurDevIndex = 2};
            scanner.CalculateAddressRange();
            Trace.WriteLine(scanner.AddressCount);
        }
    }
}
