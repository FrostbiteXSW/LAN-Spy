using System.Net;
using System.Net.NetworkInformation;

namespace LAN_Spy.Model.Classes {
    /// <summary>
    ///     为存储主机IP地址及其对应物理地址提供数据结构支持。
    /// </summary>
    public class Host {
        /// <summary>
        ///     获取或设置主机的IP地址。
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public IPAddress IPAddress;

        /// <summary>
        ///     获取主机的物理地址。
        /// </summary>
        public PhysicalAddress PhysicalAddress { get; }

        /// <summary>
        ///     初始化 <see cref="Host"/> 类的实例。
        /// </summary>
        /// <param name="ipAddress">IP地址。</param>
        /// <param name="physicalAddress">物理地址。</param>
        public Host(IPAddress ipAddress, PhysicalAddress physicalAddress) {
            IPAddress = ipAddress;
            PhysicalAddress = physicalAddress;
        }
    }
}
