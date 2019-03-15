using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LAN_Spy.Controller;
using LAN_Spy.Controller.Classes;
using LAN_Spy.Model;
using LAN_Spy.Model.Classes;
using Message = LAN_Spy.Controller.Message;

namespace LAN_Spy.View.MainForm {
    public partial class MainForm {
        /// <summary>
        ///     <see cref="Model.Scanner" /> 模块实例。
        /// </summary>
        private Scanner Scanner { get; set; }

        /// <summary>
        ///     启用与 <see cref="Model.Scanner" /> 相关的控件。
        /// </summary>
        private void EnableScannerItems() {
            启动扫描模块ToolStripMenuItem.Text = "停止模块";
            扫描主机ToolStripMenuItem.Enabled = true;
            侦测主机ToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        ///     禁用与 <see cref="Model.Scanner" /> 相关的控件。
        /// </summary>
        private void DisableScannerItems() {
            启动扫描模块ToolStripMenuItem.Text = "启动模块";
            扫描主机ToolStripMenuItem.Enabled = false;
            侦测主机ToolStripMenuItem.Enabled = false;
            HostList.Rows.Clear();
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
    }
}