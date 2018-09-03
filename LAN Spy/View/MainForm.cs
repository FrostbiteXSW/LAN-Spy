using LAN_Spy.Controller;
using LAN_Spy.Model;
using LAN_Spy.Model.Classes;
using SharpPcap;
using SharpPcap.WinPcap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Message = LAN_Spy.Controller.Message;

namespace LAN_Spy.View {
    public partial class MainForm : Form {
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
            if (启动所有模块ToolStripMenuItem.Text == "重启所有模块" && !StopModels()) {
                MessageBox.Show("一个或多个模块未能成功初始化，请检查。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 启动模块
            if (StartupModels(Models)) {
                if (Models.Count(item => item.CurDevName == "") != 0) return;
                MessageBox.Show("所有模块均已成功设置。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                启动所有模块ToolStripMenuItem.Text = "重启所有模块";

                // TODO:新增模块时请更新此处的代码
                启动扫描模块ToolStripMenuItem.Text = 启动毒化模块ToolStripMenuItem.Text = 启动监视模块ToolStripMenuItem.Text = "停止模块";
                扫描主机ToolStripMenuItem.Enabled = true;
                侦测主机ToolStripMenuItem.Enabled = true;
                开始毒化ToolStripMenuItem.Enabled = true;
                开始监视ToolStripMenuItem.Enabled = true;
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
            StopModels();
            Close();
        }

        /// <summary>
        ///     菜单项“停止所有模块”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 停止所有模块ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!StopModels())
                MessageBox.Show("一个或多个模块未能成功初始化，请检查。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show("所有模块已停止。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            启动所有模块ToolStripMenuItem.Text = "启动所有模块";
            
            // TODO:新增模块时请更新此处的代码
            启动扫描模块ToolStripMenuItem.Text = 启动毒化模块ToolStripMenuItem.Text = 启动监视模块ToolStripMenuItem.Text = "启动模块";
            扫描主机ToolStripMenuItem.Enabled = false;
            侦测主机ToolStripMenuItem.Enabled = false;
            开始毒化ToolStripMenuItem.Enabled = false;
            开始监视ToolStripMenuItem.Enabled = false;
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
                if (!StartupModels(new[] {Scanner}))
                    MessageBox.Show("模块未能成功初始化，请检查。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Scanner.CurDevName != "") {
                    MessageBox.Show("模块已成功设置。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    启动所有模块ToolStripMenuItem.Text = "重启所有模块";
                    启动扫描模块ToolStripMenuItem.Text = "停止模块";
                    扫描主机ToolStripMenuItem.Enabled = true;
                    侦测主机ToolStripMenuItem.Enabled = true;
                }
            }
            else {
                Scanner.Reset();
                Scanner.CurDevName = "";
                MessageBox.Show("模块已停止。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                启动扫描模块ToolStripMenuItem.Text = "启动模块";
                扫描主机ToolStripMenuItem.Enabled = false;
                侦测主机ToolStripMenuItem.Enabled = false;

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
                var result = MessageBox.Show("无法确定指定设备的IPv4网络环境，仍然继续？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK) break;
            }

            // 创建载入界面
            var loading = new Loading("设置模块中，请稍候");
            var task = new Thread(scan => {
                Thread.Sleep(1000);
                try {
                    lock (modelsList) {
                        foreach (var model in modelsList)
                            model.CurDevName = index.Item.ToString();
                    }
                }
                catch (ThreadAbortException) {
                    lock (modelsList) {
                        foreach (var model in modelsList)
                            model.CurDevName = "";
                    }
                }
                finally {
                    loading.Close();
                }
            }) {Name = RegistedThreadName.StartupModels.ToString()};
            MessagePipe.SendInMessage(new KeyValuePair<Message, object>(Message.TaskIn, task));
            loading.ShowDialog();

            // 等待结果
            while ((MessagePipe.TopOutMessage.Key != Message.TaskOut 
                    && MessagePipe.TopOutMessage.Key != Message.UserCancel)
                   || (MessagePipe.TopOutMessage.Key == Message.TaskOut 
                       && ((Thread) MessagePipe.TopOutMessage.Value).Name != task.Name)) 
                Thread.Sleep(500);

            // 任务完成
            if (MessagePipe.TopOutMessage.Key != Message.UserCancel) {
                MessagePipe.GetNextOutMessage();
                return true;
            }

            // 用户取消
            MessagePipe.GetNextOutMessage();
            MessagePipe.SendInMessage(new KeyValuePair<Message, object>(Message.TaskCancel, task));
            while (MessagePipe.TopOutMessage.Key != Message.TaskAborted
                   || ((Thread) MessagePipe.TopOutMessage.Value).Name != task.Name) {
                if (MessagePipe.TopOutMessage.Key == Message.TaskNotFound
                    && ((Thread) MessagePipe.TopOutMessage.Value).Name == task.Name) {
                    MessagePipe.GetNextOutMessage();
                    MessageBox.Show("未能找到指定名称的工作线程。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                Thread.Sleep(500);
            }
            MessagePipe.GetNextOutMessage();
            return false;
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
            var loading = new Loading("正在扫描，请稍候");
            var task = new Thread(scan => {
                try {
                    Thread.Sleep(1000);
                    lock (Scanner) {
                        Scanner.ScanForTarget();
                    }
                }
                catch (ThreadAbortException) {
                    lock (Scanner) {
                        Scanner.Reset();
                    }
                }
                finally {
                    loading.Close();
                }
            }) {Name = RegistedThreadName.ScanForTarget.ToString()};
            MessagePipe.SendInMessage(new KeyValuePair<Message, object>(Message.TaskIn, task));
            loading.ShowDialog();

            // 等待结果
            while ((MessagePipe.TopOutMessage.Key != Message.TaskOut 
                    && MessagePipe.TopOutMessage.Key != Message.UserCancel)
                   || (MessagePipe.TopOutMessage.Key == Message.TaskOut 
                       && ((Thread) MessagePipe.TopOutMessage.Value).Name != task.Name)) 
                Thread.Sleep(500);

            // 用户取消
            if (MessagePipe.TopOutMessage.Key == Message.UserCancel) {
                MessagePipe.GetNextOutMessage();
                MessagePipe.SendInMessage(new KeyValuePair<Message, object>(Message.TaskCancel, task));
                while (MessagePipe.TopOutMessage.Key != Message.TaskAborted
                       || ((Thread) MessagePipe.TopOutMessage.Value).Name != task.Name) {
                    if (MessagePipe.TopOutMessage.Key == Message.TaskNotFound
                        && ((Thread) MessagePipe.TopOutMessage.Value).Name == task.Name) {
                        MessagePipe.GetNextOutMessage();
                        MessageBox.Show("未能找到指定名称的工作线程。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Thread.Sleep(500);
                }
                MessagePipe.GetNextOutMessage();
                return;
            }

            // 扫描结束
            MessagePipe.GetNextOutMessage();

            // 输出扫描结果
            lock (Scanner) {
                foreach (var host in Scanner.HostList) {
                    // 格式化MAC地址
                    var temp = new StringBuilder(host.PhysicalAddress.ToString());
                    if (temp.Length == 12)
                        temp = temp.Insert(2, '-').Insert(5, '-').Insert(8, '-').Insert(11, '-').Insert(14, '-');
                    HostList.Rows.Add(host.IPAddress, temp);
                    HostList.Rows[HostList.Rows.Count - 1].ContextMenuStrip = HostListMenuStrip;
                }
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
            var loading = new Loading("正在侦测，取消以停止");
            var task = new Thread(scan => {
                try {
                    Thread.Sleep(1000);
                    lock (Scanner) {
                        Scanner.SpyForTarget();
                    }
                }
                catch (ThreadAbortException) { }
                finally {
                    loading.Close();
                }
            }) {Name = RegistedThreadName.SpyForTarget.ToString()};
            MessagePipe.SendInMessage(new KeyValuePair<Message, object>(Message.TaskIn, task));
            loading.ShowDialog();

            // 等待结果
            var waitTime = 0;
            while (MessagePipe.TopOutMessage.Key != Message.UserCancel) {
                Thread.Sleep(500);
                if ((waitTime += 500) == 30000)
                    throw new TimeoutException("等待消息队列传递消息超时。");
            }
            MessagePipe.GetNextOutMessage();

            // 用户取消
            MessagePipe.SendInMessage(new KeyValuePair<Message, object>(Message.TaskCancel, task));
            while (MessagePipe.TopOutMessage.Key != Message.TaskAborted
                   || ((Thread) MessagePipe.TopOutMessage.Value).Name != task.Name) {
                if (MessagePipe.TopOutMessage.Key == Message.TaskNotFound
                    && ((Thread) MessagePipe.TopOutMessage.Value).Name == task.Name) {
                    MessagePipe.GetNextOutMessage();
                    MessageBox.Show("未能找到指定名称的工作线程。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Thread.Sleep(500);
            }
            MessagePipe.GetNextOutMessage();

            // 输出侦测结果
            lock (Scanner) {
                foreach (var host in Scanner.HostList) {
                    // 格式化MAC地址
                    var temp = new StringBuilder(host.PhysicalAddress.ToString());
                    if (temp.Length == 12)
                        temp = temp.Insert(2, '-').Insert(5, '-').Insert(8, '-').Insert(11, '-').Insert(14, '-');
                    HostList.Rows.Add(host.IPAddress, temp);
                    HostList.Rows[HostList.Rows.Count - 1].ContextMenuStrip = HostListMenuStrip;
                }
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
                if (!StartupModels(new[] {Poisoner}))
                    MessageBox.Show("模块未能成功初始化，请检查。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Poisoner.CurDevName != "") {
                    MessageBox.Show("模块已成功设置。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    启动所有模块ToolStripMenuItem.Text = "重启所有模块";
                    启动毒化模块ToolStripMenuItem.Text = "停止模块";
                    开始毒化ToolStripMenuItem.Enabled = true;
                }
            }
            else {
                Poisoner.StopPoisoning();
                Poisoner.Reset();
                Poisoner.CurDevName = "";
                MessageBox.Show("模块已停止。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                启动毒化模块ToolStripMenuItem.Text = "启动模块";
                开始毒化ToolStripMenuItem.Enabled = false;

                // TODO:新增模块时请更新此处的代码
                if (启动扫描模块ToolStripMenuItem.Text == "启动模块"
                    && 启动监视模块ToolStripMenuItem.Text == "启动模块")
                    启动所有模块ToolStripMenuItem.Text = "启动所有模块";
            }
        }
        
        /// <summary>
        ///     菜单项“复制”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void DataGridView复制ToolStripMenuItem_Click(object sender, EventArgs e) {
            DataGridView gridView;
            
            // TODO:新增模块时请更新此处的代码
            switch (MainTabContainer.SelectedIndex) {
                case 0:
                    // 调用者为主机列表
                    gridView = HostList;
                    break;
                case 2:
                    // 调用者为连接列表
                    gridView = ConnectionList;
                    break;
                default:
                    return;
            }

            var str = new StringBuilder();

            foreach (DataGridViewRow row in gridView.Rows) {
                if (!row.Selected) continue;
                foreach (DataGridViewCell cell in row.Cells)
                    str.Append($"{cell.Value} ");
                str.Replace(" ", "\r\n", str.Length - 1, 1);
            }

            Clipboard.SetDataObject(str.ToString().Substring(0, str.Length - 2), true);
        }

        /// <summary>
        ///     右键菜单打开后触发的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void HostListMenuStrip_Opened(object sender, EventArgs e) {
            var index = HostList.PointToClient(MousePosition).Y / HostList.Rows[0].Height - 1;
            if (HostList.Rows[index].Selected) return;
            HostList.ClearSelection();
            HostList.Rows[index].Selected = true;
        }
        
        /// <summary>
        ///     菜单项“启动模块”（监视）单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 启动监视模块ToolStripMenuItem_Click(object sender, EventArgs e) {
            // 判断工作模式
            if (启动监视模块ToolStripMenuItem.Text == "启动模块") {
                if (!StartupModels(new[] {Watcher}))
                    MessageBox.Show("模块未能成功初始化，请检查。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (Poisoner.CurDevName != "") {
                    MessageBox.Show("模块已成功设置。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    启动所有模块ToolStripMenuItem.Text = "重启所有模块";
                    启动监视模块ToolStripMenuItem.Text = "停止模块";
                    开始监视ToolStripMenuItem.Enabled = true;
                }
            }
            else {
                Watcher.StopWatching();
                Watcher.Reset();
                Watcher.CurDevName = "";
                MessageBox.Show("模块已停止。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                启动监视模块ToolStripMenuItem.Text = "启动模块";
                开始监视ToolStripMenuItem.Enabled = false;

                // TODO:新增模块时请更新此处的代码
                if (启动扫描模块ToolStripMenuItem.Text == "启动模块"
                    && 启动监视模块ToolStripMenuItem.Text == "启动模块")
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
        ///     菜单项“添加到目标组1”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 添加到目标组1ToolStripMenuItem_Click(object sender, EventArgs e) {

        }
    }
}