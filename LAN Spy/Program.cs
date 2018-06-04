using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;

namespace LAN_Spy {
    class Program {
        static void Main(string[] args) {
            var deviceList = CaptureDeviceList.Instance;
            foreach (var item in deviceList)
                Console.WriteLine(item.Description);
        }
    }
}
