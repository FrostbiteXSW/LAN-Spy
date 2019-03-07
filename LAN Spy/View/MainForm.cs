using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LAN_Spy.Controller;
using LAN_Spy.Controller.Classes;
using LAN_Spy.Model;
using LAN_Spy.Model.Classes;
using PacketDotNet;
using SharpPcap;
using SharpPcap.WinPcap;
using Message = LAN_Spy.Controller.Message;

namespace LAN_Spy.View {
    public partial class MainForm : Form {
        /// <summary>
        ///     阻止的连接列表
        /// </summary>
        private readonly HashTable _blockTable = new HashTable();

        /// <inheritdoc />
        /// <summary>
        ///     初始化 <see cref="MainForm" /> 窗口。
        /// </summary>
        /// <param name="models">传入的创建完成的模块实例组，此实例句柄在绑定完成后将被清除。</param>
        public MainForm(ref BasicClass[] models) {
            InitializeComponent();

            // TODO:新增模块时请更新此处的代码
            foreach (var model in models) {
                var type = model.GetType();
                if (type == typeof(Scanner))
                    Scanner = model as Scanner;
                else if (type == typeof(Poisoner))
                    Poisoner = model as Poisoner;
                else if (type == typeof(Watcher))
                    Watcher = model as Watcher;
            }

            models = null;
            TopMost = true;
        }

        /// <summary>
        ///     <see cref="Model.Scanner" /> 模块实例。
        /// </summary>
        private Scanner Scanner { get; }

        /// <summary>
        ///     <see cref="Model.Poisoner" /> 模块实例。
        /// </summary>
        private Poisoner Poisoner { get; }

        /// <summary>
        ///     <see cref="Model.Watcher" /> 模块实例。
        /// </summary>
        private Watcher Watcher { get; }

        /// <summary>
        ///     获取所有模块的实例组合。
        /// </summary>
        // TODO:新增模块时请更新此处的代码
        private IEnumerable<BasicClass> Models => new BasicClass[] {Scanner, Poisoner, Watcher};

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

                // TODO:新增模块时请更新此处的代码
                启动所有模块ToolStripMenuItem.Text = "启动所有模块";
                启动扫描模块ToolStripMenuItem.Text = 启动毒化模块ToolStripMenuItem.Text = 启动监视模块ToolStripMenuItem.Text = "启动模块";
                扫描主机ToolStripMenuItem.Enabled = false;
                侦测主机ToolStripMenuItem.Enabled = false;
                开始毒化ToolStripMenuItem.Enabled = false;
                开始监视ToolStripMenuItem.Enabled = false;
                添加到目标组1ToolStripMenuItem.Enabled = false;
                添加到目标组2ToolStripMenuItem.Enabled = false;
                阻止此连接ToolStripMenuItem.Enabled = false;
                HostList.Rows.Clear();
                Target1List.Rows.Clear();
                Target2List.Rows.Clear();
                BlockList.Rows.Clear();
                ConnectionListUpdateTimer.Stop();
                new WaitTimeoutChecker(30000).ThreadSleep(100, () => ConnectionListUpdateTimer.Enabled);
                ConnectionList.Rows.Clear();
            }

            // 启动模块
            if (StartupModels(Models)) {
                if (Models.Any(item => item.CurDevName == "")) return;
                MessageBox.Show("所有模块均已成功设置。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                启动所有模块ToolStripMenuItem.Text = "重启所有模块";

                // TODO:新增模块时请更新此处的代码
                启动扫描模块ToolStripMenuItem.Text = 启动毒化模块ToolStripMenuItem.Text = 启动监视模块ToolStripMenuItem.Text = "停止模块";
                扫描主机ToolStripMenuItem.Enabled = true;
                侦测主机ToolStripMenuItem.Enabled = true;
                开始毒化ToolStripMenuItem.Enabled = true;
                开始监视ToolStripMenuItem.Enabled = true;
                添加到目标组1ToolStripMenuItem.Enabled = true;
                添加到目标组2ToolStripMenuItem.Enabled = true;
                阻止此连接ToolStripMenuItem.Enabled = true;
            }
            else {
                MessageBox.Show("一个或多个模块未能成功初始化，请单独启动模块。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            // TODO:新增模块时请更新此处的代码
            启动所有模块ToolStripMenuItem.Text = "启动所有模块";
            启动扫描模块ToolStripMenuItem.Text = 启动毒化模块ToolStripMenuItem.Text = 启动监视模块ToolStripMenuItem.Text = "启动模块";
            扫描主机ToolStripMenuItem.Enabled = false;
            侦测主机ToolStripMenuItem.Enabled = false;
            开始毒化ToolStripMenuItem.Enabled = false;
            开始监视ToolStripMenuItem.Enabled = false;
            添加到目标组1ToolStripMenuItem.Enabled = false;
            添加到目标组2ToolStripMenuItem.Enabled = false;
            阻止此连接ToolStripMenuItem.Enabled = false;
            HostList.Rows.Clear();
            Target1List.Rows.Clear();
            Target2List.Rows.Clear();
            BlockList.Rows.Clear();
            ConnectionListUpdateTimer.Stop();
            new WaitTimeoutChecker(30000).ThreadSleep(100, () => ConnectionListUpdateTimer.Enabled);
            ConnectionList.Rows.Clear();
        }

        /// <summary>
        ///     停止所有模块的工作。
        /// </summary>
        /// <returns>成功返回true，错误返回false。</returns>
        private bool StopModels() {
            var result = true;

            // TODO:新增模块时请更新此处的代码
            if (Scanner is null) {
                result = false;
            }
            else {
                Scanner.Reset();
                Scanner.CurDevName = "";
            }

            if (Poisoner is null) {
                result = false;
            }
            else {
                Poisoner.StopPoisoning();
                Poisoner.Reset();
                Poisoner.CurDevName = "";
            }

            if (Watcher is null) {
                result = false;
            }
            else {
                Watcher.StopWatching();
                Watcher.Reset();
                Watcher.CurDevName = "";
            }

            return result;
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
        ///     菜单项“启动模块”（扫描）单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 启动扫描模块ToolStripMenuItem_Click(object sender, EventArgs e) {
            // 判断工作模式
            if (启动扫描模块ToolStripMenuItem.Text == "启动模块") {
                if (!StartupModels(new[] {Scanner})) {
                    MessageBox.Show("模块未能成功初始化，请检查。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Scanner.CurDevName != "") {
                    MessageBox.Show("模块已成功设置。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    启动所有模块ToolStripMenuItem.Text = "重启所有模块";
                    启动扫描模块ToolStripMenuItem.Text = "停止模块";
                    扫描主机ToolStripMenuItem.Enabled = true;
                    侦测主机ToolStripMenuItem.Enabled = true;
                }
            }
            else {
                // 创建载入界面
                var task = new Thread(stop => {
                    Scanner.Reset();
                    Scanner.CurDevName = "";
                }) {Name = RegisteredThreadName.StopScanner.ToString()};
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
                MessageBox.Show("模块已停止。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                启动扫描模块ToolStripMenuItem.Text = "启动模块";
                扫描主机ToolStripMenuItem.Enabled = false;
                侦测主机ToolStripMenuItem.Enabled = false;
                HostList.Rows.Clear();

                // TODO:新增模块时请更新此处的代码
                if (启动毒化模块ToolStripMenuItem.Text == "启动模块"
                    && 启动监视模块ToolStripMenuItem.Text == "启动模块")
                    启动所有模块ToolStripMenuItem.Text = "启动所有模块";
            }
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
        ///     菜单项“扫描主机”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 扫描主机ToolStripMenuItem_Click(object sender, EventArgs e) {
            // 清空历史纪录
            HostList.Rows.Clear();

            // 创建载入界面
            var task = new Thread(scan => {
                try {
                    Scanner.ScanForTarget();
                }
                catch (ThreadAbortException) {
                    Scanner.Reset();
                }
            }) {Name = RegisteredThreadName.ScanForTarget.ToString()};
            MessagePipe.SendInMessage(new KeyValuePair<Message, Thread>(Message.TaskIn, task));
            var loading = new Loading("正在扫描，请稍候", task);
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
                        return;
                    case Message.TaskOut:
                        break;
                    case Message.TaskNotFound:
                        MessageBox.Show("未能找到指定名称的工作线程。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    default:
                        throw new Exception($"无效的消息类型：{result}");
                }
            }
            MessagePipe.ClearAllMessage(task);

            // 输出扫描结果
            foreach (var host in Scanner.HostList) {
                // 格式化MAC地址
                var temp = new StringBuilder(host.PhysicalAddress.ToString());
                if (temp.Length == 12)
                    temp = temp.Insert(2, '-').Insert(5, '-').Insert(8, '-').Insert(11, '-').Insert(14, '-');

                if (Scanner.GatewayAddresses.Contains(host.IPAddress)) {
                    HostList.Rows.Add(host.IPAddress.ToString(), temp.ToString(), "网关地址");
                    Poisoner.Gateway = host;
                }
                else if (Equals(host.IPAddress, Scanner.Ipv4Address)) {
                    HostList.Rows.Add(host.IPAddress.ToString(), temp.ToString(), "当前设备地址");
                }
                else {
                    HostList.Rows.Add(host.IPAddress.ToString(), temp.ToString(), "");
                }

                HostList.Rows[HostList.Rows.Count - 1].ContextMenuStrip = HostListMenuStrip;
            }
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
        ///     菜单项“侦测主机”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 侦测主机ToolStripMenuItem_Click(object sender, EventArgs e) {
            // 清空历史纪录
            HostList.Rows.Clear();

            // 创建载入界面
            var task = new Thread(spy => {
                try {
                    Scanner.SpyForTarget();
                }
                catch (ThreadAbortException) { }
            }) {Name = RegisteredThreadName.SpyForTarget.ToString()};
            MessagePipe.SendInMessage(new KeyValuePair<Message, Thread>(Message.TaskIn, task));
            var loading = new Loading("正在侦测，取消以停止", task);
            loading.ShowDialog();

            // 等待结果
            new WaitTimeoutChecker(30000).ThreadSleep(500, () => MessagePipe.GetNextOutMessage(task) != Message.UserCancel);

            // 用户取消
            MessagePipe.SendInMessage(new KeyValuePair<Message, Thread>(Message.TaskCancel, task));
            var result = Message.NoAvailableMessage;
            new WaitTimeoutChecker(30000).ThreadSleep(500, () => (result = MessagePipe.GetNextOutMessage(task)) == Message.NoAvailableMessage);
            MessagePipe.ClearAllMessage(task);

            // 检查是否正确结束
            if (result != Message.TaskAborted) {
                MessageBox.Show("线程异常结束。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 输出侦测结果
            foreach (var host in Scanner.HostList) {
                // 格式化MAC地址
                var temp = new StringBuilder(host.PhysicalAddress.ToString());
                if (temp.Length == 12)
                    temp = temp.Insert(2, '-').Insert(5, '-').Insert(8, '-').Insert(11, '-').Insert(14, '-');

                if (Scanner.GatewayAddresses.Contains(host.IPAddress)) {
                    HostList.Rows.Add(host.IPAddress.ToString(), temp.ToString(), "网关地址");
                    Poisoner.Gateway = host;
                }
                else if (Equals(host.IPAddress, Scanner.Ipv4Address)) {
                    HostList.Rows.Add(host.IPAddress.ToString(), temp.ToString(), "当前设备地址");
                }
                else {
                    HostList.Rows.Add(host.IPAddress.ToString(), temp.ToString(), "");
                }

                HostList.Rows[HostList.Rows.Count - 1].ContextMenuStrip = HostListMenuStrip;
            }
        }

        /// <summary>
        ///     菜单项“启动模块”（毒化）单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 启动毒化模块ToolStripMenuItem_Click(object sender, EventArgs e) {
            // 判断工作模式
            if (启动毒化模块ToolStripMenuItem.Text == "启动模块") {
                if (!StartupModels(new[] {Poisoner})) {
                    MessageBox.Show("模块未能成功初始化，请检查。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Poisoner.CurDevName != "") {
                    // 查找网关
                    if (Poisoner.Gateway is null)
                        foreach (var host in Scanner.HostList)
                            if (Scanner.GatewayAddresses.Contains(host.IPAddress))
                                Poisoner.Gateway = host;

                    MessageBox.Show("模块已成功设置。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    启动所有模块ToolStripMenuItem.Text = "重启所有模块";
                    启动毒化模块ToolStripMenuItem.Text = "停止模块";
                    开始毒化ToolStripMenuItem.Enabled = true;
                    添加到目标组1ToolStripMenuItem.Enabled = true;
                    添加到目标组2ToolStripMenuItem.Enabled = true;
                    阻止此连接ToolStripMenuItem.Enabled = true;
                }
            }
            else {
                // 创建载入界面
                var task = new Thread(stop => {
                    Poisoner.StopPoisoning();
                    Poisoner.Reset();
                    Poisoner.CurDevName = "";
                }) {Name = RegisteredThreadName.StopPoisoner.ToString()};
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
                MessageBox.Show("模块已停止。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                启动毒化模块ToolStripMenuItem.Text = "启动模块";
                开始毒化ToolStripMenuItem.Enabled = false;
                添加到目标组1ToolStripMenuItem.Enabled = false;
                添加到目标组2ToolStripMenuItem.Enabled = false;
                阻止此连接ToolStripMenuItem.Enabled = false;
                Target1List.Rows.Clear();
                Target2List.Rows.Clear();
                BlockList.Rows.Clear();

                // TODO:新增模块时请更新此处的代码
                if (启动扫描模块ToolStripMenuItem.Text == "启动模块"
                    && 启动监视模块ToolStripMenuItem.Text == "启动模块")
                    启动所有模块ToolStripMenuItem.Text = "启动所有模块";
            }
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
        ///     菜单项“启动模块”（监视）单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 启动监视模块ToolStripMenuItem_Click(object sender, EventArgs e) {
            // 判断工作模式
            if (启动监视模块ToolStripMenuItem.Text == "启动模块") {
                if (!StartupModels(new[] {Watcher})) {
                    MessageBox.Show("模块未能成功初始化，请检查。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Watcher.CurDevName != "") {
                    MessageBox.Show("模块已成功设置。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    启动所有模块ToolStripMenuItem.Text = "重启所有模块";
                    启动监视模块ToolStripMenuItem.Text = "停止模块";
                    开始监视ToolStripMenuItem.Enabled = true;
                }
            }
            else {
                // 创建载入界面
                var task = new Thread(stop => {
                    Watcher.StopWatching();
                    Watcher.Reset();
                    Watcher.CurDevName = "";
                }) {Name = RegisteredThreadName.StopWatcher.ToString()};
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
                MessageBox.Show("模块已停止。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                启动监视模块ToolStripMenuItem.Text = "启动模块";
                开始监视ToolStripMenuItem.Enabled = false;
                ConnectionListUpdateTimer.Stop();
                new WaitTimeoutChecker(30000).ThreadSleep(100, () => ConnectionListUpdateTimer.Enabled);
                ConnectionList.Rows.Clear();

                // TODO:新增模块时请更新此处的代码
                if (启动扫描模块ToolStripMenuItem.Text == "启动模块"
                    && 启动毒化模块ToolStripMenuItem.Text == "启动模块")
                    启动所有模块ToolStripMenuItem.Text = "启动所有模块";
            }
        }

        /// <summary>
        ///     开始毒化菜单项启用状态发生改变时触发事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 开始毒化ToolStripMenuItem_EnabledChanged(object sender, EventArgs e) {
            if (!开始毒化ToolStripMenuItem.Enabled)
                开始毒化ToolStripMenuItem.Text = "开始毒化";
        }

        /// <summary>
        ///     开始监视菜单项启用状态发生改变时触发事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 开始监视ToolStripMenuItem_EnabledChanged(object sender, EventArgs e) {
            if (!开始监视ToolStripMenuItem.Enabled)
                开始监视ToolStripMenuItem.Text = "开始监视";
        }

        /// <summary>
        ///     菜单项“添加到目标组”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 添加到目标组ToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (DataGridViewRow row in HostList.Rows) {
                if (!row.Selected) continue;
                if (((ToolStripMenuItem) sender).Text[((ToolStripMenuItem) sender).Text.Length - 1].Equals('1')) {
                    if (Target1List.Rows.Cast<DataGridViewRow>()
                        .Any(item => item.Cells[0].Value.ToString().Equals(row.Cells["HostIP"].Value.ToString())
                                     && item.Cells[1].Value.ToString().Equals(row.Cells["HostMAC"].Value.ToString())))
                        continue;
                    Target1List.Rows.Add(row.Cells["HostIP"].Value.ToString(), row.Cells["HostMAC"].Value.ToString());
                    Target1List.Rows[Target1List.Rows.Count - 1].ContextMenuStrip = TargetListMenuStrip;
                }
                else {
                    if (Target2List.Rows.Cast<DataGridViewRow>()
                        .Any(item => item.Cells[0].Value.ToString().Equals(row.Cells["HostIP"].Value.ToString())
                                     && item.Cells[1].Value.ToString().Equals(row.Cells["HostMAC"].Value.ToString())))
                        continue;
                    Target2List.Rows.Add(row.Cells["HostIP"].Value.ToString(), row.Cells["HostMAC"].Value.ToString());
                    Target2List.Rows[Target2List.Rows.Count - 1].ContextMenuStrip = TargetListMenuStrip;
                }
            }
        }

        /// <summary>
        ///     菜单项“从目标组移除”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 从目标组移除ToolStripMenuItem_Click(object sender, EventArgs e) {
            var list = Target1List.SelectedRows.Count != 0 ? Target1List : Target2List;
            foreach (DataGridViewRow row in list.SelectedRows)
                list.Rows.Remove(row);
        }

        /// <summary>
        ///     毒化目标列表选择情况变化时触发的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void TargetList_SelectionChanged(object sender, EventArgs e) {
            DataGridView curList, oldList;
            if (((DataGridView) sender).Name == "Target1List") {
                curList = Target1List;
                oldList = Target2List;
            }
            else {
                curList = Target2List;
                oldList = Target1List;
            }

            if (oldList.SelectedRows.Count == 0) return;

            var selection = curList.SelectedRows;
            oldList.ClearSelection();
            foreach (DataGridViewRow row in selection)
                row.Selected = true;
        }

        /// <summary>
        ///     菜单项“开始毒化”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 开始毒化ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (开始毒化ToolStripMenuItem.Text == "开始毒化") {
                if (MessageBox.Show("注意：在重新启动毒化工作前，目标无法被更改，仍然启动？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    return;

                if (Poisoner.Gateway is null && MessageBox.Show("未能找到默认网关，被毒化设备将不能接入外部网络，仍然继续？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    return;

                // 创建载入界面
                var task = new Thread(poisoning => {
                    try {
                        Poisoner.Target1.Clear();
                        Poisoner.Target2.Clear();

                        foreach (DataGridViewRow target in Target1List.Rows)
                            Poisoner.Target1.Add(new Host(IPAddress.Parse(target.Cells["Target1IP"].Value.ToString()),
                                PhysicalAddress.Parse(target.Cells["Target1MAC"].Value.ToString())));
                        foreach (DataGridViewRow target in Target2List.Rows)
                            Poisoner.Target2.Add(new Host(IPAddress.Parse(target.Cells["Target2IP"].Value.ToString()),
                                PhysicalAddress.Parse(target.Cells["Target2MAC"].Value.ToString())));

                        Poisoner.StartPoisoning();
                    }
                    catch (ThreadAbortException) {
                        // 用户中止了操作
                        Poisoner.StopPoisoning();
                    }
                }) {Name = RegisteredThreadName.StartPoisoning.ToString()};
                MessagePipe.SendInMessage(new KeyValuePair<Message, Thread>(Message.TaskIn, task));
                var loading = new Loading("正在毒化，请稍候", task);
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
                            return;
                        case Message.TaskOut:
                            break;
                        case Message.TaskNotFound:
                            MessageBox.Show("未能找到指定名称的工作线程。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        default:
                            throw new Exception($"无效的消息类型：{result}");
                    }
                }

                MessagePipe.ClearAllMessage(task);
                MessageBox.Show("毒化工作已启动。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                开始毒化ToolStripMenuItem.Text = "停止毒化";
            }
            else {
                // 创建载入界面
                var task = new Thread(stop => { Poisoner.StopPoisoning(); }) {Name = RegisteredThreadName.StopPoisoning.ToString()};
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
                MessageBox.Show("毒化工作已停止。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                开始毒化ToolStripMenuItem.Text = "开始毒化";
            }
        }

        /// <summary>
        ///     菜单项“开始毒化”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 开始监视ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (开始监视ToolStripMenuItem.Text == "开始监视") {
                // 创建载入界面
                var task = new Thread(watching => {
                    try {
                        Watcher.StartWatching();
                    }
                    catch (ThreadAbortException) {
                        // 用户中止了操作
                        Watcher.StopWatching();
                    }
                }) {Name = RegisteredThreadName.StartWatching.ToString()};
                MessagePipe.SendInMessage(new KeyValuePair<Message, Thread>(Message.TaskIn, task));
                var loading = new Loading("正在启动监视，请稍候", task);
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
                            return;
                        case Message.TaskOut:
                            break;
                        case Message.TaskNotFound:
                            MessageBox.Show("未能找到指定名称的工作线程。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        default:
                            throw new Exception($"无效的消息类型：{result}");
                    }
                }

                MessagePipe.ClearAllMessage(task);
                MessageBox.Show("监视工作已启动。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ConnectionListUpdateTimer.Start();
                开始监视ToolStripMenuItem.Text = "停止监视";
            }
            else {
                // 创建载入界面
                var task = new Thread(stop => { Watcher.StopWatching(); }) {Name = RegisteredThreadName.StopWatching.ToString()};
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
                ConnectionListUpdateTimer.Stop();
                new WaitTimeoutChecker(30000).ThreadSleep(100, () => ConnectionListUpdateTimer.Enabled);
                ConnectionList.Rows.Clear();
                MessageBox.Show("监视工作已停止。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                开始监视ToolStripMenuItem.Text = "开始监视";
            }
        }

        /// <summary>
        ///     连接列表更新周期到期时触发的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void ConnectionListUpdateTimer_Tick(object sender, EventArgs e) {
            // 获取当前连接列表
            var curConnection = ConnectionList.Rows.Cast<DataGridViewRow>().ToList();

            foreach (var link in Watcher.TcpLinks) {
                // 检查是否过滤本机流量
                if (过滤本机流量ToolStripMenuItem.Checked
                    && (link.SrcAddress.Equals(Watcher.Ipv4Address)
                        || link.DstAddress.Equals(Watcher.Ipv4Address)))
                    continue;

                // 创建新连接并检查是否存在
                var row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewTextBoxCell {Value = $"{link.SrcAddress}:{link.SrcPort}"});
                row.Cells.Add(new DataGridViewTextBoxCell {Value = $"{link.DstAddress}:{link.DstPort}"});
                row.ContextMenuStrip = ConnectionListMenuStrip;
                if (curConnection.Any(item => item.Cells["SrcAddress"].Value.Equals(row.Cells[0].Value)
                                              && item.Cells["DstAddress"].Value.Equals(row.Cells[1].Value)))
                    // 旧连接仍存活，从待移除列表删除
                    curConnection.RemoveAll(item => item.Cells["SrcAddress"].Value.Equals(row.Cells[0].Value)
                                                    && item.Cells["DstAddress"].Value.Equals(row.Cells[1].Value));
                else
                    // 添加新连接
                    ConnectionList.Rows.Add(row);
            }

            // 移除不存活的连接
            foreach (var item in curConnection)
                ConnectionList.Rows.Remove(ConnectionList.Rows.Cast<DataGridViewRow>().First(target => target.Cells["SrcAddress"].Value.Equals(item.Cells[0].Value)
                                                                                                       && target.Cells["DstAddress"].Value.Equals(item.Cells[1].Value)));
        }

        /// <summary>
        ///     连接列表右键菜单关闭时触发的事件，重新开始更新列表。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void ConnectionListMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e) {
            ConnectionListUpdateTimer.Start();
        }

        /// <summary>
        ///     菜单项“断开此连接”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 断开此连接ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!ConnectionList.SelectedRows.Cast<DataGridViewRow>()
                .Aggregate(true, (current, row) => !(!current || !Watcher.KillConnection(IPAddress.Parse(row.Cells["SrcAddress"].Value.ToString().Substring(0, row.Cells["SrcAddress"].Value.ToString().LastIndexOf(':'))),
                                                         ushort.Parse(row.Cells["SrcAddress"].Value.ToString().Substring(row.Cells["SrcAddress"].Value.ToString().LastIndexOf(':') + 1)),
                                                         IPAddress.Parse(row.Cells["DstAddress"].Value.ToString().Substring(0, row.Cells["DstAddress"].Value.ToString().LastIndexOf(':'))),
                                                         ushort.Parse(row.Cells["DstAddress"].Value.ToString().Substring(row.Cells["DstAddress"].Value.ToString().LastIndexOf(':') + 1))))))
                MessageBox.Show("一个或多个数据包发送失败。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        ///     连接列表键盘按键事件响应方法，暂停列表更新。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void ConnectionList_KeyEvent(object sender, KeyEventArgs e) {
            if (ConnectionListUpdateTimer.Enabled) {
                ConnectionListUpdateTimer.Stop();
                new WaitTimeoutChecker(30000).ThreadSleep(100, () => ConnectionListUpdateTimer.Enabled);
            }
            else if (!ConnectionListMenuStrip.Visible) {
                ConnectionListUpdateTimer.Start();
            }
        }

        /// <summary>
        ///     连接列表鼠标按键事件响应方法，暂停列表更新。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void ConnectionList_MouseEvent(object sender, MouseEventArgs e) {
            if (ConnectionListUpdateTimer.Enabled) {
                ConnectionListUpdateTimer.Stop();
                new WaitTimeoutChecker(30000).ThreadSleep(100, () => ConnectionListUpdateTimer.Enabled);
            }
            else if (!ConnectionListMenuStrip.Visible) {
                ConnectionListUpdateTimer.Start();
            }
        }

        /// <summary>
        ///     菜单项“过滤本机流量”勾选状态改变时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 过滤本机流量ToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
            // 检查监视器是否在工作
            if (开始监视ToolStripMenuItem.Text.Equals("开始监视"))
                return;

            // 暂停列表更新
            ConnectionListUpdateTimer.Stop();
            new WaitTimeoutChecker(30000).ThreadSleep(100, () => ConnectionListUpdateTimer.Enabled);

            // 去除已有本机流量数据
            var removeRows = ConnectionList.Rows.Cast<DataGridViewRow>().Where(row =>
                IPAddress.Parse(row.Cells["SrcAddress"].Value.ToString().Substring(0, row.Cells["SrcAddress"].Value.ToString().LastIndexOf(':'))).Equals(Watcher.Ipv4Address)
                || IPAddress.Parse(row.Cells["DstAddress"].Value.ToString().Substring(0, row.Cells["DstAddress"].Value.ToString().LastIndexOf(':'))).Equals(Watcher.Ipv4Address)).ToList();
            foreach (var row in removeRows)
                ConnectionList.Rows.Remove(row);

            // 恢复列表更新
            ConnectionListUpdateTimer.Start();
        }

        /// <summary>
        ///     毒化器处理IPv4数据包到达事件的方法。
        /// </summary>
        /// <param name="packet">到达的 <see cref="IPv4Packet" /> 类型数据包。</param>
        /// <param name="isHandled">指示数据包是否已被处理并且不需要被转发。</param>
        private void Poisoner_OnIPv4PacketReceive(Packet packet, out bool isHandled) {
            isHandled = false;

            // 分析IPv4数据包
            var ipv4 = (IPv4Packet) packet;

            // 判断源IP和目标IP的通讯是否存在于禁止列表中
            if (!(_blockTable[(ipv4.SourceAddress.ToString() + ipv4.DestinationAddress).GetHashCode()] is null)
                || !(_blockTable[(ipv4.DestinationAddress.ToString() + ipv4.SourceAddress).GetHashCode()] is null))
                isHandled = true;
        }

        /// <summary>
        ///     菜单项“阻止此连接”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 阻止此连接ToolStripMenuItem_Click(object sender, EventArgs e) {
            var needInit = _blockTable.Length == 0;

            foreach (DataGridViewRow row in ConnectionList.SelectedRows) {
                string srcIP = row.Cells["SrcAddress"].Value.ToString().Substring(0, row.Cells["SrcAddress"].Value.ToString().IndexOf(':')),
                    dstIP = row.Cells["DstAddress"].Value.ToString().Substring(0, row.Cells["DstAddress"].Value.ToString().IndexOf(':'));

                if (!(_blockTable[(srcIP + dstIP).GetHashCode()] is null))
                    continue;

                var target = new DataGridViewRow();

                target.Cells.Add(new DataGridViewTextBoxCell {Value = srcIP});
                target.Cells.Add(new DataGridViewTextBoxCell {Value = dstIP});
                target.ContextMenuStrip = BlockListMenuStrip;

                BlockList.Rows.Add(target);
                _blockTable.Add((srcIP + dstIP).GetHashCode(), target);
            }

            if (needInit)
                Poisoner.OnIPv4PacketReceive += Poisoner_OnIPv4PacketReceive;
        }

        /// <summary>
        ///     菜单项“取消阻止”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 取消阻止ToolStripMenuItem_Click(object sender, EventArgs e) {
            var targets = new List<DataGridViewRow>();
            var hashes = new List<int>();

            foreach (DataGridViewRow row in BlockList.SelectedRows) {
                var hash = (row.Cells["SrcIP"].Value.ToString() + row.Cells["DstIP"].Value).GetHashCode();
                hashes.Add(hash);
                targets.Add((DataGridViewRow) _blockTable[hash]);
            }

            _blockTable.RemoveRange(hashes);
            foreach (var target in targets)
                BlockList.Rows.Remove(target);

            if (_blockTable.Length == 0)
                Poisoner.OnIPv4PacketReceive -= Poisoner_OnIPv4PacketReceive;
        }
    }
}