using System;
using System.Collections.Generic;
using System.Threading;

namespace LAN_Spy.Controller {
    /// <summary>
    ///     控制View后台工作的独立线程控制器。
    /// </summary>
    public static class TaskHandler {
        /// <summary>
        ///     工作线程。
        /// </summary>
        private static Thread _workThread;

        /// <summary> 
        ///     初始化 <see cref="TaskHandler"/> 的线程容器。
        /// </summary>
        public static void Init() {
            Stop();
            _workThread = new Thread(work => {
                Thread task = null;
                var message = new KeyValuePair<Message, object>(Message.NoAvailableMessage, null);

                try {
                    while (true) {
                        // 接收下一个消息
                        message = MessagePipe.GetNextInMessage();

                        // 根据消息类型进行处理
                        switch (message.Key) {
                            // 新任务到达
                            case Message.TaskIn:
                                task = (Thread) message.Value;
                                task.Start();
                                while (task.IsAlive) {
                                    if (MessagePipe.TopInMessage.Key == Message.TaskCancel) {
                                        // 尝试中止任务
                                        MessagePipe.GetNextInMessage();
                                        task.Abort();
                                        var guard = task;
                                        task = null;

                                        // 等待任务结束
                                        var waitTime = 0;
                                        while (guard.IsAlive) {
                                            Thread.Sleep(500);
                                            waitTime += 500;
                                            if (waitTime == 30000)
                                                throw new TimeoutException("任务长时间未能中止。");
                                        }

                                        // 任务中止
                                        MessagePipe.SendOutMessage(new KeyValuePair<Message, object>(Message.TaskAborted, message.Value));
                                        break;
                                    }
                                    Thread.Sleep(500);
                                }

                                // 任务完成
                                if (task == null) break;
                                MessagePipe.SendOutMessage(new KeyValuePair<Message, object>(Message.TaskOut, message.Value));
                                task = null;
                                break;

                            // 无等待接收消息
                            case Message.NoAvailableMessage:
                                Thread.Sleep(1000);
                                break;
                        }
                    }
                }
                catch (ThreadAbortException) {
                    if (!(task is null) && task.IsAlive) {
                        task.Abort();
                        var waitTime = 0;
                        while (task.IsAlive) {
                            Thread.Sleep(500);
                            waitTime += 500;
                            if (waitTime == 30000)
                                throw new TimeoutException("任务长时间未能中止。");
                        }
                        MessagePipe.SendOutMessage(new KeyValuePair<Message, object>(Message.TaskAborted, message.Value));
                    }
                }
            });
            _workThread.Start();
        }

        /// <summary>
        ///     终止 <see cref="TaskHandler"/> 的运行。
        /// </summary>
        public static void Stop() {
            if (_workThread is null) return;
            _workThread.Abort();
            while (_workThread.IsAlive)
                Thread.Sleep(500);
            _workThread = null;
        }
    }
}
