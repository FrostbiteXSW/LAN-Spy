using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LAN_Spy.Model.Classes;

namespace LAN_Spy.Controller {
    /// <summary>
    ///     控制View后台工作的独立线程控制器。
    /// </summary>
    public static class TaskHandler {
        /// <summary>
        ///     工作线程队列。
        /// </summary>
        private static readonly List<Thread> WorkThreads = new List<Thread>();

        /// <summary>
        ///     消息接收线程。
        /// </summary>
        private static readonly Thread MessageReceiver = new Thread(work => {
            try {
                while (true) {
                    // 接收下一个消息
                    var message = MessagePipe.GetNextInMessage();

                    // 根据消息类型进行处理
                    switch (message.Key) {
                        // 新任务到达
                        case Message.TaskIn:
                            var task = message.Value;
                            task.Start();
                            lock (WorkThreads) {
                                WorkThreads.Add(task);
                            }
                            break;

                        // 取消任务
                        case Message.TaskCancel:
                            MessagePipe.GetNextInMessage();

                            // 查找目标
                            if (WorkThreads.All(item => item.Name != message.Value.Name)) {
                                MessagePipe.SendOutMessage(new KeyValuePair<Message, Thread>(Message.TaskNotFound, message.Value));
                                break;
                            }
                            Thread target;
                            lock (WorkThreads) {
                                target = WorkThreads.Find(item => item.Name == message.Value.Name);
                            }

                            // 尝试中止任务
                            lock (WorkThreads) {
                                WorkThreads.Remove(target);
                            }
                            target.Abort();

                            // 等待任务结束
                            new WaitTimeoutChecker(30000).ThreadSleep(500, func => target.IsAlive);

                            // 任务成功中止
                            MessagePipe.SendOutMessage(new KeyValuePair<Message, Thread>(Message.TaskAborted, message.Value));
                            break;

                        // 无等待接收消息
                        case Message.NoAvailableMessage:
                            Thread.Sleep(1000);
                            break;

                        // 无效消息
                        default:
                            throw new Exception($"消息队列传出无效消息：{message.Key.ToString()}");
                    }
                }
            }
            catch (ThreadAbortException) { }
        });

        /// <summary>
        ///     线程运行监视器。
        /// </summary>
        private static readonly Thread Inspector = new Thread(work => {
            try {
                while (true) {
                    lock (WorkThreads) {
                        WorkThreads.ForEach(workThread => {
                            if (!workThread.IsAlive)
                                MessagePipe.SendOutMessage(new KeyValuePair<Message, Thread>(Message.TaskOut, workThread));
                        });
                        WorkThreads.RemoveAll(workThread => !workThread.IsAlive);
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (ThreadAbortException) { }
        });

        /// <summary>
        ///     初始化 <see cref="TaskHandler" /> 的线程容器。
        /// </summary>
        public static void Init() {
            Stop();
            MessageReceiver.Start();
            Inspector.Start();
        }

        /// <summary>
        ///     终止 <see cref="TaskHandler" /> 的运行。
        /// </summary>
        public static void Stop() {
            if (MessageReceiver.IsAlive) MessageReceiver.Abort();
            if (Inspector.IsAlive) Inspector.Abort();
            if (WorkThreads.Count == 0) return;
            WorkThreads.ForEach(thread => {
                if (thread.IsAlive) thread.Abort();
            });
            new WaitTimeoutChecker(30000).ThreadSleep(500, func => WorkThreads.Any(thread => thread.IsAlive) || MessageReceiver.IsAlive || Inspector.IsAlive);
            WorkThreads.Clear();
        }
    }
}