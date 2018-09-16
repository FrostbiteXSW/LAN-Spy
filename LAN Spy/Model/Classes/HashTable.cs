using System;
using System.Collections.Generic;

namespace LAN_Spy.Model.Classes {
    /// <summary>
    ///     提供一个哈希表供快速查找。
    /// </summary>
    public class HashTable {
        /// <summary>
        ///     存放哈希值及其对应对象的哈希表。
        /// </summary>
        private readonly KeyValuePair<int, object>[] _table;

        /// <summary>
        ///     折半查找哈希值对应的对象。
        /// </summary>
        /// <param name="key">指定对象的哈希值。</param>
        /// <returns>哈希值对应的对象，若未找到则返回 <see langword="null"/> 。</returns>
        public object this[int key] {
            get {
                int min = 0, max = _table.Length - 1;

                while (min <= max) {
                    var ptr = _table[(min + max) / 2];
                    if (ptr.Key == key)
                        return ptr.Value;
                    if (ptr.Key < key)
                        max = (min + max) / 2 - 1;
                    else
                        min = (min + max) / 2 + 1;
                }

                return null;
            }
        }

        /// <summary>
        ///     以一组对象初始化 <see cref="HashTable"/> 对象的实例，此实例的哈希表在初始化后不可更改。
        /// </summary>
        /// <param name="objects">对象集合。</param>
        public HashTable(IEnumerable<KeyValuePair<int, object>> objects) {
            var temp = new List<KeyValuePair<int, object>>();

            foreach (var item in objects) {
                if (temp.Count == 0) {
                    temp.Add(new KeyValuePair<int, object>(item.Key, item.Value));
                    continue;
                }

                int min = 0, max = temp.Count - 1;

                while (min < max) {
                    var ptr = temp[(min + max) / 2];
                    if (ptr.Key == item.Key)
                        throw new ArgumentException("检测到哈希值碰撞。");
                    if (ptr.Key < item.Key)
                        max = (min + max) / 2 - 1;
                    else
                        min = (min + max) / 2 + 1;
                }

                if (item.Key == temp[min].Key)
                    throw new ArgumentException("检测到哈希值碰撞。");

                if (item.Key < temp[min].Key)
                    temp.Insert(min, new KeyValuePair<int, object>(item.Key, item.Value));
                else
                    temp.Insert(min + 1, new KeyValuePair<int, object>(item.Key, item.Value));
            }

            temp.TrimExcess();
            _table = temp.ToArray();
        }
    }
}
