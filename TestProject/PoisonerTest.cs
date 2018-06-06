using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using LAN_Spy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject {
    [TestClass]
    public class PoisonerTest {
        /// <summary>
        ///     测试 <see cref="Poisoner"/> 的 StartPoisoning 方法。
        /// </summary>
        [TestMethod]
        public void TestStartPoisoning() {
            Scanner scanner = new Scanner {CurDevIndex = 2};
            scanner.ScanForTarget();
            Poisoner poisoner = new Poisoner {CurDevIndex = 2};
            List<Host> target1 = new List<Host> {scanner.HostList[0]},
                target2 = new List<Host>(scanner.HostList);
            target2.RemoveAt(0);
            poisoner.Target1 = target1;
            poisoner.Target2 = target2;
            poisoner.StartPoisoning();
            Thread.Sleep(60 * 1000);
        }
    }
}