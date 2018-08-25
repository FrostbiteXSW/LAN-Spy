using System;
using System.Windows.Forms;
using LAN_Spy.Model;

namespace LAN_Spy.View {
    public partial class MainForm : Form {
        /// <summary>
        ///     <see cref="Scanner"/> 模块实例。
        /// </summary>
        private readonly Scanner _scanner;
        
        /// <summary>
        ///     <see cref="Poisoner"/> 模块实例。
        /// </summary>
        private readonly Poisoner _poisoner;
        
        /// <summary>
        ///     <see cref="Watcher"/> 模块实例。
        /// </summary>
        private readonly Watcher _watcher;

        /// <inheritdoc />
        /// <summary> 
        ///     初始化 <see cref="T:LAN_Spy.View.MainForm" /> 窗口。
        /// </summary>
        /// <param name="scanner">传入的创建完成的扫描器实例，此实例句柄在绑定完成后将被清除。</param>
        /// <param name="poisoner">传入的创建完成的毒化器实例，此实例句柄在绑定完成后将被清除。</param>
        /// <param name="watcher">传入的创建完成的监视器实例，此实例句柄在绑定完成后将被清除。</param>
        public MainForm(ref Scanner scanner, ref Poisoner poisoner, ref Watcher watcher) {
            InitializeComponent();
            _scanner = scanner;
            _poisoner = poisoner;
            _watcher = watcher;
            scanner = null;
            poisoner = null;
            watcher = null;
            TopMost = true;
        }

        /// <summary>
        ///     菜单项“启动所有模块”单击时的事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void 启动所有模块ToolStripMenuItem_Click(object sender, EventArgs e) {
            var chooseDevice = new ChooseDevice(_scanner);
            chooseDevice.ShowDialog();

            // TODO:初始化模块，移除下面的测试用代码
            Text = _scanner.CurDevIndex.ToString();
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
            _poisoner.StopPoisoning();
            _watcher.StopWatching();

            if (!_poisoner.IsStarted && !_watcher.IsStarted)
                Close();
            else
                Environment.Exit(0);
        }
    }
}
