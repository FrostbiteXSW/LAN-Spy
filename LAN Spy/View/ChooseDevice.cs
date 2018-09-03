using LAN_Spy.Model.Classes;
using SharpPcap;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using SharpPcap.WinPcap;

namespace LAN_Spy.View {
    public partial class ChooseDevice : Form {
        /// <summary>
        ///     回传设备选项的 <see cref="PointerPacker"/> 句柄。
        /// </summary>
        private readonly PointerPacker _index;
        
        /// <summary>
        ///     缓存设备的完整名称。
        /// </summary>
        private List<string> _devName = new List<string>();

        /// <summary>
        ///     设备列表鼠标悬停弹出窗口。
        /// </summary>
        private readonly ToolTip _deviceListTooltip = new ToolTip();
        
        /// <summary>
        ///     设备列表描述信息，用于鼠标悬停弹出窗口中显示。
        /// </summary>
        private readonly List<string> _deviceListInfo = new List<string>();

        /// <summary>
        ///     鼠标最后一次悬停的项的索引号，用于防止反复更新鼠标悬停弹出窗口。
        /// </summary>
        private int _deviceListTempIndex = -1;

        /// <inheritdoc />
        /// <summary>
        ///     初始化 <see cref="ChooseDevice" /> 窗口。
        /// </summary>
        /// <param name="index">用于回传设备选项的编号。</param>
        public ChooseDevice(ref PointerPacker index) {
            InitializeComponent();
            _index = index;
            
            // 设置鼠标悬停弹出窗口参数
            _deviceListTooltip.AutoPopDelay = 5000;
            _deviceListTooltip.InitialDelay = 1000;
            _deviceListTooltip.ReshowDelay = 500;

            // 获取设备信息
            CheckForIllegalCrossThreadCalls = false; 
            new Thread(getDeviceInfo => {
                var collection = new List<string>();
                foreach (var dev in CaptureDeviceList.Instance) {
                    collection.Add(((WinPcapDevice) dev).Interface.FriendlyName);
                    _devName.Add(dev.Name);
                    var buf = dev.ToString();
                    buf = buf.Replace("\n\n", "\n");
                    buf = buf.Replace("\n", "\r\n");
                    _deviceListInfo.Add(buf);
                }
                lock (DeviceList) {
                    foreach (var str in collection)
                        DeviceList.Items.Add(str);
                    DeviceList.Enabled = true;
                }
                CheckForIllegalCrossThreadCalls = true; 
            }).Start();
        }
        
        /// <summary>
        ///     设备列表鼠标移动事件，显示 <see cref="ToolTip"/> 弹出窗口。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void DeviceList_MouseMove(object sender, MouseEventArgs e) {
            var index = ((ListBox) sender).IndexFromPoint(e.Location);
            if (index < 0) {
                _deviceListTempIndex = -1;
                return;
            }
            if (_deviceListTempIndex != -1 && _deviceListTempIndex == index) return;
            _deviceListTempIndex = index;
            _deviceListTooltip.SetToolTip(DeviceList, _deviceListInfo[index]);
        }

        /// <summary>
        ///     设备列表选项更改触发事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void DeviceList_SelectedIndexChanged(object sender, EventArgs e) {
            if (DeviceList.SelectedIndex >= 0 && DeviceList.SelectedIndex < CaptureDeviceList.Instance.Count)
                CommitButton.Enabled = true;
            else
                CommitButton.Enabled = false;
        }

        /// <summary>
        ///     确认按钮单击事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void CommitButton_Click(object sender, EventArgs e) {
            _index.Item = _devName[DeviceList.SelectedIndex];
            Close();
        }

        /// <summary>
        ///     取消按钮单击事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void CancelButton_Click(object sender, EventArgs e) {
            _index.Item = "";
            Close();
        }

        /// <summary>
        ///     双击设备列表触发事件。
        /// </summary>
        /// <param name="sender">触发事件的控件对象。</param>
        /// <param name="e">事件的参数。</param>
        private void DeviceList_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (DeviceList.SelectedIndex >= 0 && DeviceList.SelectedIndex < CaptureDeviceList.Instance.Count)
                CommitButton_Click(sender, e);
        }
    }
}
