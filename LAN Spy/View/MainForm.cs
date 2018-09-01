using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LAN_Spy.Model;
using LAN_Spy.Model.Classes;
using SharpPcap;
using SharpPcap.WinPcap;

namespace LAN_Spy.View {
    public partial class MainForm : Form {
        /// <summary>
        ///     <see cref="Model.Scanner"/> 模块实例。
        /// </summary>
        private Scanner Scanner { get; }
        
        /// <summary>
        ///     <see cref="Model.Poisoner"/> 模块实例。
        /// </summary>
        private Poisoner Poisoner { get; }
        
        /// <summary>
        ///     <see cref="Model.Watcher"/> 模块实例。
        /// </summary>
        private Watcher Watcher { get; }

        /// <summary>
        ///     获取所有模块的实例组合。
        /// </summary>
        // TODO:新增模块时请更新此处的代码
        private BasicClass[] Models => new BasicClass[] {Scanner, Poisoner, Watcher};

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
        ///     菜单项“启动所有模块”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 启动所有模块ToolStripMenuItem_Click(object sender, EventArgs e) {
            // 判断是否为重启
            if (启动所有模块ToolStripMenuItem.Text == "重启所有模块" && StopModels()) {
                MessageBox.Show("一个或多个模块未能成功初始化，请检查。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 启动模块
            if (!StartupModels(Models))
                MessageBox.Show("一个或多个模块未能成功初始化，请单独启动模块。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (Models.First(item => item.CurDevIndex == -1) is null) {
                MessageBox.Show("所有模块均已成功设置。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                启动所有模块ToolStripMenuItem.Text = "重启所有模块";
                启动扫描模块ToolStripMenuItem.Text = 启动毒化模块ToolStripMenuItem.Text = 启动监视模块ToolStripMenuItem.Text = "停止模块";
                扫描主机ToolStripMenuItem.Enabled = true;
                侦测主机ToolStripMenuItem.Enabled = true;
            }
        }

        /// <summary>
        ///     <see cref="MainForm"/> 激活时的事件。
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
            if (StopModels())
                Close();
            else
                Environment.Exit(0);
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
            启动扫描模块ToolStripMenuItem.Text = 启动毒化模块ToolStripMenuItem.Text = 启动监视模块ToolStripMenuItem.Text = "启动模块";
            扫描主机ToolStripMenuItem.Enabled = false;
            侦测主机ToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        ///     停止所有模块的工作。
        /// </summary>
        /// <returns>成功返回true，错误返回false。</returns>
        private bool StopModels() {
            var result = true;
            
            // TODO:新增模块时请更新此处的代码
            if (Scanner is null)
                result = false;
            else {
                Scanner.Reset();
                Scanner.CurDevIndex = -1;
            }

            if (Poisoner is null)
                result = false;
            else {
                Poisoner.StopPoisoning();
                Poisoner.Reset();
                Poisoner.CurDevIndex = -1;
            }

            if (Watcher is null)
                result = false;
            else {
                Watcher.StopWatching();
                Watcher.Reset();
                Watcher.CurDevIndex = -1;
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
                if (!StartupModels(new []{Scanner}))
                    MessageBox.Show("模块未能成功初始化，请检查。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("模块已成功设置。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                启动所有模块ToolStripMenuItem.Text = "重启所有模块";
                启动扫描模块ToolStripMenuItem.Text = "停止模块";
                扫描主机ToolStripMenuItem.Enabled = true;
                侦测主机ToolStripMenuItem.Enabled = true;
            }
            else {
                Scanner.Reset();
                Scanner.CurDevIndex = -1;
                启动扫描模块ToolStripMenuItem.Text = "启动模块";
                扫描主机ToolStripMenuItem.Enabled = false;
                侦测主机ToolStripMenuItem.Enabled = false;
            }
        }

        /// <summary>
        ///     启动指定的模块。
        /// </summary>
        /// <param name="models">需要启动的模块列表。</param>
        /// <returns>成功返回true，错误返回false。</returns>
        private bool StartupModels(IEnumerable<BasicClass> models) {
            // 判断模块是否初始化
            var modelsList = models.ToList();
            if (modelsList.Contains(null))
                return false;

            // 选择设备
            var index = new PointerPacker(-1);
            var chooseDevice = new ChooseDevice(ref index);
            while (true) {
                chooseDevice.ShowDialog();

                // 用户点击了取消，中止本次操作
                var value = (int) index.Item;
                if (value == -1) return true;

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
            var loadingThread = new Thread(load => {
                var loading = new Loading("设置模块中，请稍候");
                try { Application.Run(loading); }
                catch (Exception) { loading.Close(); }
            });
            loadingThread.Start();
            
            // 设置模块
            foreach (var model in modelsList)
                model.CurDevIndex = (int) index.Item;
            
            // 终止载入界面
            loadingThread.Abort();
            while (loadingThread.IsAlive)
                Thread.Sleep(500);

            return true;
        }
        
        /// <summary>
        ///     菜单项“启动模块”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 扫描主机ToolStripMenuItem_Click(object sender, EventArgs e) {
            // 创建载入界面
            var loadingThread = new Thread(load => {
                var loading = new Loading("正在扫描，请稍候");
                try { Application.Run(loading); }
                catch (Exception) { loading.Close(); }
            });
            loadingThread.Start();

            // 扫描主机
            Scanner.ScanForTarget();
            foreach (var host in Scanner.HostList) {
                // 格式化MAC地址
                var temp = new StringBuilder(host.PhysicalAddress.ToString());
                if (temp.Length == 12)
                    temp = temp.Insert(2, '-').Insert(5, '-').Insert(8, '-').Insert(11, '-').Insert(14, '-');
                HostList.Rows.Add(host.IPAddress, temp);
            }

            // 终止载入界面
            loadingThread.Abort();
            while (loadingThread.IsAlive)
                Thread.Sleep(500);
        }
    }
}
