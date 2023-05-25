using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemMonitoringDemo.Base.Dto.MonitirDataDto;
using SystemMonitoringDemo.Base.Enums.Promethues;
using SystemMonitoringDemo.Extensions;
using SystemMonitoringDemo.Services.IService;
using SystemMonitoringDemo.Services.Service;

namespace SystemMonitoringDemo.Controllers
{
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
        /// LINUX URL
        /// </summary>
        private readonly string LINUXURL = "http://192.168.1.122:9090/api/v1/query_range";

        /// <summary>
        /// WIN URL
        /// </summary>
        private readonly string WINURL = "http://192.168.1.138:9090/api/v1/query_range";

        public MonirorDataController(ILogger<MonirorDataController> logger, Func<Type, IOSService> oSServiceList, IHttpClientExtension httpClientExtension, IConvertDataExtension convertDataExtension)
        {
            this._logger = logger;
            _winService = oSServiceList(typeof(WinService));
            _linuxService = oSServiceList(typeof(LinuxService));
            _httpClientExtension = httpClientExtension;
            _convertDataExtension = convertDataExtension;
        }

        [HttpGet(Name = "GetCPU")]
        public async Task<IActionResult> GetCPU([FromQuery] MonitorDataInputDto inputDto)
        {
            inputDto = _convertDataExtension.ConvertMonitorInputDto(inputDto);

            var res = inputDto.TargetSystemType == SystemType.Windows ? await _winService.GetCpuUsageAsync(inputDto) : await _linuxService.GetCpuUsageAsync(inputDto);

            return Ok(new { code = 200, data = res });
        }
    }
}
