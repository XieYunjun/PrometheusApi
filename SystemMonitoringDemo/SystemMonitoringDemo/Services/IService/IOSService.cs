using SystemMonitoringDemo.Base.Dto.MonitirDataDto;

namespace SystemMonitoringDemo.Services.IService
{
    /// <summary>
    /// IOSService
    /// </summary>
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
        Task<List<MonitorDataDto>> GetMemoryUsageAsync(MonitorDataInputDto inputDto);

        /// <summary>
        /// 磁盘使用率
        /// </summary>
        /// <returns></returns>
        Task<List<MonitorDataDto>> GetDiskUsageAsync(MonitorDataInputDto inputDto);

        /// <summary>
        /// 网络流量
        /// </summary>
        /// <returns></returns>
        Task<List<MonitorDataDto>> GetNetworkTrafficAsync(MonitorDataInputDto inputDto);

        /// <summary>
        /// 网络带宽使用
        /// </summary>
        /// <returns></returns>
        Task<List<MonitorDataDto>> GetNetworkBandwidthUseAsync(MonitorDataInputDto inputDto);

    }
}
