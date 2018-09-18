//#define TEST_CONSOLE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using LAN_Spy.Controller.Classes;
using LAN_Spy.Model;
using LAN_Spy.Model.Classes;
using LAN_Spy.View;
using SharpPcap;
using SharpPcap.WinPcap;

namespace LAN_Spy.Controller {
    internal static class Program {
        /// <summary>
        ///     应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main() {
#if TEST_CONSOLE
            Test_in_Console();
#else
            // 初始化环境
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 检测WinPcap库
            try {
                CaptureDeviceList.New();
            }
            catch (Exception) {
                MessageBox.Show("本程序需要WinPcap支持，请确保WinPcap库正常工作！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 初始化模块
            TaskHandler.Init();
            Scanner scanner = null;
            Poisoner poisoner = null;
            Watcher watcher = null;
            var task = new Thread(load => {
                try {
                    var scannerThread = new Thread(init => { scanner = new Scanner(); });
                    var poisonerThread = new Thread(init => { poisoner = new Poisoner(); });
                    var watcherThread = new Thread(init => {
                        scannerThread.Start();
                        watcher = new Watcher();
                    });

                    watcherThread.Start();
                    poisonerThread.Start();

                    /*----------------------------------------------------------------
                                               （时间段1）
                            mainThread ---------------------------> watcherThread
                                |                                         |
                                |              （时间段2）                |
                                V                                         V
                          poisonerThread                            scannerThread
        
                                    将启动线程工作的一部分分摊给子线程
                    ----------------------------------------------------------------*/

                    new WaitTimeoutChecker(30000).ThreadSleep(500, () => scannerThread.IsAlive || poisonerThread.IsAlive || watcherThread.IsAlive);
                }
                catch (ThreadAbortException) {
                    Environment.Exit(-1);
                }
            }) {Name = RegisteredThreadName.ProgramInit.ToString()};
            MessagePipe.SendInMessage(new KeyValuePair<Message, Thread>(Message.TaskIn, task));
            var loading = new Loading("初始化中，请稍候", task);
            loading.ShowDialog();

            // 用户取消
            if (MessagePipe.GetNextOutMessage(task) == Message.UserCancel)
                Environment.Exit(-1);

            // 初始化完成（由loading判断得到）
            MessagePipe.ClearAllMessage(task);

            var models = new BasicClass[] {scanner, poisoner, watcher};
            Application.Run(new MainForm(ref models));
#endif
        }

        /// <summary>
        ///     控制台用测试程序。
        /// </summary>
        private static void Test_in_Console() {
            // 初始化
            var instance = CaptureDeviceList.Instance;
            var scanner = new Scanner();
            var poisoner = new Poisoner();

            // 打印当前可用设备列表
            var n = 0;
            var devList = new List<KeyValuePair<int, string>>();
            Console.WriteLine("Available devices: ");
            foreach (var item in instance) {
                var device = (WinPcapDevice) item;
                devList.Add(new KeyValuePair<int, string>(n, device.Interface.FriendlyName));
                Console.WriteLine($"{++n}. {device.Interface.FriendlyName}");
            }

            // 选择设备
            Console.WriteLine();
            Console.Write("Select using device: ");
            var index = int.Parse(Console.ReadLine() ?? throw new FormatException("Not valid number.")) - 1;
            if (index >= n) throw new IndexOutOfRangeException("No such device.");
            scanner.CurDevName = poisoner.CurDevName = devList.Find(item => item.Key == index).Value;

            // 输出地址数量并开始扫描
            Console.WriteLine();
            Console.Write($"Current network has {scanner.AddressCount} available addresses. Start scan? [Y/N]");
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
            Console.WriteLine($"Total hosts: {scanner.HostList.Count}");
            n = 1;
            foreach (var host in scanner.HostList) {
                Console.Write($"{n++}. {host.IPAddress} is at {host.PhysicalAddress}");
                if (scanner.GatewayAddresses.Contains(host.IPAddress))
                    Console.WriteLine(" (Possible Gateway Address)");
                else if (Equals(host.IPAddress, scanner.Ipv4Address))
                    Console.WriteLine(" (Possible Device Address)");
                else
                    Console.WriteLine();
            }

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
                foreach (var gateway in scanner.GatewayAddresses) {
                    if (!Equals(host.IPAddress, gateway)) continue;
                    Console.Write($"Gateway detected({host.IPAddress}). Use it? [Y/N]");
                    if (Console.ReadLine()?.ToUpperInvariant() != "Y")
                        break;
                    poisoner.Gateway = host;
                    flag = true;
                    break;
                }
                if (flag) break;
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
                var watcher = new Watcher {CurDevName = devList.Find(item => item.Key == index).Value};
                try {
                    watcher.StartWatching();
                    while (true) {
                        Thread.Sleep(10 * 1000);
                        Console.WriteLine($"Current time: {DateTime.Now.ToLongTimeString()}");
                        var c = 0;
                        foreach (var tcpLink in watcher.TcpLinks)
                            Console.WriteLine($"{++c}. {tcpLink.SrcAddress} -> {tcpLink.DstAddress}");
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