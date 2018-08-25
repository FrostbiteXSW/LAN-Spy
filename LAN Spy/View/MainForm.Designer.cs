namespace LAN_Spy.View {
    partial class MainForm {
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
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.开始ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.启动所有模块ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止所有模块ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.扫描ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.毒化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.监视ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SplitLine = new System.Windows.Forms.Label();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开始ToolStripMenuItem,
            this.扫描ToolStripMenuItem,
            this.毒化ToolStripMenuItem,
            this.监视ToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(584, 25);
            this.MainMenu.TabIndex = 0;
            // 
            // 开始ToolStripMenuItem
            // 
            this.开始ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.启动所有模块ToolStripMenuItem,
            this.停止所有模块ToolStripMenuItem,
            this.关于ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.开始ToolStripMenuItem.Name = "开始ToolStripMenuItem";
            this.开始ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.开始ToolStripMenuItem.Text = "开始";
            // 
            // 启动所有模块ToolStripMenuItem
            // 
            this.启动所有模块ToolStripMenuItem.Name = "启动所有模块ToolStripMenuItem";
            this.启动所有模块ToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.启动所有模块ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.启动所有模块ToolStripMenuItem.Text = "启动所有模块";
            this.启动所有模块ToolStripMenuItem.Click += new System.EventHandler(this.启动所有模块ToolStripMenuItem_Click);
            // 
            // 停止所有模块ToolStripMenuItem
            // 
            this.停止所有模块ToolStripMenuItem.Name = "停止所有模块ToolStripMenuItem";
            this.停止所有模块ToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.停止所有模块ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.停止所有模块ToolStripMenuItem.Text = "停止所有模块";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.关于ToolStripMenuItem.Text = "关于...";
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // 扫描ToolStripMenuItem
            // 
            this.扫描ToolStripMenuItem.Name = "扫描ToolStripMenuItem";
            this.扫描ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.扫描ToolStripMenuItem.Text = "扫描";
            // 
            // 毒化ToolStripMenuItem
            // 
            this.毒化ToolStripMenuItem.Name = "毒化ToolStripMenuItem";
            this.毒化ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.毒化ToolStripMenuItem.Text = "毒化";
            // 
            // 监视ToolStripMenuItem
            // 
            this.监视ToolStripMenuItem.Name = "监视ToolStripMenuItem";
            this.监视ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.监视ToolStripMenuItem.Text = "监视";
            // 
            // SplitLine
            // 
            this.SplitLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SplitLine.Location = new System.Drawing.Point(0, 27);
            this.SplitLine.Name = "SplitLine";
            this.SplitLine.Size = new System.Drawing.Size(584, 2);
            this.SplitLine.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.SplitLine);
            this.Controls.Add(this.MainMenu);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.MainMenu;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "GUI for LAN Spy";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem 开始ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 启动所有模块ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停止所有模块ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 扫描ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 毒化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 监视ToolStripMenuItem;
        private System.Windows.Forms.Label SplitLine;
    }
}