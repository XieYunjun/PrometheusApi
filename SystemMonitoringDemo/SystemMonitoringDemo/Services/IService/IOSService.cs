namespace SystemMonitoringDemo.Services.IService
{
    public interface IOSService
    {
        /// <summary>
        /// Cpu使用率
        /// </summary>
        /// <returns></returns>
        Task GetCpuUsageAsync();

        /// <summary>
        /// 内存使用率
        /// </summary>
        /// <returns></returns>
        Task GetMemoryUsageAsync();

        /// <summary>
        /// 磁盘使用率
        /// </summary>
        /// <returns></returns>
        Task GetDiskUsageAsync();

        /// <summary>
        /// 网络流量
        /// </summary>
        /// <returns></returns>
        Task GetNetworkTrafficAsync();

        /// <summary>
        /// 网络带宽使用
        /// </summary>
        /// <returns></returns>
        Task GetNetworkBandwidthUseAsync();

    }
}
