using SystemMonitoringDemo.Base.Dto.MonitirDataDto;

namespace SystemMonitoringDemo.Services.IService
{
    public interface IOSService
    {
        /// <summary>
        /// Cpu使用率
        /// </summary>
        /// <returns></returns>
        Task<List<MonitorDataDto>> GetCpuUsageAsync(MonitorDataInputDto inputDto);

        /// <summary>
        /// 内存使用率
        /// </summary>
        /// <returns></returns>
        Task GetMemoryUsageAsync();

        /// <summary>
        /// 磁盘使用率
        /// </summary>
        /// <returns></returns>
        Task<List<MonitorDataDto>> GetDiskUsageAsync(MonitorDataInputDto inputDto);

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
