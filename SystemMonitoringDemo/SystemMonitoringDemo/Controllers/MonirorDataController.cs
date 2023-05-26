using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemMonitoringDemo.Base.Dto.MonitirDataDto;
using SystemMonitoringDemo.Base.Enums.Promethues;
using SystemMonitoringDemo.Extensions;
using SystemMonitoringDemo.Services.IService;
using SystemMonitoringDemo.Services.Service;

namespace SystemMonitoringDemo.Controllers
{
    /// <summary>
    /// 监测
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MonirorDataController : ControllerBase
    {
        private readonly ILogger<MonirorDataController> _logger;

        private readonly IOSService _winService;

        private readonly IOSService _linuxService;

        private readonly IHttpClientExtension _httpClientExtension;

        private readonly IConvertDataExtension _convertDataExtension;

        /// <summary>
        /// 监测
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="oSServiceList"></param>
        /// <param name="httpClientExtension"></param>
        /// <param name="convertDataExtension"></param>
        public MonirorDataController(ILogger<MonirorDataController> logger, Func<Type, IOSService> oSServiceList, IHttpClientExtension httpClientExtension, IConvertDataExtension convertDataExtension)
        {
            this._logger = logger;
            _winService = oSServiceList(typeof(WinService));
            _linuxService = oSServiceList(typeof(LinuxService));
            _httpClientExtension = httpClientExtension;
            _convertDataExtension = convertDataExtension;
        }

        /// <summary>
        /// Cpu使用率
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet("GetCPU")]
        public async Task<IActionResult> GetCPUAsync([FromQuery] MonitorDataInputDto inputDto)
        {
            inputDto = _convertDataExtension.ConvertMonitorInputDto(inputDto);

            var res = inputDto.TargetSystemType == SystemType.Windows ? await _winService.GetCpuUsageAsync(inputDto) : await _linuxService.GetCpuUsageAsync(inputDto);

            return Ok(new { code = 200, data = res });
        }

        /// <summary>
        /// 内存使用率
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet("GetMemory")]
        public async Task<IActionResult> GetMemoryUsageAsync([FromQuery] MonitorDataInputDto inputDto)
        {
            inputDto = _convertDataExtension.ConvertMonitorInputDto(inputDto);

            var res = inputDto.TargetSystemType == SystemType.Windows ? await _winService.GetMemoryUsageAsync(inputDto) : await _linuxService.GetMemoryUsageAsync(inputDto);

            return Ok(new { code = 200, data = res });
        }

        /// <summary>
        /// 磁盘使用率
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet("GetDisk")]
        public async Task<IActionResult> GetDiskUsageAsync([FromQuery] MonitorDataInputDto inputDto)
        {
            inputDto = _convertDataExtension.ConvertMonitorInputDto(inputDto);

            var res = inputDto.TargetSystemType == SystemType.Windows ? await _winService.GetDiskUsageAsync(inputDto) : await _linuxService.GetDiskUsageAsync(inputDto);

            return Ok(new { code = 200, data = res });
        }

        /// <summary>
        /// 网络流量
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet("GetNetworkTraffic")]
        public async Task<IActionResult> GetNetworkTrafficAsync([FromQuery] MonitorDataInputDto inputDto)
        {
            inputDto = _convertDataExtension.ConvertMonitorInputDto(inputDto);

            var res = inputDto.TargetSystemType == SystemType.Windows ? await _winService.GetNetworkTrafficAsync(inputDto) : await _linuxService.GetNetworkTrafficAsync(inputDto);

            return Ok(new { code = 200, data = res });
        }

        /// <summary>
        /// 网络带宽使用
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpGet("GetNetworkBandwidthUse")]
        public async Task<IActionResult> GetNetworkBandwidthUseAsync([FromQuery] MonitorDataInputDto inputDto)
        {
            inputDto = _convertDataExtension.ConvertMonitorInputDto(inputDto);

            var res = inputDto.TargetSystemType == SystemType.Windows ? await _winService.GetNetworkBandwidthUseAsync(inputDto) : await _linuxService.GetNetworkBandwidthUseAsync(inputDto);

            return Ok(new { code = 200, data = res });
        }
    }
}
