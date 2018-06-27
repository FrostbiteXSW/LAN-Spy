using System;
using System.Threading;
using LAN_Spy.Model;
using SharpPcap;
using SharpPcap.WinPcap;

namespace LAN_Spy {
    internal static class Program {
        private static void Main() {
            // 初始化
            var instance = CaptureDeviceList.Instance;
            var scanner = new Scanner();
            var poisoner = new Poisoner();

            // 打印当前可用设备列表
            var n = 0;
            Console.WriteLine("Available devices: ");
            foreach (var item in instance) {
                var device = (WinPcapDevice) item;
                Console.WriteLine(++n + ". " + device.Interface.FriendlyName);
            }

            // 选择设备
            Console.WriteLine();
            Console.Write("Select using device: ");
            var index = int.Parse(Console.ReadLine() ?? throw new FormatException("Not valid number.")) - 1;
            if (index >= n) throw new IndexOutOfRangeException("No such device.");
            scanner.CurDevIndex = poisoner.CurDevIndex = index;

            // 输出地址数量并开始扫描
            Console.WriteLine();
            Console.Write("Current network has " + scanner.AddressCount + " available addresses. Start scan? [Y/N]");
            if (Console.ReadLine()?.ToUpperInvariant() != "Y") {
                Console.WriteLine("Process interrupted.");
                return;
            }
            Console.WriteLine("Scanning...");
            scanner.ScanForTarget();

            // 开始被动监听
            Console.WriteLine("Spying...");
            var t = new Thread(scanner.SpyForTarget);
            t.Start();
            Thread.Sleep(10 * 1000);
            t.Abort();
            while (t.IsAlive) Thread.Sleep(100);

            // 打印检测到的主机列表
            Console.WriteLine();
            Console.WriteLine("Total hosts: " + scanner.HostList.Count);
            n = 1;
            foreach (var host in scanner.HostList)
                if (Equals(host.IPAddress, scanner.GatewayAddress))
                    Console.WriteLine(n++ + ". " + host.IPAddress + " is at " + host.PhysicalAddress + " (Possible Gateway Address)");
                else if (Equals(host.IPAddress, scanner.Ipv4Address))
                    Console.WriteLine(n++ + ". " + host.IPAddress + " is at " + host.PhysicalAddress + " (Possible Device Address)");
                else
                    Console.WriteLine(n++ + ". " + host.IPAddress + " is at " + host.PhysicalAddress);

            // 选择目标
            Console.WriteLine();
            Console.Write("Select target1: ");
            var targets = Console.ReadLine()?.Split(' ');
            if (targets == null) throw new FormatException("Not valid numbers.");
            foreach (var target in targets) {
                var tindex = int.Parse(target);
                if (tindex >= n) throw new IndexOutOfRangeException("No such host.");
                poisoner.Target1.Add(scanner.HostList[tindex - 1]);
            }
            Console.Write("Select target2: ");
            targets = Console.ReadLine()?.Split(' ');
            if (targets == null) throw new FormatException("Not valid numbers.");
            foreach (var target in targets) {
                var tindex = int.Parse(target);
                if (tindex >= n) throw new IndexOutOfRangeException("No such host.");
                poisoner.Target2.Add(scanner.HostList[tindex - 1]);
            }

            // 设定默认网关
            var flag = false;
            foreach (var host in scanner.HostList) {
                if (!Equals(host.IPAddress, scanner.GatewayAddress)) continue;
                Console.Write("Gateway detected(" + host.IPAddress + "). Use it? [Y/N]");
                if (Console.ReadLine()?.ToUpperInvariant() != "Y")
                    break;
                poisoner.Gateway = host;
                flag = true;
                break;
            }
            if (!flag) {
                Console.Write("Select gateway: ");
                poisoner.Gateway = scanner.HostList[int.Parse(Console.ReadLine() ?? throw new FormatException()) - 1];
            }

            // 开始毒化
            Console.WriteLine();
            Console.WriteLine("Poisoning...");
            poisoner.StartPoisoning();
            Console.WriteLine("You can press any key to stop when this program is working.");

            // 创建监视器线程并监视可能存在的连接
            Console.WriteLine();
            var watchThread = new Thread(watch => {
                var watcher = new Watcher {CurDevIndex = index};
                try {
                    watcher.StartWatching();
                    while (true) {
                        Thread.Sleep(10 * 1000);
                        Console.WriteLine("Current time: " + DateTime.Now.ToLongTimeString());
                        var c = 0;
                        foreach (var tcpLink in watcher.TcpLinks)
                            Console.WriteLine(++c + ". " + tcpLink.Src + " -> " + tcpLink.Dst);
                        Console.WriteLine();
                    }
                }
                catch (ThreadAbortException) { }
                finally {
                    watcher.StopWatching();
                    watcher.Reset();
                }
            });
            watchThread.Start();

            // 结束程序
            Console.ReadKey();
            poisoner.StopPoisoning();
            poisoner.Reset();
            watchThread.Abort();
            var waitTime = 60 * 1000;
            while (true) {
                if (!watchThread.IsAlive) break;
                Thread.Sleep(1000);
                if ((waitTime -= 1000) != 0) continue;
                Console.WriteLine("Oops, something goes wrong. Force quit.");
                Environment.Exit(-1);
            }
            Console.WriteLine("Stopped. Thanks for using.");
        }
    }
}