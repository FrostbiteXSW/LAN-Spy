using LAN_Spy.Model;
using LAN_Spy.Model.Classes;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
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
            // 判断模块是否初始化
            if (Models.Contains(null)) {
                MessageBox.Show("一个或多个模块未能成功初始化，请单独启动模块。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 选择设备
            var index = new PointerPacker(-1);
            var chooseDevice = new ChooseDevice(ref index);
            while (true) {
                chooseDevice.ShowDialog();

                // 用户点击了取消，中止本次操作
                var value = (int) index.Item;
                if (value == -1) return;

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
            foreach (var model in Models)
                model.CurDevIndex = (int) index.Item;
            
            // 终止载入界面
            loadingThread.Abort();
            while (loadingThread.IsAlive)
                Thread.Sleep(500);

            MessageBox.Show("所有模块均已成功设置。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            if (!StopModels())
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
            if (StopModels()) 
                MessageBox.Show("一个或多个模块未能成功初始化，请检查。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        ///     停止所有模块的工作。
        /// </summary>
        /// <returns>返回为 true 则发生错误。</returns>
        private bool StopModels() {
            var error = false;
            
            // TODO:新增模块时请更新此处的代码
            if (Poisoner is null)
                error = true;
            else
                Poisoner.StopPoisoning();

            if (Watcher is null)
                error = true;
            else
                Watcher.StopWatching();

            return error;
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
    }
}
