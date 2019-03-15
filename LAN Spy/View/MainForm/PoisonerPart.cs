using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using LAN_Spy.Controller;
using LAN_Spy.Controller.Classes;
using LAN_Spy.Model;
using LAN_Spy.Model.Classes;
using PacketDotNet;
using Message = LAN_Spy.Controller.Message;

namespace LAN_Spy.View.MainForm {
    public partial class MainPart {
        /// <summary>
        ///     <see cref="Model.Poisoner" /> 模块实例。
        /// </summary>
        private Poisoner Poisoner { get; set; }

        /// <summary>
        ///     启用与 <see cref="Model.Poisoner" /> 相关的控件。
        /// </summary>
        private void EnablePoisonerItems() {
            启动毒化模块ToolStripMenuItem.Text = "停止模块";
            开始毒化ToolStripMenuItem.Enabled = true;
            添加到目标组1ToolStripMenuItem.Enabled = true;
            添加到目标组2ToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        ///     禁用与 <see cref="Model.Poisoner" /> 相关的控件。
        /// </summary>
        private void DisablePoisonerItems() {
            启动毒化模块ToolStripMenuItem.Text = "启动模块";
            开始毒化ToolStripMenuItem.Enabled = false;
            添加到目标组1ToolStripMenuItem.Enabled = false;
            添加到目标组2ToolStripMenuItem.Enabled = false;
            Target1List.Rows.Clear();
            Target2List.Rows.Clear();
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
                    Poisoner.Stop();
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
        ///     开始毒化菜单项启用状态发生改变时触发事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 开始毒化ToolStripMenuItem_EnabledChanged(object sender, EventArgs e) {
            if (!开始毒化ToolStripMenuItem.Enabled)
                开始毒化ToolStripMenuItem.Text = "开始毒化";
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
                        Poisoner.Stop();
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
                var task = new Thread(stop => { Poisoner.Stop(); }) {Name = RegisteredThreadName.StopPoisoning.ToString()};
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
    }
}