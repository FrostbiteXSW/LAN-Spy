﻿using System;
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
    }
}