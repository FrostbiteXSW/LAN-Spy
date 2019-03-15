using System.Diagnostics;
using System.Threading;
using LAN_Spy.Model;
using LAN_Spy.Model.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject {
    [TestClass]
    public class WatcherTest {
        /// <summary>
        ///     测试无Poisoner情况下分析本机连接。
        /// </summary>
        [TestMethod]
        public void TestLocalAnalyze() {
            var watcher = new Watcher {CurDevName = "WLAN"};
            watcher.StartWatching();
            Thread.Sleep(30 * 1000);
            ((BasicClass) watcher).Stop();
            foreach (var tcpLink in watcher.TcpLinks)
                Trace.WriteLine(tcpLink.SrcAddress + " --> " + tcpLink.DstAddress);
            watcher.Reset();
        }
    }
}