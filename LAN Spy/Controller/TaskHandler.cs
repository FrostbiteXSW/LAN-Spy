using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
                            var task = (Thread) message.Value;
                            task.Start();
                            WorkThreads.Add(task);
                            break;

                        // 取消任务
                        case Message.TaskCancel:
                            MessagePipe.GetNextInMessage();

                            // 查找目标
                            var target = WorkThreads.Find(item => item.Name == ((Thread) message.Value).Name);
                            if (target is null) {
                                MessagePipe.SendOutMessage(new KeyValuePair<Message, object>(Message.TaskNotFound, message.Value));
                                break;
                            }

                            // 尝试中止任务
                            WorkThreads.Remove(target);
                            target.Abort();

                            // 等待任务结束
                            var waitTime = 0;
                            while (target.IsAlive) {
                                Thread.Sleep(500);
                                if ((waitTime += 500) == 30000)
                                    throw new TimeoutException("任务长时间未能中止。");
                            }

                            // 任务成功中止
                            MessagePipe.SendOutMessage(new KeyValuePair<Message, object>(Message.TaskAborted, message.Value));
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
                    WorkThreads.ForEach(workThread => {
                        if (!workThread.IsAlive)
                            MessagePipe.SendOutMessage(new KeyValuePair<Message, object>(Message.TaskOut, workThread));
                    });
                    WorkThreads.RemoveAll(workThread => !workThread.IsAlive);
                    Thread.Sleep(1000);
                }
            }
            catch (ThreadAbortException) { }
        });

        /// <summary> 
        ///     初始化 <see cref="TaskHandler"/> 的线程容器。
        /// </summary>
        public static void Init() {
            Stop();
            MessageReceiver.Start();
            Inspector.Start();
        }

        /// <summary>
        ///     终止 <see cref="TaskHandler"/> 的运行。
        /// </summary>
        public static void Stop() {
            if (WorkThreads.Count == 0) return;
            WorkThreads.ForEach(thread => {thread.Abort();});
            MessageReceiver.Abort();
            Inspector.Abort();
            var waitTime = 0;
            while (WorkThreads.Count(thread => thread.IsAlive) > 0 || MessageReceiver.IsAlive || Inspector.IsAlive) {
                Thread.Sleep(500);
                if ((waitTime += 500) == 30000)
                    throw new TimeoutException("等待线程结束超时。");
            }
            WorkThreads.Clear();
        }
    }
}
