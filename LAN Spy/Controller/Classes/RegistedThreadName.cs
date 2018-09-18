namespace LAN_Spy.Controller.Classes {
    /// <summary>
    ///     注册的无重复线程名称，为 <see cref="TaskHandler" /> 内区分线程的唯一标志，必须设置在线程的 Name 属性上。
    /// </summary>
    public enum RegisteredThreadName {
        /// <summary>
        ///     在Program.cs内，用以初始化程序。
        /// </summary>
        ProgramInit,

        /// <summary>
        ///     在 MainForm.cs 的 StartupModels 方法内，用以初始化核心模块。
        /// </summary>
        StartupModels,

        /// <summary>
        ///     在 MainForm.cs 的 扫描主机ToolStripMenuItem_Click 方法内，用以扫描主机。
        /// </summary>
        ScanForTarget,

        /// <summary>
        ///     在 MainForm.cs 的 侦测主机ToolStripMenuItem_Click 方法内，用以侦测主机。
        /// </summary>
        SpyForTarget,

        /// <summary>
        ///     在 MainForm.cs 的 开始毒化ToolStripMenuItem_Click 方法内，用以启动毒化工作。
        /// </summary>
        StartPoisoning,

        /// <summary>
        ///     在 MainForm.cs 的 开始监视ToolStripMenuItem_Click 方法内，用以启动监视工作。
        /// </summary>
        StartWatching,

        /// <summary>
        ///     在 MainForm.cs 的 停止所有模块ToolStripMenuItem_Click 方法内，用以停止所有模块。
        /// </summary>
        StopAllModels,

        /// <summary>
        ///     在 MainForm.cs 的 退出ToolStripMenuItem_Click 方法内，用以停止所有模块。
        /// </summary>
        ExitStopAllModels,

        /// <summary>
        ///     在 MainForm.cs 的 启动所有模块ToolStripMenuItem_Click 方法内，用以停止所有模块。
        /// </summary>
        RestartStopAllModels,

        /// <summary>
        ///     在 MainForm.cs 的 启动扫描模块ToolStripMenuItem_Click 方法内，用以停止扫描模块。
        /// </summary>
        StopScanner,

        /// <summary>
        ///     在 MainForm.cs 的 启动毒化模块ToolStripMenuItem_Click 方法内，用以停止毒化模块。
        /// </summary>
        StopPoisoner,

        /// <summary>
        ///     在 MainForm.cs 的 启动监视模块ToolStripMenuItem_Click 方法内，用以停止监视模块。
        /// </summary>
        StopWatcher,

        /// <summary>
        ///     在 MainForm.cs 的 开始毒化ToolStripMenuItem_Click 方法内，用以停止毒化工作。
        /// </summary>
        StopPoisoning,

        /// <summary>
        ///     在 MainForm.cs 的 开始监视ToolStripMenuItem_Click 方法内，用以停止监视工作。
        /// </summary>
        StopWatching
    }
}