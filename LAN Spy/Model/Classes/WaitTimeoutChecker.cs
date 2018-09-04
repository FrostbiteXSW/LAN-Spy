using System;
using System.Threading;

namespace LAN_Spy.Model.Classes {
    /// <summary>
    ///     为线程等待超时提供自动检查支持。
    /// </summary>
    public class WaitTimeoutChecker {
        /// <summary>
        ///     指示是否在超时后抛出异常。
        /// </summary>
        private readonly bool _useException = true;

        /// <summary>
        ///     等待时间。
        /// </summary>
        private int _time;

        /// <summary>
        ///     初始化 <see cref="WaitTimeoutChecker" /> 类的实例。
        /// </summary>
        /// <param name="time">超时时间。</param>
        public WaitTimeoutChecker(int time) {
            _time = time;
        }

        /// <summary>
        ///     初始化 <see cref="WaitTimeoutChecker" /> 类的实例。
        /// </summary>
        /// <param name="time">超时时间。</param>
        /// <param name="useException">指示是否在超时后抛出 <see cref="TimeoutException" /> 异常，默认抛出。</param>
        public WaitTimeoutChecker(int time, bool useException) {
            _time = time;
            _useException = useException;
        }

        /// <summary>
        ///     让当前线程等待一段时间，并检测是否超时，如果超时时间耗尽将不会等待。
        /// </summary>
        /// <param name="time">等待的时间长度，以毫秒为单位。</param>
        /// <returns>若等待未超时返回true，否则返回false。</returns>
        /// <exception cref="TimeoutException">等待已超时。</exception>
        public bool ThreadSleep(int time) {
            if (_time <= 0) return false;
            Thread.Sleep(Math.Min(time, _time));
            if ((_time -= time) <= 0 && _useException)
                throw new TimeoutException("等待已超时。");
            return _time > 0;
        }
    }
}