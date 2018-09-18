using System;
using System.Collections.Generic;
using System.Threading;

namespace LAN_Spy.Model.Classes {
    /// <summary>
    ///     提供一个哈希表供快速查找以及较查找更慢的插入。
    /// </summary>
    public class HashTable {
        /// <summary>
        ///     存放哈希值及其对应对象的哈希表。
        /// </summary>
        private KeyValuePair<int, object>[] _table;

        /// <summary>
        ///     当前读线程的个数。
        /// </summary>
        private int _readerCount;

        /// <summary>
        ///     当前是否允许读（表示是否要进入临界区写）。
        /// </summary>
        private bool _isReadEnabled = true; 

        /// <summary>
        ///     折半查找哈希值对应的对象。
        /// </summary>
        /// <param name="key">指定对象的哈希值。</param>
        /// <returns>哈希值对应的对象，若未找到则返回 <see langword="null"/> 。</returns>
        public object this[int key] {
            get {
                while (!_isReadEnabled)
                    Thread.Sleep(100);
                _readerCount++;

                int min = 0, max = _table.Length - 1;
                while (min <= max) {
                    var ptr = _table[(min + max) / 2];
                    if (ptr.Key == key) {
                        _readerCount--;
                        return ptr.Value;
                    }
                    if (ptr.Key < key)
                        max = (min + max) / 2 - 1;
                    else
                        min = (min + max) / 2 + 1;
                }

                _readerCount--;
                return null;
            }
        }

        /// <summary>
        ///     以一组对象初始化 <see cref="HashTable"/> 对象的实例。
        /// </summary>
        /// <param name="objects">对象集合。</param>
        /// <exception cref="ArgumentException">检测到哈希值碰撞。</exception>
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

        /// <summary>
        ///     向表中插入一个新对象并关闭读权限直到插入完成。
        /// </summary>
        /// <param name="key">插入对象的哈希值。</param>
        /// <param name="value">插入对象的指针。</param>
        /// <exception cref="ArgumentException">检测到哈希值碰撞。</exception>
        public void Add(int key, object value) {
            if (_table.Length == 0) {
                _table = new[] {new KeyValuePair<int, object>(key, value)};
                return;
            }

            var temp = new List<KeyValuePair<int, object>>(_table);
            int min = 0, max = temp.Count - 1;
            while (min < max) {
                var ptr = temp[(min + max) / 2];
                if (ptr.Key == key)
                    throw new ArgumentException("检测到哈希值碰撞。");
                if (ptr.Key < key)
                    max = (min + max) / 2 - 1;
                else
                    min = (min + max) / 2 + 1;
            }

            if (key == temp[min].Key)
                throw new ArgumentException("检测到哈希值碰撞。");

            if (key < temp[min].Key)
                temp.Insert(min, new KeyValuePair<int, object>(key, key));
            else
                temp.Insert(min + 1, new KeyValuePair<int, object>(key, key));

            temp.TrimExcess();

            _isReadEnabled = false;
            var sleeper = new WaitTimeoutChecker(30000);
            while (_readerCount != 0)
                sleeper.ThreadSleep(100);

            _table = temp.ToArray();

            _isReadEnabled = true;
        }

        /// <summary>
        ///     向表中插入一组对象并关闭读权限直到插入完成。
        /// </summary>
        /// <param name="objects">对象集合。</param>
        /// <exception cref="ArgumentException">检测到哈希值碰撞。</exception>
        public void AddRange(IEnumerable<KeyValuePair<int, object>> objects) {
            var temp = new List<KeyValuePair<int, object>>(_table);
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

            _isReadEnabled = false;
            var sleeper = new WaitTimeoutChecker(30000);
            while (_readerCount != 0)
                sleeper.ThreadSleep(100);

            _table = temp.ToArray();

            _isReadEnabled = true;
        }
    }
}
