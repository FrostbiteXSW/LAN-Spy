using System;
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
        ///     初始化 <see cref="Host" /> 类的实例。
        /// </summary>
        /// <param name="ipAddress">IP地址。</param>
        /// <param name="physicalAddress">物理地址。</param>
        public Host(IPAddress ipAddress, PhysicalAddress physicalAddress) {
            IPAddress = ipAddress;
            PhysicalAddress = physicalAddress;
        }

        /// <summary>
        ///     获取主机的物理地址。
        /// </summary>
        public PhysicalAddress PhysicalAddress { get; }

        /// <summary>
        ///     为 <see cref="Host" /> 对象排序提供比较方法
        /// </summary>
        /// <param name="a">第一个排序对象</param>
        /// <param name="b">第二个排序对象</param>
        /// <returns>默认以IP地址升序规则返回结果</returns>
        public static int SortMethod(Host a, Host b) {
            var aBytes = a.IPAddress.GetAddressBytes();
            var bBytes = b.IPAddress.GetAddressBytes();

            try {
                for (var i = 0; i < 4; i++) {
                    if ((aBytes[i] & 0xFF) < (bBytes[i] & 0xFF))
                        return -1;
                    if ((aBytes[i] & 0xFF) > (bBytes[i] & 0xFF))
                        return 1;
                }
                return 0;
            }
            catch (Exception) {
                return string.CompareOrdinal(a.IPAddress.ToString(), b.IPAddress.ToString());
            }
        }
    }
}