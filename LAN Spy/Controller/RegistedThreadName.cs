namespace LAN_Spy.Controller {
    /// <summary>
    ///     注册的无重复线程名称，为 <see cref="TaskHandler"/> 内区分线程的唯一标志，必须设置在线程的 Name 属性上。
    /// </summary>
    public enum RegistedThreadName {
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
        StartPoisoning
    }
}
