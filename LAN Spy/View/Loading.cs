using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using LAN_Spy.Controller;
using Message = LAN_Spy.Controller.Message;

namespace LAN_Spy.View {
    public partial class Loading : Form {
        /// <summary>
        ///     载入提示信息末尾的点的数量。
        /// </summary>
        private int _dotCount;
        
        /// <inheritdoc />
        /// <summary>
        ///     初始化 <see cref="Loading" /> 窗口。
        /// </summary>
        /// <param name="message">载入窗口需要显示的信息。</param>
        public Loading(string message) {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            LoadingInfoLabel.Text = message;
        }
        
        /// <summary>
        ///     取消按钮单击事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        /// <exception cref=""></exception>
        private void CancelButton_Click(object sender, EventArgs e) {
            MessagePipe.SendOutMessage(new KeyValuePair<Message, object>(Message.UserCancel, null));
            Close();
        }

        /// <summary>
        ///     计时器到时触发事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void TickTimer_Tick(object sender, EventArgs e) {
            if (++_dotCount == 4) {
                _dotCount = 0;
                LoadingInfoLabel.Text = LoadingInfoLabel.Text.TrimEnd('.');
            }
            else
                LoadingInfoLabel.Text += '.';
            LoadingInfoLabel.Left = (Width - LoadingInfoLabel.Width) / 2;
        }
    }
}
