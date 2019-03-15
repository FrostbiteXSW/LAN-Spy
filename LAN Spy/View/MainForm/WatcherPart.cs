using LAN_Spy.Controller;
using LAN_Spy.Controller.Classes;
using LAN_Spy.Model;
using LAN_Spy.Model.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Message = LAN_Spy.Controller.Message;

namespace LAN_Spy.View.MainForm {
    public partial class MainPart {
        /// <summary>
        ///     阻止的连接列表
        /// </summary>
        private readonly Hashtable _blockTable = new Hashtable();

        /// <summary>
        ///     <see cref="Model.Watcher" /> 模块实例。
        /// </summary>
        private Watcher Watcher { get; set; }

        /// <summary>
        ///     启用与 <see cref="Model.Watcher" /> 相关的控件。
        /// </summary>
        private void EnableWatcherItems() {
            启动监视模块ToolStripMenuItem.Text = "停止模块";
            开始监视ToolStripMenuItem.Enabled = true;
            阻止此连接ToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        ///     禁用与 <see cref="Model.Watcher" /> 相关的控件。
        /// </summary>
        private void DisableWatcherItems() {
            启动监视模块ToolStripMenuItem.Text = "启动模块";
            开始监视ToolStripMenuItem.Enabled = false;
            阻止此连接ToolStripMenuItem.Enabled = false;
            BlockList.Rows.Clear();
            ConnectionListUpdateTimer.Stop();
            new WaitTimeoutChecker(30000).ThreadSleep(100, () => ConnectionListUpdateTimer.Enabled);
            ConnectionList.Rows.Clear();
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
                    Watcher.Stop();
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
        ///     开始监视菜单项启用状态发生改变时触发事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 开始监视ToolStripMenuItem_EnabledChanged(object sender, EventArgs e) {
            if (!开始监视ToolStripMenuItem.Enabled)
                开始监视ToolStripMenuItem.Text = "开始监视";
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
                        Watcher.Stop();
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
                var task = new Thread(stop => { Watcher.Stop(); }) {Name = RegisteredThreadName.StopWatching.ToString()};
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
                                                                                || IPAddress.Parse(row.Cells["DstAddress"].Value.ToString().Substring(0, row.Cells["DstAddress"].Value.ToString().LastIndexOf(':')))
                                                                                            .Equals(Watcher.Ipv4Address)).ToList();
            foreach (var row in removeRows)
                ConnectionList.Rows.Remove(row);

            // 恢复列表更新
            ConnectionListUpdateTimer.Start();
        }

        /// <summary>
        ///     菜单项“阻止此连接”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 阻止此连接ToolStripMenuItem_Click(object sender, EventArgs e) {
            var needInit = _blockTable.Count == 0;

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

            foreach (DataGridViewRow row in BlockList.SelectedRows) {
                var hash = (row.Cells["SrcIP"].Value.ToString() + row.Cells["DstIP"].Value).GetHashCode();
                targets.Add((DataGridViewRow) _blockTable[hash]);
                _blockTable.Remove(hash);
            }

            foreach (var target in targets)
                BlockList.Rows.Remove(target);

            if (_blockTable.Count == 0)
                Poisoner.OnIPv4PacketReceive -= Poisoner_OnIPv4PacketReceive;
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
        ///     连接列表右键菜单关闭时触发的事件，重新开始更新列表。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void ConnectionListMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e) {
            ConnectionListUpdateTimer.Start();
        }
    }
}