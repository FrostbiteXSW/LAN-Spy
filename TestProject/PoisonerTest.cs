using LAN_Spy.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpPcap;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using LAN_Spy.Model.Classes;

namespace TestProject {
    [TestClass]
    public class PoisonerTest {
        /// <summary>
        ///     测试 <see cref="Poisoner" /> 的 StartPoisoning 方法。
        /// </summary>
        [TestMethod]
        public void TestStartPoisoning() {
            var scanner = new Scanner {CurDevIndex = 2};
            scanner.ScanForTarget();
            var poisoner = new Poisoner {CurDevIndex = 2};
            List<Host> target1 = new List<Host> {scanner.HostList[0]},
                target2 = new List<Host>(scanner.HostList);
            target2.RemoveAt(0);
            poisoner.Target1 = target1;
            poisoner.Target2 = target2;
            poisoner.StartPoisoning();
            Thread.Sleep(60 * 1000);
        }

        /// <summary>
        ///     测试两个线程使用不同过滤器同时抓包是否会产生冲突。
        /// </summary>
        [TestMethod]
        public void TestTwoCapture() {
            bool ipex = false, arpex = false;
            var thread1 = new Thread(capture1 => {
                try {
                    var device = CaptureDeviceList.New()[2];
                    device.Open();
                    device.OnPacketArrival += Device_OnPacketArrival1;
                    device.Filter = "ip";
                    device.StartCapture();
                    Thread.Sleep(30 * 1000);
                    device.OnPacketArrival -= Device_OnPacketArrival1;
                    device.StopCapture();
                    device.Close();
                }
                catch {
                    ipex = true;
                }
            });
            var thread2 = new Thread(capture2 => {
                try {
                    var device = CaptureDeviceList.New()[2];
                    device.Open();
                    device.OnPacketArrival += Device_OnPacketArrival2;
                    device.Filter = "arp";
                    device.StartCapture();
                    Thread.Sleep(30 * 1000);
                    device.OnPacketArrival -= Device_OnPacketArrival2;
                    device.StopCapture();
                    device.Close();
                } 
                catch {
                    arpex = true;
                }
            });
            thread1.Start();
            thread2.Start();
            while (thread1.IsAlive || thread2.IsAlive)
                Thread.Sleep(1000);
            if (ipex || arpex) {
                Trace.WriteLine("Exception happened.");
                return;
            }
            Trace.WriteLine("IP: " + _ip);
            Trace.WriteLine("ARP: " + _arp);

            /* 测试结果表明：
             *     同一个设备使用CaptureDeviceList.New()生成的不同实例
             * 在抓包时不会产生冲突，所有的Start、Stop、Open或是Close，
             * 甚至是绑定方法等等都视为独立设备，使用这种方法可以有效应
             * 用于多个线程使用不同过滤器同时抓包的场合。
             */
        }
        /// <summary>
        ///     适用于 <see cref="TestTwoCapture"/> 测试。
        /// </summary>
        private uint _ip, _arp;
        /// <summary>
        ///     适用于 <see cref="TestTwoCapture"/> 测试。
        /// </summary>
        private void Device_OnPacketArrival1(object sender, CaptureEventArgs e) {
            _ip++;
        }
        /// <summary>
        ///     适用于 <see cref="TestTwoCapture"/> 测试。
        /// </summary>
        private void Device_OnPacketArrival2(object sender, CaptureEventArgs e) {
            _arp++;
        }
    }
}