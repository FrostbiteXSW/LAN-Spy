namespace LAN_Spy.View {
    partial class Loading {
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
            this.components = new System.ComponentModel.Container();
            this.LoadingInfoLabel = new System.Windows.Forms.Label();
            this.CancelButton = new System.Windows.Forms.Button();
            this.TickTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // LoadingInfoLabel
            // 
            this.LoadingInfoLabel.AutoSize = true;
            this.LoadingInfoLabel.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LoadingInfoLabel.Location = new System.Drawing.Point(63, 23);
            this.LoadingInfoLabel.Name = "LoadingInfoLabel";
            this.LoadingInfoLabel.Size = new System.Drawing.Size(209, 20);
            this.LoadingInfoLabel.TabIndex = 0;
            this.LoadingInfoLabel.Text = "初始化模块中，请稍候";
            // 
            // CancelButton
            // 
            this.CancelButton.Font = new System.Drawing.Font("黑体", 10F);
            this.CancelButton.Location = new System.Drawing.Point(118, 71);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(98, 28);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "取消";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // TickTimer
            // 
            this.TickTimer.Enabled = true;
            this.TickTimer.Interval = 750;
            this.TickTimer.Tick += new System.EventHandler(this.TickTimer_Tick);
            // 
            // Loading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 111);
            this.ControlBox = false;
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.LoadingInfoLabel);
            this.Font = new System.Drawing.Font("宋体", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Loading";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "载入中...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LoadingInfoLabel;
        private new System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Timer TickTimer;
    }
}