using System;
using System.Diagnostics;
using System.Threading;
using LAN_Spy.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpPcap;
using SharpPcap.WinPcap;

namespace TestProject {
    [TestClass]
    public class ScannerTest {
        /// <summary>
        ///     测试通过 <see cref="WinPcapDevice" /> 获取IP地址、子网掩码和MAC地址。
        /// </summary>
        [TestMethod]
        public void TestGetAddresses() {
            var n = 1;
            foreach (var item in CaptureDeviceList.Instance) {
                var device = (WinPcapDevice) item;
                Trace.WriteLine("Device " + n++ + ": " + device.Interface.FriendlyName);
                foreach (var address in device.Addresses)
                    switch (address.Addr.sa_family) {
                        case 23:
                            // IPv6
                            Trace.WriteLine("IPv6 address: " + address.Addr.ipAddress);
                            break;
                        case 2:
                            // IPv4
                            Trace.WriteLine("IPv4 address: " + address.Addr.ipAddress);
                            Trace.WriteLine("Netmask: " + address.Netmask.ipAddress);
                            break;
                        case 0:
                            // MAC
                            Trace.WriteLine("MAC address: " + address.Addr.hardwareAddress);
                            // Trace.WriteLine("MAC address: " + device.Interface.MacAddress);
                            break;
                        default:
                            throw new Exception("Unknown sa_family: " + address.Addr.sa_family);
                    }
                Trace.WriteLine("");
            }
        }

        /// <summary>
        ///     测试 <see cref="Scanner" /> 的 ScanForTarget 方法。
        /// </summary>
        [TestMethod]
        public void TestScanForTarget() {
            var scanner = new Scanner {CurDevName = "WLAN"};
            scanner.ScanForTarget();
            Trace.WriteLine("Total avaliable addresses: " + scanner.AddressCount);
            Trace.WriteLine("Total hosts: " + scanner.HostList.Count);
            foreach (var host in scanner.HostList)
                Trace.WriteLine(host.IPAddress + " is at " + host.PhysicalAddress);
        }

        /// <summary>
        ///     测试 <see cref="Scanner" /> 的 ScanForTarget 方法。
        /// </summary>
        [TestMethod]
        public void TestSpyForTarget() {
            var scanner = new Scanner {CurDevName = "WLAN"};
            var thread = new Thread(scanner.SpyForTarget);
            thread.Start();
            Thread.Sleep(30 * 1000);
            thread.Abort();
            while (thread.IsAlive)
                Thread.Sleep(100);
            Trace.WriteLine("Total hosts: " + scanner.HostList.Count);
            foreach (var host in scanner.HostList)
                Trace.WriteLine(host.IPAddress + " is at " + host.PhysicalAddress);
        }
    }
}