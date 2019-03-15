using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using LAN_Spy.Controller;
using LAN_Spy.Controller.Classes;
using LAN_Spy.Model;
using LAN_Spy.Model.Classes;
using SharpPcap;
using SharpPcap.WinPcap;
using Message = LAN_Spy.Controller.Message;

namespace LAN_Spy.View.MainForm {
    public partial class MainForm : Form {
        /// <inheritdoc />
        /// <summary>
        ///     初始化 <see cref="MainForm" /> 窗口。
        /// </summary>
        public MainForm() {
            // 初始化模块
            TaskHandler.Init();
            var task = new Thread(load => {
                try {
                    // TODO:新增模块时请更新此处的代码
                    var threads = new[] {
                        new Thread(init => { Scanner = new Scanner(); }),
                        new Thread(init => { Poisoner = new Poisoner(); }),
                        new Thread(init => { Watcher = new Watcher(); })
                    };
                    foreach (var thread in threads) thread.Start();
                    new WaitTimeoutChecker(30000).ThreadSleep(500, () => threads.Any(item => item.IsAlive));
                }
                catch (ThreadAbortException) {
                    Environment.Exit(-1);
                }
            }) {Name = RegisteredThreadName.ProgramInit.ToString()};
            MessagePipe.SendInMessage(new KeyValuePair<Message, Thread>(Message.TaskIn, task));
            var loading = new Loading("初始化中，请稍候", task);
            loading.ShowDialog();

            // 用户取消
            if (MessagePipe.GetNextOutMessage(task) == Message.UserCancel)
                Environment.Exit(-1);

            // 初始化完成（由loading判断得到）
            MessagePipe.ClearAllMessage(task);

            TopMost = true;
            InitializeComponent();
        }

        /// <summary>
        ///     获取所有模块的实例组合。
        /// </summary>
        // TODO:新增模块时请更新此处的代码
        private IEnumerable<BasicClass> Models => new BasicClass[] {Scanner, Poisoner, Watcher};

        /// <summary>
        ///     启用所有控件。
        /// </summary>
        private void EnableAllItems() {
            // TODO:新增模块时请更新此处的代码
            启动所有模块ToolStripMenuItem.Text = "重启所有模块";
            EnableScannerItems();
            EnablePoisonerItems();
            EnableWatcherItems();
        }

        /// <summary>
        ///     禁用所有控件。
        /// </summary>
        private void DisableAllItems() {
            // TODO:新增模块时请更新此处的代码
            启动所有模块ToolStripMenuItem.Text = "启动所有模块";
            DisableScannerItems();
            DisablePoisonerItems();
            DisableWatcherItems();
        }

        /// <summary>
        ///     菜单项“启动所有模块”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 启动所有模块ToolStripMenuItem_Click(object sender, EventArgs e) {
            // 判断是否为重启
            if (启动所有模块ToolStripMenuItem.Text == "重启所有模块") {
                var result = false;

                // 创建载入界面
                var task = new Thread(stop => { result = StopModels(); }) {Name = RegisteredThreadName.RestartStopAllModels.ToString()};
                MessagePipe.SendInMessage(new KeyValuePair<Message, Thread>(Message.TaskIn, task));
                var loading = new Loading("正在停止，请稍候", task);
                loading.ShowDialog();

                // 等待结果
                new WaitTimeoutChecker(30000).ThreadSleep(500, () => {
                    var msg = MessagePipe.GetNextOutMessage(task);
                    switch (msg) {
                    case Message.NoAvailableMessage:
                        return true;
                    case Message.TaskOut:
                        return false;
                    default:
                        throw new Exception($"无效的消息类型：{msg}");
                    }
                });

                // 模块已停止
                MessagePipe.ClearAllMessage(task);
                if (!result) {
                    MessageBox.Show("一个或多个模块未能成功停止，请检查。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DisableAllItems();
            }

            // 启动模块
            if (StartupModels(Models)) {
                if (Models.Any(item => item.CurDevName == "")) return;
                MessageBox.Show("所有模块均已成功设置。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EnableAllItems();
            }
            else {
                MessageBox.Show("一个或多个模块未能成功初始化，请检查。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     菜单项“停止所有模块”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 停止所有模块ToolStripMenuItem_Click(object sender, EventArgs e) {
            var result = false;

            // 创建载入界面
            var task = new Thread(stop => { result = StopModels(); }) {Name = RegisteredThreadName.StopAllModels.ToString()};
            MessagePipe.SendInMessage(new KeyValuePair<Message, Thread>(Message.TaskIn, task));
            var loading = new Loading("正在停止，请稍候", task);
            loading.ShowDialog();

            // 等待结果
            new WaitTimeoutChecker(30000).ThreadSleep(500, () => {
                var msg = MessagePipe.GetNextOutMessage(task);
                switch (msg) {
                case Message.NoAvailableMessage:
                    return true;
                case Message.TaskOut:
                    return false;
                default:
                    throw new Exception($"无效的消息类型：{msg}");
                }
            });

            // 模块已停止
            MessagePipe.ClearAllMessage(task);
            if (!result)
                MessageBox.Show("一个或多个模块未能成功停止，请检查。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("所有模块已停止。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DisableAllItems();
        }

        /// <summary>
        ///     启动指定的模块。
        /// </summary>
        /// <param name="models">需要启动的模块列表。</param>
        /// <returns>成功返回true，错误返回false。</returns>
        private static bool StartupModels(IEnumerable<BasicClass> models) {
            // 判断模块是否初始化
            var modelsList = models.ToList();
            if (modelsList.Contains(null))
                return false;

            // 选择设备
            var index = new PointerPacker("");
            var chooseDevice = new ChooseDevice(ref index);
            while (true) {
                chooseDevice.ShowDialog();

                // 用户点击了取消，中止本次操作
                var value = (string) index.Item;
                if (value.Length == 0) return true;

                // 检查设备网络是否有效
                var device = (WinPcapDevice) CaptureDeviceList.Instance[value];
                var isValid = false;
                foreach (var address in device.Addresses) {
                    if (address.Addr.sa_family != 2) continue;
                    isValid = true;
                    break;
                }

                if (isValid) break;
                var dialogResult = MessageBox.Show("无法确定指定设备的IPv4网络环境，仍然继续？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.OK) break;
            }

            // 创建载入界面
            var task = new Thread(init => {
                try {
                    foreach (var model in modelsList)
                        model.CurDevName = index.Item.ToString();
                }
                catch (ThreadAbortException) {
                    foreach (var model in modelsList)
                        model.CurDevName = "";
                }
            }) {Name = RegisteredThreadName.StartupModels.ToString()};
            MessagePipe.SendInMessage(new KeyValuePair<Message, Thread>(Message.TaskIn, task));
            var loading = new Loading("设置模块中，请稍候", task);
            loading.ShowDialog();

            // 等待结果
            var result = Message.NoAvailableMessage;
            new WaitTimeoutChecker(30000).ThreadSleep(500, () => (result = MessagePipe.GetNextOutMessage(task)) == Message.NoAvailableMessage);

            // 用户取消
            if (result == Message.UserCancel) {
                MessagePipe.SendInMessage(new KeyValuePair<Message, Thread>(Message.TaskCancel, task));
                new WaitTimeoutChecker(30000).ThreadSleep(500, () => (result = MessagePipe.GetNextOutMessage(task)) == Message.NoAvailableMessage);

                switch (result) {
                case Message.TaskAborted:
                    return false;
                case Message.TaskOut:
                    break;
                case Message.TaskNotFound:
                    MessageBox.Show("未能找到指定名称的工作线程。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                default:
                    throw new Exception($"无效的消息类型：{result}");
                }
            }

            // 任务完成
            MessagePipe.ClearAllMessage(task);
            return true;
        }

        /// <summary>
        ///     停止所有模块的工作。
        /// </summary>
        /// <returns>成功返回true，错误返回false。</returns>
        private bool StopModels() {
            var result = true;

            foreach (var model in Models)
                if (model is null) {
                    result = false;
                }
                else {
                    model.Stop();
                    model.Reset();
                    model.CurDevName = "";
                }

            return result;
        }

        /// <summary>
        ///     <see cref="MainForm" /> 激活时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void MainForm_Activated(object sender, EventArgs e) {
            TopMost = false;
            Activated -= MainForm_Activated;
        }

        /// <summary>
        ///     窗口关闭时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            Environment.Exit(0);
        }

        /// <summary>
        ///     右键菜单打开后触发的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void ContextMenuStrip_Opened(object sender, EventArgs e) {
            var source = ((ContextMenuStrip) sender).SourceControl;
            DataGridView list;

            switch (source.Name) {
            case "HostList":
                list = HostList;
                break;
            case "Target1List":
                list = Target1List;
                break;
            case "Target2List":
                list = Target2List;
                break;
            case "ConnectionList":
                list = ConnectionList;
                // 菜单打开时暂时停止更新列表
                ConnectionListUpdateTimer.Stop();
                new WaitTimeoutChecker(30000).ThreadSleep(100, () => ConnectionListUpdateTimer.Enabled);
                break;
            case "BlockList":
                list = BlockList;
                break;
            default:
                return;
            }

            var index = list.PointToClient(MousePosition).Y / list.ColumnHeadersHeight - 1 + list.FirstDisplayedScrollingRowIndex;
            if (list.Rows[index].Selected) return;
            list.ClearSelection();
            list.Rows[index].Selected = true;
        }

        /// <summary>
        ///     菜单项“关于”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e) {
            MessageBox.Show("GUI for LAN Spy\r\n" +
                            "Made by FrostbiteXSW\r\n" +
                            "Any idea or critic is welcomed", "作者信息");
        }

        /// <summary>
        ///     菜单项“退出”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e) {
            // 创建载入界面
            var task = new Thread(stop => { StopModels(); }) {Name = RegisteredThreadName.ExitStopAllModels.ToString()};
            MessagePipe.SendInMessage(new KeyValuePair<Message, Thread>(Message.TaskIn, task));
            var loading = new Loading("正在退出，请稍候", task);
            loading.ShowDialog();

            // 等待结果
            if (!new WaitTimeoutChecker(30000).ThreadSleep(500, () => {
                var msg = MessagePipe.GetNextOutMessage(task);
                switch (msg) {
                case Message.NoAvailableMessage:
                    return true;
                case Message.TaskOut:
                    return false;
                default:
                    throw new Exception($"无效的消息类型：{msg}");
                }
            }))
                Environment.Exit(-1);

            // 模块已停止
            Environment.Exit(0);
        }
    }
}