using System;
using System.Windows.Forms;
using LAN_Spy.Model;
using LAN_Spy.Model.Classes;

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

        /// <inheritdoc />
        /// <summary> 
        ///     初始化 <see cref="MainForm" /> 窗口。
        /// </summary>
        /// <param name="models">传入的创建完成的模块实例组，此实例句柄在绑定完成后将被清除。</param>
        public MainForm(ref BasicClass[] models) {
            InitializeComponent();

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
            var chooseDevice = new ChooseDevice(Scanner);
            chooseDevice.ShowDialog();

            // TODO:初始化模块，移除下面的测试用代码
            Text = Scanner.CurDevIndex.ToString();
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
            var error = false;

            if (Poisoner is null) {
                error = true;
                MessageBox.Show(@"检测到毒化器实例初始化异常。", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                Poisoner.StopPoisoning();

            if (Watcher is null) {
                error = true;
                MessageBox.Show(@"检测到监视器实例初始化异常。", @"警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                Watcher.StopWatching();

            if (!error && !Poisoner.IsStarted && !Watcher.IsStarted)
                Close();
            else
                Environment.Exit(0);
        }
    }
}
