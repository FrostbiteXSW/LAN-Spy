using System;
using System.Collections.Generic;

namespace LAN_Spy.Controller {
    /// <summary>
    ///     控制器传递消息内容。
    /// </summary>
    public enum Message {
        /// <summary>
        ///     当前消息队列中无待接收的消息。
        /// </summary>
        NoAvailableMessage = 0,
        
        /// <summary>
        ///     有新任务传递给 <see cref="TaskHandler"/> 处理。
        /// </summary>
        TaskIn = 100,

        /// <summary>
        ///     取消未处理完的任务。
        /// </summary>
        TaskCancel,

        /// <summary>
        ///     <see cref="TaskHandler"/> 已处理完的任务等待接收。
        /// </summary>
        TaskOut = 200,

        /// <summary>
        ///     任务已被丢弃。
        /// </summary>
        TaskAborted,

        /// <summary>
        ///     未找到指定名称线程。
        /// </summary>
        TaskNotFound,

        /// <summary>
        ///     用户请求取消操作。
        /// </summary>
        UserCancel
    }

    /// <summary>
    ///     用以传递消息给 <see cref="TaskHandler"/> 以及从 <see cref="TaskHandler"/> 接收消息的管道载体。
    /// </summary>
    public static class MessagePipe {
        /// <summary>
        ///     传入 <see cref="TaskHandler"/> 的消息队列。
        /// </summary>
        private static readonly Queue<KeyValuePair<Message, object>> InMessages = new Queue<KeyValuePair<Message, object>>();
        
        /// <summary>
        ///     从 <see cref="TaskHandler"/> 传出的消息队列。
        /// </summary>
        private static readonly Queue<KeyValuePair<Message, object>> OutMessages = new Queue<KeyValuePair<Message, object>>();
        
        /// <summary>
        ///     获取一个有关无消息的消息参数对新实例。
        /// </summary>
        private static KeyValuePair<Message, object> NoAvailableMessagePair => new KeyValuePair<Message, object>(Message.NoAvailableMessage, null);

        /// <summary>
        ///     获取待接收传入消息数量。
        /// </summary>
        public static int InCount { get { lock (InMessages) { return InMessages.Count; }}}

        /// <summary>
        ///     获取待接收传出消息数量。
        /// </summary>
        public static int OutCount { get { lock (OutMessages) { return OutMessages.Count; }}}

        /// <summary>
        ///     检查传入消息队列顶端的内容。
        /// </summary>
        public static KeyValuePair<Message, object> TopInMessage {
            get {
                if (InCount == 0) return NoAvailableMessagePair;
                lock (InMessages) { return InMessages.Peek(); }
            }
        }

        /// <summary>
        ///     检查传出消息队列顶端的内容。
        /// </summary>
        public static KeyValuePair<Message, object> TopOutMessage {
            get { 
                if (OutCount == 0) return NoAvailableMessagePair;
                lock (OutMessages) { return OutMessages.Peek(); }
            }
        }

        /// <summary>
        ///     获取下一个传入消息及其参数。
        /// </summary>
        /// <returns>返回消息参数对。</returns>
        public static KeyValuePair<Message, object> GetNextInMessage() {
            // 检查是否有消息传入
            if (InCount == 0)
                return NoAvailableMessagePair;
            
            // 获取传入消息
            lock (InMessages) { return InMessages.Dequeue(); }
        }

        /// <summary>
        ///     发送新的传入消息及其参数。
        /// </summary>
        /// <param name="inMessage">传入消息参数对。</param>
        public static void SendInMessage(KeyValuePair<Message, object> inMessage) {
            // 检查消息有效性
            if ((int) inMessage.Key < 100 || (int) inMessage.Key > 199)
                throw new Exception("无效的消息。");

            lock (InMessages) { InMessages.Enqueue(inMessage); }
        }
        
        /// <summary>
        ///     获取下一个传出消息及其参数。
        /// </summary>
        /// <returns>返回消息参数对。</returns>
        public static KeyValuePair<Message, object> GetNextOutMessage() {
            // 检查是否有消息传出
            if (OutCount == 0) 
                return NoAvailableMessagePair;

            // 获取传出消息
            lock (OutMessages) { return OutMessages.Dequeue(); }
        }

        /// <summary>
        ///     发送新的传出消息及其参数。
        /// </summary>
        /// <param name="outMessage">传出消息参数对。</param>
        public static void SendOutMessage(KeyValuePair<Message, object> outMessage) {
            // 检查消息有效性
            if ((int) outMessage.Key < 200 || (int) outMessage.Key > 299)
                throw new Exception("无效的消息。");

            lock (OutMessages) { OutMessages.Enqueue(outMessage); }
        }
    }
}
