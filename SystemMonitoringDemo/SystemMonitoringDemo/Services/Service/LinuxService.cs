using System;
using System.Text.Json;
using SystemMonitoringDemo.Base.Dto.MonitirDataDto;
using SystemMonitoringDemo.Base.Dto.PrometheusDto;
using SystemMonitoringDemo.Extensions;
using SystemMonitoringDemo.Services.IService;

namespace SystemMonitoringDemo.Services.Service
{
    public class LinuxService : IOSService
    {
        private readonly IHttpClientExtension _httpClient;

        public LinuxService(IHttpClientExtension httpClient)
        {
            _httpClient = httpClient;
        }
        #region CpuUsage
        public async Task<List<MonitorAxisDataDto>> GetCpuUsageAsync()
        {

            // 100 - (avg by(instance) (irate(node_cpu_seconds_total{instance="192.168.1.135:9100",mode="idle"}[5m]))*100)
            // {"status":"success","data":{"resultType":"matrix","result":[
            // {"metric":{"instance":"192.168.1.135:9100"},"values":[
            // [1684119018.786,"13.000000000004846"],
            // [1684119104.786,"10.824999999943742"],
            // [1684119190.786,"10.483333333322648"],
            // [1684119276.786,"11.958333333289687"],
            // [1684119362.786,"12.408333333344984"],
            // [1684119448.786,"11.935795719728233"],
            // [1684119534.786,"12.758333333331379"]
            // ]}]}}

            var url = "http://192.168.1.122:9090/api/v1/query_range?";

            var dic = new Dictionary<string, string>();

            dic.Add("query", "");
            dic.Add("start", "1684887301.38");
            dic.Add("end", "1684890901.38");
            dic.Add("step", "14");

            await GetCpuUsageSystemAsync(url, dic);

            await GetCpuUsageUserAsync(url, dic);

            await GetCpuUsageIowaitAsync(url, dic);

            await GetCpuUsageTotalAsync(url, dic);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Cpu 系统使用率
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        private async Task GetCpuUsageSystemAsync(string url,Dictionary<string,string> dic)
        {
            dic["query"] = "avg(rate(node_cpu_seconds_total{instance=\"192.168.1.135:9100\",mode=\"system\"}[5m])) by (instance) *100";

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);
        }

        /// <summary>
        /// Cpu 用户使用率
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        private async Task GetCpuUsageUserAsync(string url, Dictionary<string, string> dic)
        {
            dic["query"] = "avg(rate(node_cpu_seconds_total{instance=\"192.168.1.135:9100\",mode=\"user\"}[5m])) by (instance) *100";

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);
        }

        /// <summary>
        /// Cpu 磁盘IO 使用率
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        private async Task GetCpuUsageIowaitAsync(string url, Dictionary<string, string> dic)
        {
            dic["query"] = "avg(rate(node_cpu_seconds_total{instance=\"192.168.1.135:9100\",mode=\"iowait\"}[5m])) by (instance) *100";

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);
        }

        /// <summary>
        /// Cpu 总使用率
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        private async Task GetCpuUsageTotalAsync(string url, Dictionary<string, string> dic)
        {
            dic["query"] = "(1 - avg(rate(node_cpu_seconds_total{instance=\"192.168.1.135:9100\",mode=\"idle\"}[5m])) by (instance))*100";

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);
        }

        #endregion

        public Task GetDiskUsageAsync()
        {
            // node_filesystem_free_bytes{instance="192.168.1.135:9100"} / node_filesystem_size_bytes{instance="192.168.1.135:9100"}
            // {"status":"success","data":{"resultType":"matrix","result":
            // [{"metric":{"device":"/dev/nvme0n1p1","fstype":"vfat","instance":"192.168.1.135:9100","job":"node","mountpoint":"/boot/efi"},"values":[[1684118758.291,"0.9881203559306486"]]},
            // {"metric":{"device":"/dev/nvme0n1p2","fstype":"ext4","instance":"192.168.1.135:9100","job":"node","mountpoint":"/"},"values":[[1684118758.291,"0.5111413613472033"]]},
            // {"metric":{"device":"/dev/nvme0n1p2","fstype":"ext4","instance":"192.168.1.135:9100","job":"node","mountpoint":"/var/snap/firefox/common/host-hunspell"},"values":[[1684118758.291,"0.5111413613472033"],[1684118844.291,"0.5111410333958146"]]},
            // {"metric":{"device":"/dev/sda1","fstype":"ext4","instance":"192.168.1.135:9100","job":"node","mountpoint":"/home/mytek135/data"},"values":[[1684118758.291,"0.6241377475467904"]]}]}}

            throw new NotImplementedException();
        }

        public Task GetMemoryUsageAsync()
        {
            // (node_memory_MemTotal_bytes{instance="192.168.1.135:9100"}  - node_memory_MemFree_bytes{instance="192.168.1.135:9100"} ) / node_memory_MemTotal_bytes{instance="192.168.1.135:9100"} 
            // {"status":"success","data":{"resultType":"matrix","result":[
            // {"metric":{"instance":"192.168.1.135:9100","job":"node"},"values":[
            // [1684119182.044,"0.9565221575231009"],
            // [1684119268.044,"0.9605448313588436"],
            // [1684119354.044,"0.9674517004019713"],
            // [1684119440.044,"0.9675706311992986"],
            // [1684119526.044,"0.9730843814007037"],
            // [1684119612.044,"0.9761210646428362"]
            // ]}]}}
            throw new NotImplementedException();
        }

        public Task GetNetworkBandwidthUseAsync()
        {
            // irate(node_network_transmit_bytes_total{instance="192.168.1.135:9100"}[5m]) 平均出网
            //{"status":"success","data":
            //{"resultType":"matrix","result":[
            //{"metric":{"device":"enp2s0","instance":"192.168.1.135:9100","job":"node"},"values":[
            //[1684120289.307,"6178.2"],
            //[1684120375.307,"4537.2"],
            //[1684120461.307,"4741.333333333333"],
            //[1684120547.307,"4355.6"],
            //[1684120633.307,"4599.266666666666"],
            //[1684120719.307,"5167.466666666666"],
            //[1684120805.307,"5424.6"]
            //]}]}}


            // irate(node_network_receive_bytes_total{instance="192.168.1.135:9100"}[5m]) 平均入网

            throw new NotImplementedException();
        }

        public Task GetNetworkTrafficAsync()
        {
            // increase(node_network_transmit_bytes_total{instance="192.168.1.135:9100"}[5m]) 平均出网
            // {"status":"success","data":{"resultType":"matrix","result":[
            // {"metric":{"device":"enp2s0","instance":"192.168.1.135:9100","job":"node"},"values":[
            // [1684120788.163,"1476482.1052631577"],
            // [1684120874.163,"1524663.0713576444"]
            // ]}]}}

            // increase(node_network_receive_bytes_total{instance="192.168.1.135:9100"}[5m]) 平均入网


            throw new NotImplementedException();
        }
    }
}
