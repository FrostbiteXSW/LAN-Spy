using System;
using System.Collections.Generic;
using System.Net;
using PacketDotNet;

namespace LAN_Spy.Model.Classes {
    /// <summary>
    ///     网路上一组TCP连接的两端主机。
    /// </summary>
    public class TcpLink {
        /// <summary>
        ///     初始化 <see cref="TcpLink" /> 类的实例。
        /// </summary>
        /// <param name="srcAddress">连接的起点，推荐以当前子网的主机作为起点。</param>
        /// <param name="srcPort">连接起点的端口。</param>
        /// <param name="dstAddress">连接的终点，推荐以非当前子网的主机作为终点。</param>
        /// <param name="dstPort">连接终点的端口。</param>
        public TcpLink(IPAddress srcAddress, ushort srcPort, IPAddress dstAddress, ushort dstPort) {
            SrcAddress = srcAddress;
            DstAddress = dstAddress;
            SrcPort = srcPort;
            DstPort = dstPort;
            UpdateTime();
        }

        /// <summary>
        ///     连接的起点。
        /// </summary>
        public IPAddress SrcAddress { get; }

        /// <summary>
        ///     连接的终点，
        /// </summary>
        public IPAddress DstAddress { get; }

        /// <summary>
        ///     连接起点的端口。
        /// </summary>
        public ushort SrcPort { get; }

        /// <summary>
        ///     连接终点的端口，
        /// </summary>
        public ushort DstPort { get; }

        /// <summary>
        ///     最后一次检测到连接的数据包，此项由用户手动设置，供连接分析使用。
        /// </summary>
        public EthernetPacket LastPacket { get; set; }

        /// <summary>
        ///     最后检测到连接的时间，并非确切的连接开始时间，而是侦测到连接时的本地时间。
        /// </summary>
        public DateTime Time { get; private set; }

        /// <summary>
        ///     更新最后检测到连接的时间。
        /// </summary>
        public void UpdateTime() {
            Time = DateTime.Now;
        }

        /// <summary>
        ///     比较指定 <see cref="TcpLink" /> 对象是否与当前对象相同。
        /// </summary>
        /// <param name="obj">需要比较的对象。</param>
        /// <returns>如果两个对象表示的连接相同返回 <see langword="true" />，否则返回<see langword="false" />。</returns>
        public bool Equals(TcpLink obj) {
            if (obj is null) return false;

            return obj.SrcAddress.Equals(SrcAddress)
                && obj.SrcPort.Equals(SrcPort)
                && obj.DstAddress.Equals(DstAddress)
                && obj.DstPort.Equals(DstPort);
        }

        /// <summary>
        ///     获取对象的哈希值。
        /// </summary>
        /// <returns>对象的哈希值。</returns>
        public override int GetHashCode() {
            return (SrcAddress.ToString() + DstAddress + SrcPort + DstPort).GetHashCode();
        }

        /// <summary>
        ///     为 <see cref="TcpLink" /> 对象排序提供比较方法
        /// </summary>
        /// <param name="a">第一个排序对象</param>
        /// <param name="b">第二个排序对象</param>
        /// <returns>默认以IP地址及端口升序规则返回结果</returns>
        public static int SortMethod(TcpLink a, TcpLink b) {
            int result;

            // 比较源地址
            var aBytes = a.SrcAddress.GetAddressBytes();
            var bBytes = b.SrcAddress.GetAddressBytes();
            try {
                for (var i = 0; i < 4; i++) {
                    if ((aBytes[i] & 0xFF) < (bBytes[i] & 0xFF))
                        return -1;
                    if ((aBytes[i] & 0xFF) > (bBytes[i] & 0xFF))
                        return 1;
                }
            }
            catch (Exception) {
                result = string.CompareOrdinal(a.SrcAddress.ToString(), b.SrcAddress.ToString());
                if (result != 0) return result;
            }

            // 比较源端口
            if (a.SrcPort > b.SrcPort) return 1;
            if (a.SrcPort < b.SrcPort) return -1;

            // 比较目标地址
            aBytes = a.DstAddress.GetAddressBytes();
            bBytes = b.DstAddress.GetAddressBytes();
            try {
                for (var i = 0; i < 4; i++) {
                    if ((aBytes[i] & 0xFF) < (bBytes[i] & 0xFF))
                        return -1;
                    if ((aBytes[i] & 0xFF) > (bBytes[i] & 0xFF))
                        return 1;
                }
            }
            catch (Exception) {
                result = string.CompareOrdinal(a.SrcAddress.ToString(), b.SrcAddress.ToString());
                if (result != 0) return result;
            }

            // 比较目标端口
            if (a.DstPort > b.DstPort) return 1;
            if (a.DstPort < b.DstPort) return -1;

            // 相等元素（理论上不可达，若出现此现象请检查）
            return 0;
        }
    }

    /// <inheritdoc />
    /// <summary>
    ///     供 <see cref="T:LAN_Spy.Model.Classes.TcpLink" /> 类使用的比较器。
    /// </summary>
    public class TcpLinkEqualityComparer : IEqualityComparer<TcpLink> {
        public bool Equals(TcpLink x, TcpLink y) {
            if (x is null || y is null)
                return x is null && y is null;

            return x.SrcAddress.Equals(y.SrcAddress)
                && x.SrcPort.Equals(y.SrcPort)
                && x.DstAddress.Equals(y.DstAddress)
                && x.DstPort.Equals(y.DstPort);
        }

        public int GetHashCode(TcpLink obj) {
            return (obj.SrcAddress.ToString() + obj.DstAddress + obj.SrcPort + obj.DstPort).GetHashCode();
        }
    }
}