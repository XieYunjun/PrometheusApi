using System.Text.Json;
using SystemMonitoringDemo.Base.Dto.MonitirDataDto;
using SystemMonitoringDemo.Base.Dto.PrometheusDto;
using SystemMonitoringDemo.Extensions;
using SystemMonitoringDemo.Services.IService;

namespace SystemMonitoringDemo.Services.Service
{
    public class WinService : IOSService
    {
        private readonly IHttpClientExtension _httpClient;

        private readonly IConvertDataExtension _convertDataExtension;


        public WinService(IHttpClientExtension httpClient, IConvertDataExtension convertDataExtension)
        {
            _httpClient = httpClient;
            _convertDataExtension = convertDataExtension;
        }

        public async Task<List<MonitorAxisDataDto>> GetCpuUsageAsync()
        {
            // query_range?quety=
            // 100 - (avg by(instance)(irate(windows_cpu_time_total{instance="192.168.1.138:9182",mode="idle"}[5m]))*100)
            // {"status":"success","data":{"resultType":"matrix","result":[
            // {"metric":{"instance":"192.168.1.138:9182"},"values":[
            // [1684195789.088,"4.334610259826789"],
            // [1684195803.088,"7.837809125449951"],
            // [1684195817.088,"3.6460069675956674"]
            // ]}]}}

            var url = "http://127.0.0.1:9090/api/v1/query_range?";

            var dic = new Dictionary<string, string>();

            //dic.Add("query", "100 - (avg by(instance)(irate(windows_cpu_time_total{instance=\"192.168.1.138:9182\",mode=\"idle\"}[5m]))*100)");
            dic.Add("query", "(avg without (cpu) (sum(irate(windows_cpu_time_total{instance=\"192.168.1.138:9182\",mode!=\"idle\"}[5m])) by (mode)) / 16)* 100");
            dic.Add("start", TimeStampExtension.GetSecondTimeStamp(DateTime.UtcNow.AddMinutes(-10)));
            dic.Add("end", TimeStampExtension.GetSecondTimeStamp(DateTime.UtcNow));
            dic.Add("step", "15");

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url,dic);

            if (res is not null)
            {
                var datas = _convertDataExtension.ConvertData(res.data.result, nameof(PrometheusResultMetric.mode));

                return datas;
            }

            return new List<MonitorAxisDataDto>();
        }

        public Task GetDiskUsageAsync()
        {
            // query_range?query=
            // rate(windows_logical_disk_split_ios_total{instance="192.168.1.138:9182"}[60m])

            // {"status":"success","data":{"resultType":"matrix","result":[
            // {"metric":{"instance":"192.168.1.138:9182","job":"x86-win","volume":"C:"},"values":[[1684196965.960,"0.1090654900610934"]]},
            // {"metric":{"instance":"192.168.1.138:9182","job":"x86-win","volume":"D:"},"values":[[1684196965.960,"0.005578797445580225"]]},
            // {"metric":{"instance":"192.168.1.138:9182","job":"x86-win","volume":"E:"},"values":[[1684196965.960,"0.0005578797445580225"]]},
            // {"metric":{"instance":"192.168.1.138:9182","job":"x86-win","volume":"F:"},"values":[[1684196965.960,"0.6410038264971678"]]}]}}

            throw new NotImplementedException();
        }

        public Task GetMemoryUsageAsync()
        {
            // 虚拟内存使用率 query_range?quety=
            // (windows_os_virtual_memory_bytes{instance = "192.168.1.138:9182"} - windows_os_virtual_memory_free_bytes{instance = "192.168.1.138:9182"}) / windows_os_virtual_memory_bytes{instance = "192.168.1.138:9182"}
            // {"status":"success","data":{"resultType":"matrix","result":[
            // {"metric":{"instance":"192.168.1.138:9182","job":"x86-win"},"values":[
            // [1684196295.713,"0.658931309416835"],
            // [1684196309.713,"0.6595236727923468"],
            // [1684196323.713,"0.6582989461608247"],
            // [1684196337.713,"0.6585498997679343"]
            // ]}]}}

            // 物理内存使用率 query_range?quety=
            // (windows_cs_physical_memory_bytes{instance = "192.168.1.138:9182"} - windows_os_physical_memory_free_bytes{instance = "192.168.1.138:9182"}) / windows_cs_physical_memory_bytes{instance = "192.168.1.138:9182"} 
            // {"status":"success","data":{"resultType":"matrix","result":[
            // {"metric":{"instance":"192.168.1.138:9182","job":"x86-win"},"values":[
            // [1684196655.704,"0.6346970841978536"],
            // [1684196669.704,"0.63469326428631"],
            // [1684196683.704,"0.6344586580523417"]
            // ]}]}}

            throw new NotImplementedException();
        }

        public Task GetNetworkBandwidthUseAsync()
        {
            // 平均出网 query_range?quety=
            // rate(windows_net_bytes_sent_total{instance="192.168.1.138:9182"}[60m])
            // {"status":"success","data":{"resultType":"matrix","result":[
            // {"metric":{"instance":"192.168.1.138:9182","job":"x86-win","nic":"Broadcom_NetXtreme_Gigabit_Ethernet"},"values":[
            // [1684197945.649,"301.86150367991115"],[1684197959.649,"300.2332222127072"]
            // ]}]}}

            // 平均入网
            // rate(windows_net_bytes_received_total{instance="192.168.1.138:9182"}[60m])


            throw new NotImplementedException();
        }

        public Task GetNetworkTrafficAsync()
        {
            // 总流量 query_range?quety=

            // sum(increase(windows_net_bytes_total{instance="192.168.1.138:9182"}[1h])) by (instance)
            // {"status":"success","data":{"resultType":"matrix","result":[
            // {"metric":{"instance":"192.168.1.138:9182"},"values":[
            // [1684198291.238,"4030714.8075303617"],
            // [1684198305.238,"4052441.050791452"]
            // ]}]}}


            throw new NotImplementedException();
        }
    }
}
