using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LAN_Spy.View;

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
        ///     有新任务传递给 <see cref="TaskHandler" /> 处理。
        /// </summary>
        TaskIn = 100,

        /// <summary>
        ///     取消未处理完的任务。
        /// </summary>
        TaskCancel,

        /// <summary>
        ///     <see cref="TaskHandler" /> 已处理完的任务等待接收。
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
    ///     用以传递消息给 <see cref="TaskHandler" /> 以及从 <see cref="TaskHandler" /> 接收消息的管道载体。
    /// </summary>
    public static class MessagePipe {
        /// <summary>
        ///     传入 <see cref="TaskHandler" /> 的消息队列。
        /// </summary>
        private static readonly List<KeyValuePair<Message, Thread>> InMessages = new List<KeyValuePair<Message, Thread>>();

        /// <summary>
        ///     从 <see cref="TaskHandler" /> 传出的消息队列。
        /// </summary>
        private static readonly List<KeyValuePair<Message, Thread>> OutMessages = new List<KeyValuePair<Message, Thread>>();

        /// <summary>
        ///     获取下一个传入消息及其参数。
        /// </summary>
        /// <returns>返回消息参数对。</returns>
        public static KeyValuePair<Message, Thread> GetNextInMessage() {
            // 获取传入消息
            lock (InMessages) {
                if (InMessages.Count == 0)
                    return new KeyValuePair<Message, Thread>(Message.NoAvailableMessage, new Thread(empty => { }) {Name = ""});
                var msg = InMessages[0];
                InMessages.RemoveAt(0);
                return msg;
            }
        }

        /// <summary>
        ///     发送新的传入消息及其参数。
        /// </summary>
        /// <param name="inMessage">传入消息参数对。</param>
        public static void SendInMessage(KeyValuePair<Message, Thread> inMessage) {
            // 检查消息有效性
            if ((int) inMessage.Key < 100 || (int) inMessage.Key > 199)
                throw new Exception("无效的消息。");

            lock (InMessages) {
                InMessages.Add(inMessage);
            }
        }

        /// <summary>
        ///     获取对应任务的下一个传出消息。
        /// </summary>
        /// <param name="task">查询的任务。</param>
        /// <returns>返回最早的消息。</returns>
        public static Message GetNextOutMessage(Thread task) {
            lock (OutMessages) {
                // 检查是否有消息传出
                if (OutMessages.All(item => item.Value.Name != task.Name))
                    return Message.NoAvailableMessage;

                // 获取传出消息
                var msg = OutMessages.First(item => item.Value.Name == task.Name);
                OutMessages.Remove(msg);
                return msg.Key;
            }
        }

        /// <summary>
        ///     发送新的传出消息及其参数。
        /// </summary>
        /// <param name="outMessage">传出消息参数对。</param>
        public static void SendOutMessage(KeyValuePair<Message, Thread> outMessage) {
            // 检查消息有效性
            if ((int) outMessage.Key < 200 || (int) outMessage.Key > 299)
                throw new Exception("无效的消息。");

            lock (OutMessages) {
                OutMessages.Add(outMessage);
            }
        }

        /// <summary>
        ///     清除所有与某一任务有关的消息。
        /// </summary>
        /// <param name="task">需要清除的任务。</param>
        public static void ClearAllMessage(Thread task) {
            lock (InMessages) {
                InMessages.RemoveAll(item => item.Value.Name == task.Name);
            }
            lock (OutMessages) {
                OutMessages.RemoveAll(item => item.Value.Name == task.Name);
            }
        }

        /// <summary>
        ///     供载入窗口检查任务是否完成，此方法不会从列表中移除消息。
        /// </summary>
        /// <param name="loading">调用方法的载入窗口。</param>
        /// <param name="task">查询的任务。</param>
        /// <returns>返回最早的消息。</returns>
        public static Message GetNextOutMessage(this Loading loading, Thread task) {
            // 检查调用者有效性
            if (loading is null)
                throw new NullReferenceException("方法的调用者不能为空。");

            lock (OutMessages) {
                // 检查是否有消息传出
                return OutMessages.All(item => item.Value.Name != task.Name) ? Message.NoAvailableMessage : OutMessages.First(item => item.Value.Name == task.Name).Key;
            }
        }
    }
}