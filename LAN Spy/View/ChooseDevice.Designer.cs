namespace LAN_Spy.View {
    partial class ChooseDevice {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.DeviceListLabel = new System.Windows.Forms.Label();
            this.DeviceList = new System.Windows.Forms.ListBox();
            this.CommitButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DeviceListLabel
            // 
            this.DeviceListLabel.AutoSize = true;
            this.DeviceListLabel.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DeviceListLabel.Location = new System.Drawing.Point(18, 9);
            this.DeviceListLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DeviceListLabel.Name = "DeviceListLabel";
            this.DeviceListLabel.Size = new System.Drawing.Size(98, 21);
            this.DeviceListLabel.TabIndex = 4;
            this.DeviceListLabel.Text = "设备列表";
            // 
            // DeviceList
            // 
            this.DeviceList.Enabled = false;
            this.DeviceList.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DeviceList.FormattingEnabled = true;
            this.DeviceList.HorizontalScrollbar = true;
            this.DeviceList.ItemHeight = 21;
            this.DeviceList.Location = new System.Drawing.Point(18, 38);
            this.DeviceList.Margin = new System.Windows.Forms.Padding(4);
            this.DeviceList.Name = "DeviceList";
            this.DeviceList.Size = new System.Drawing.Size(538, 277);
            this.DeviceList.TabIndex = 3;
            this.DeviceList.TabStop = false;
            this.DeviceList.SelectedIndexChanged += new System.EventHandler(this.DeviceList_SelectedIndexChanged);
            this.DeviceList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DeviceList_KeyUp);
            this.DeviceList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.DeviceList_MouseDoubleClick);
            this.DeviceList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DeviceList_MouseMove);
            // 
            // CommitButton
            // 
            this.CommitButton.Enabled = false;
            this.CommitButton.Font = new System.Drawing.Font("黑体", 12F);
            this.CommitButton.Location = new System.Drawing.Point(24, 326);
            this.CommitButton.Margin = new System.Windows.Forms.Padding(4);
            this.CommitButton.Name = "CommitButton";
            this.CommitButton.Size = new System.Drawing.Size(260, 56);
            this.CommitButton.TabIndex = 5;
            this.CommitButton.Text = "确认";
            this.CommitButton.UseVisualStyleBackColor = true;
            this.CommitButton.Click += new System.EventHandler(this.CommitButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Font = new System.Drawing.Font("黑体", 12F);
            this.CancelButton.Location = new System.Drawing.Point(292, 326);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(4);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(260, 56);
            this.CancelButton.TabIndex = 6;
            this.CancelButton.Text = "取消";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ChooseDevice
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(576, 392);
            this.ControlBox = false;
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.CommitButton);
            this.Controls.Add(this.DeviceListLabel);
            this.Controls.Add(this.DeviceList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChooseDevice";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "选择设备";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DeviceListLabel;
        private System.Windows.Forms.ListBox DeviceList;
        private System.Windows.Forms.Button CommitButton;
        private new System.Windows.Forms.Button CancelButton;
    }
}