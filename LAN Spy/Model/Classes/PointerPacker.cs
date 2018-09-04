namespace LAN_Spy.Model.Classes {
    /// <summary>
    ///     封装指定数据类型为类以供指针传递。
    /// </summary>
    public class PointerPacker {
        /// <summary>
        ///     初始化 <see cref="PointerPacker" /> 类的实例。
        /// </summary>
        /// <param name="item">需要包装的数据。</param>
        public PointerPacker(object item) {
            Item = item;
        }

        /// <summary>
        ///     初始化 <see cref="PointerPacker" /> 类的实例。
        /// </summary>
        public PointerPacker() {
            Item = new object();
        }

        public object Item { get; set; }
    }
}