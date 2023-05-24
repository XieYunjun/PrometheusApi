using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// LINUX URL
        /// </summary>
        private readonly string LINUXURL = "http://192.168.1.122:9090/api/v1/query_range";

        /// <summary>
        /// WIN URL
        /// </summary>
        private readonly string WINURL = "http://192.168.1.138:9090/api/v1/query_range";

        public MonirorDataController(ILogger<MonirorDataController> logger, Func<Type, IOSService> oSServiceList)
        {
            this._logger = logger;
            _winService = oSServiceList(typeof(WinService));
            _linuxService = oSServiceList(typeof(LinuxService));
        }

        [HttpGet(Name = "GetCPU")]
        public async Task<IActionResult> GetCPU()
        {
            await _winService.GetCpuUsageAsync();

            return Ok(new {code = 200,msh = ""});
        }
    }
}
