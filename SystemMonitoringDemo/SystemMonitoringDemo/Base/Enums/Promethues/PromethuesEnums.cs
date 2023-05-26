using System.ComponentModel;

namespace SystemMonitoringDemo.Base.Enums.Promethues
{
    /// <summary>
    /// Prometheus 状态错误码
    /// </summary>
    public enum ResponseErrorCode
    {
        /// <summary>
        /// 参数错误或者缺失
        /// </summary>
        [Description("参数错误或者缺失")]
        BadRequest = 404,

        /// <summary>
        /// 表达式无法执行
        /// </summary>
        [Description("表达式无法执行")]
        Unprocessable = 422,

        /// <summary>
        /// 请求超时或者被中断
        /// </summary>
        [Description("请求超时或者被中断")]
        ServiceUnavailiable = 503
    }

    /// <summary>
    /// 响应数据类型
    /// </summary>
    public enum ResponseDataType
    {
        /// <summary>
        /// 区间向量
        /// </summary>
        [Description("区间向量")]
        matrix = 0,

        /// <summary>
        /// 瞬时向量
        /// </summary>
        [Description("瞬时向量")]
        vector = 1,

        /// <summary>
        /// 标量
        /// </summary>
        [Description("标量")] 
        scalar = 2,

        /// <summary>
        /// 字符串
        /// </summary>
        [Description("字符串")]
        _string = 3
    }

    /// <summary>
    /// 时间类型
    /// </summary>
    public enum TimeType
    {
        /// <summary>
        /// 分钟
        /// </summary>
        [Description("分钟")]
        Minutes = 0,

        /// <summary>
        /// 小时
        /// </summary>
        [Description("小时")]
        Hour = 1,
    }

    /// <summary>
    /// 查询指令类型
    /// </summary>
    public enum QueryType
    {
        /// <summary>
        /// WinCpu使用率
        /// </summary>
        [Description("WinCpu使用率")]
        WinGetCpuUsage = 0,

        /// <summary>
        /// Win磁盘使用率
        /// </summary>
        [Description("Win磁盘使用率")]
        WinGetDiskUsage = 1,

        /// <summary>
        /// Win虚拟内存使用率
        /// </summary>
        [Description("Win虚拟内存使用率")]
        WinGetVirtualMemoryUsage = 2,

        /// <summary>
        /// Win物理内存使用率
        /// </summary>
        [Description("Win物理内存使用率")]
        WinGetPhysicalMemoryUsage = 3,

        /// <summary>
        /// Win网络带宽平均出网
        /// </summary>
        [Description("Win网络带宽平均出网")]
        WinGetNetworkBandwidthSentUse = 4,

        /// <summary>
        /// Win网络带宽平均入网
        /// </summary>
        [Description("Win网络带宽平均入网")]
        WinGetNetworkBandwidthReceivedUse = 5,

        /// <summary>
        /// Win网络流量上传
        /// </summary>
        [Description("Win网络流量上传")]
        WinGetNetworkTrafficSent = 6,

        /// <summary>
        /// Win网络流量下载
        /// </summary>
        [Description("Win网络流量下载")]
        WinGetNetworkTrafficReceived = 7,

        /// <summary>
        /// LinuxCpu 系统使用率
        /// </summary>
        [Description("LinuxCpu 系统使用率")]
        LinuxGetCpuUsageSystem = 8,

        /// <summary>
        /// LinuxCpu 用户使用率
        /// </summary>
        [Description("LinuxCpu 用户使用率")]
        LinuxGetCpuUsageUser = 9,

        /// <summary>
        /// LinuxCpu 磁盘IO 使用率
        /// </summary>
        [Description("LinuxCpu 磁盘IO 使用率")]
        LinuxGetCpuUsageIowait = 10,

        /// <summary>
        /// LinuxCpu 总使用率
        /// </summary>
        [Description("LinuxCpu 总使用率")]
        LinuxGetCpuUsageTotal = 11,

        /// <summary>
        /// Linux磁盘使用率
        /// </summary>
        [Description("Linux磁盘使用率")]
        LinuxGetDiskUsage = 12,

        /// <summary>
        /// Linux内存使用率
        /// </summary>
        [Description("Linux内存使用率")]
        LinuxGetMemoryUsage = 13,

        /// <summary>
        /// Linux网络带宽平均出网
        /// </summary>
        [Description("Linux网络带宽平均出网")]
        LinuxGetNetworkBandwidthUseSent = 14,

        /// <summary>
        /// Linux网络带宽平均入网
        /// </summary>
        [Description("Linux网络带宽平均入网")]
        LinuxGetNetworkBandwidthUseReceived = 15,

        /// <summary>
        /// Linux网络流量上传
        /// </summary>
        [Description("Linux网络流量上传")]
        LinuxGetNetworkTrafficTransmit = 16,

        /// <summary>
        /// Linux网络流量下载
        /// </summary>
        [Description("Linux网络流量下载")]
        LinuxGetNetworkTrafficReceive = 17,
    }

    /// <summary>
    /// 系统类型
    /// </summary>
    public enum SystemType
    {
        /// <summary>
        /// 默认本机系统
        /// </summary>
        [Description("默认本机系统")]
        Default = 0,

        /// <summary>
        /// Win
        /// </summary>
        [Description("Win")]
        Windows = 1,

        /// <summary>
        /// Linux
        /// </summary>
        [Description("Linux")]
        Linux = 2,
    }

}
