namespace SystemMonitoringDemo.Base.Const
{
    /// <summary>
    /// Prometheus常量
    /// </summary>
    public static class PrometheusConsts
    {
        /// <summary>
        /// PROMETHEUS 默认端口
        /// </summary>
        public readonly static string PROMETHEUS_DEFAULT_PORT = "9090";

        /// <summary>
        /// WIN 目标服务器默认端口 
        /// </summary>
        public readonly static string WIN_DEFAULT_PORT = "9182";

        /// <summary>
        /// LINUX 目标服务器默认端口 
        /// </summary>
        public readonly static string LINUX_DEFAULT_PORT = "9100";

        /// <summary>
        /// 默认Int 值为1
        /// </summary>
        public readonly static int DEFAULT_INT = 1;

        /// <summary>
        /// 小时 h
        /// </summary>
        public readonly static string HOUR_STR = "h";

        /// <summary>
        /// 分钟 m
        /// </summary>
        public readonly static string MINUTES_STR = "m";

        /// <summary>
        /// 默认step
        /// </summary>
        public readonly static int DEFAULT_STEP = 14;

        /// <summary>
        /// 默认时长
        /// </summary>
        public readonly static int DEFAULT_TIMECOUNT = 5;

        /// <summary>
        /// 系统CPU
        /// </summary>
        public readonly static string LINUX_CPU_SYSTEM = "system";

        /// <summary>
        /// 用户CPU
        /// </summary>
        public readonly static string LINUX_CPU_USER = "user";

        /// <summary>
        /// 磁盘ioCPU
        /// </summary>
        public readonly static string LINUX_CPU_IOWAIT = "iowait";

        /// <summary>
        /// 总CPU
        /// </summary>
        public readonly static string LINUX_CPU_TOTAL = "total";

        /// <summary>
        /// 虚拟
        /// </summary>
        public readonly static string VIRTUAL = "virtual";

        /// <summary>
        /// 物理
        /// </summary>
        public readonly static string PHYSICAL = "physical";

        /// <summary>
        /// 上传
        /// </summary>
        public readonly static string SENT = "sent";

        /// <summary>
        /// 下载
        /// </summary>
        public readonly static string DOWNLOAD = "download";

    }
}
