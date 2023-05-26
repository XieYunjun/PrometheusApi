using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Diagnostics;
using System.Text.Json;
using SystemMonitoringDemo.Base.Const;
using SystemMonitoringDemo.Base.Dto.MonitirDataDto;
using SystemMonitoringDemo.Base.Dto.PrometheusDto;
using SystemMonitoringDemo.Base.Enums.Promethues;
using SystemMonitoringDemo.Extensions;
using SystemMonitoringDemo.Services.IService;

namespace SystemMonitoringDemo.Services.Service
{
    /// <summary>
    /// LinuxService
    /// </summary>
    public class LinuxService : IOSService
    {
        private readonly IHttpClientExtension _httpClient;

        private readonly IConvertDataExtension _convertDataExtension;

        /// <summary>
        /// LinuxService
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="convertDataExtension"></param>
        public LinuxService(IHttpClientExtension httpClient, IConvertDataExtension convertDataExtension)
        {
            _httpClient = httpClient;
            _convertDataExtension = convertDataExtension;
        }
        #region CpuUsage
        /// <summary>
        /// 获取Cpu
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<MonitorDataDto>> GetCpuUsageAsync(MonitorDataInputDto inputDto)
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

            var system = await GetCpuUsageSystemAsync(inputDto);

            var user = await GetCpuUsageUserAsync(inputDto);

            var iowait = await GetCpuUsageIowaitAsync(inputDto);

            var total = await GetCpuUsageTotalAsync(inputDto);

            var allList = system.Concat(user).Concat(iowait).Concat(total).ToList();

            if (allList.Count() > 0)
            {
                var datas = _convertDataExtension.ConvertData(allList, nameof(PrometheusResultMetric.customization));

                return datas;
            }

            return new List<MonitorDataDto>();
        }

        /// <summary>
        /// Cpu 系统使用率
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        private async Task<List<PrometheusDataResult>> GetCpuUsageSystemAsync(MonitorDataInputDto inputDto)
        {
            var url = string.Empty;

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.LinuxGetCpuUsageSystem, ref url);

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);

            if (res is not null)
            {
                var resList = new List<PrometheusDataResult>();

                foreach (var entity in res.data.result)
                {
                    entity.metric.customization = PrometheusConsts.LINUX_CPU_SYSTEM;

                    resList.Add(entity);
                }

                return resList;
            }

            return new List<PrometheusDataResult>();

        }

        /// <summary>
        /// Cpu 用户使用率
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        private async Task<List<PrometheusDataResult>> GetCpuUsageUserAsync(MonitorDataInputDto inputDto)
        {
            var url = string.Empty;

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.LinuxGetCpuUsageUser, ref url);

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);

            if (res is not null)
            {
                var resList = new List<PrometheusDataResult>();

                foreach (var entity in res.data.result)
                {
                    entity.metric.customization = PrometheusConsts.LINUX_CPU_USER;

                    resList.Add(entity);
                }

                return resList;
            }

            return new List<PrometheusDataResult>();
        }

        /// <summary>
        /// Cpu 磁盘IO 使用率
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        private async Task<List<PrometheusDataResult>> GetCpuUsageIowaitAsync(MonitorDataInputDto inputDto)
        {
            var url = string.Empty;

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.LinuxGetCpuUsageIowait, ref url);

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);

            if (res is not null)
            {
                var resList = new List<PrometheusDataResult>();

                foreach (var entity in res.data.result)
                {
                    entity.metric.customization = PrometheusConsts.LINUX_CPU_IOWAIT;

                    resList.Add(entity);
                }

                return resList;
            }

            return new List<PrometheusDataResult>();
        }

        /// <summary>
        /// Cpu 总使用率
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        private async Task<List<PrometheusDataResult>> GetCpuUsageTotalAsync(MonitorDataInputDto inputDto)
        {
            var url = string.Empty;

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.LinuxGetCpuUsageTotal, ref url);

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);

            if (res is not null)
            {
                var resList = new List<PrometheusDataResult>();

                foreach (var entity in res.data.result)
                {
                    entity.metric.customization = PrometheusConsts.LINUX_CPU_TOTAL;

                    resList.Add(entity);
                }

                return resList;
            }

            return new List<PrometheusDataResult>();
        }

        #endregion

        /// <summary>
        /// 磁盘使用率
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<MonitorDataDto>> GetDiskUsageAsync(MonitorDataInputDto inputDto)
        {
            // node_filesystem_free_bytes{instance="192.168.1.135:9100"} / node_filesystem_size_bytes{instance="192.168.1.135:9100"}
            // {"status":"success","data":{"resultType":"matrix","result":
            // [{"metric":{"device":"/dev/nvme0n1p1","fstype":"vfat","instance":"192.168.1.135:9100","job":"node","mountpoint":"/boot/efi"},"values":[[1684118758.291,"0.9881203559306486"]]},
            // {"metric":{"device":"/dev/nvme0n1p2","fstype":"ext4","instance":"192.168.1.135:9100","job":"node","mountpoint":"/"},"values":[[1684118758.291,"0.5111413613472033"]]},
            // {"metric":{"device":"/dev/nvme0n1p2","fstype":"ext4","instance":"192.168.1.135:9100","job":"node","mountpoint":"/var/snap/firefox/common/host-hunspell"},"values":[[1684118758.291,"0.5111413613472033"],[1684118844.291,"0.5111410333958146"]]},
            // {"metric":{"device":"/dev/sda1","fstype":"ext4","instance":"192.168.1.135:9100","job":"node","mountpoint":"/home/mytek135/data"},"values":[[1684118758.291,"0.6241377475467904"]]}]}}

            var url = string.Empty;

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.LinuxGetDiskUsage, ref url);

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);

            if (res is not null)
            {
                var datas = _convertDataExtension.ConvertData(res.data.result, nameof(PrometheusResultMetric.device));

                return datas;
            }

            return new List<MonitorDataDto>();

        }

        /// <summary>
        /// 内存使用率
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<MonitorDataDto>> GetMemoryUsageAsync(MonitorDataInputDto inputDto)
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

            var url = string.Empty;

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.LinuxGetMemoryUsage, ref url);

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);

            if (res is not null)
            {
                var datas = _convertDataExtension.ConvertData(res.data.result, nameof(PrometheusResultMetric.instance));

                return datas;
            }

            return new List<MonitorDataDto>();
        }

        /// <summary>
        /// 网络带宽使用
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<MonitorDataDto>> GetNetworkBandwidthUseAsync(MonitorDataInputDto inputDto)
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

            var sentData = await GetNetworkBandwidthSentUseAsync(inputDto);

            var downloadData = await GetNetworkBandwidthReceivedUseAsync(inputDto);

            var allList = sentData.Concat(downloadData).ToList();

            if (allList.Count() > 0)
            {
                var datas = _convertDataExtension.ConvertData(allList, nameof(PrometheusResultMetric.customization));

                return datas;
            }

            return new List<MonitorDataDto>();
        }

        private async Task<List<PrometheusDataResult>> GetNetworkBandwidthSentUseAsync(MonitorDataInputDto inputDto)
        {
            var url = string.Empty;

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.LinuxGetNetworkBandwidthUseSent, ref url);

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);

            if (res is not null)
            {
                var resList = new List<PrometheusDataResult>();

                foreach (var entity in res.data.result)
                {
                    entity.metric.customization = PrometheusConsts.SENT;

                    resList.Add(entity);
                }

                return resList;
            }

            return new List<PrometheusDataResult>();
        }

        private async Task<List<PrometheusDataResult>> GetNetworkBandwidthReceivedUseAsync(MonitorDataInputDto inputDto)
        {
            var url = string.Empty;

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.LinuxGetNetworkBandwidthUseReceived, ref url);

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);

            if (res is not null)
            {
                var resList = new List<PrometheusDataResult>();

                foreach (var entity in res.data.result)
                {
                    entity.metric.customization = PrometheusConsts.DOWNLOAD;

                    resList.Add(entity);
                }

                return resList;
            }

            return new List<PrometheusDataResult>();
        }


        /// <summary>
        /// 网络流量
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<MonitorDataDto>> GetNetworkTrafficAsync(MonitorDataInputDto inputDto)
        {
            // increase(node_network_transmit_bytes_total{instance="192.168.1.135:9100"}[5m]) 平均出网
            // {"status":"success","data":{"resultType":"matrix","result":[
            // {"metric":{"device":"enp2s0","instance":"192.168.1.135:9100","job":"node"},"values":[
            // [1684120788.163,"1476482.1052631577"],
            // [1684120874.163,"1524663.0713576444"]
            // ]}]}}

            // increase(node_network_receive_bytes_total{instance="192.168.1.135:9100"}[5m]) 平均入网

            var sentData = await GetNetworkTrafficSentUseAsync(inputDto);

            var downloadData = await GetNetworkTrafficReceivedUseAsync(inputDto);

            var allList = sentData.Concat(downloadData).ToList();

            if (allList.Count() > 0)
            {
                var datas = _convertDataExtension.ConvertData(allList, nameof(PrometheusResultMetric.customization));

                return datas;
            }

            return new List<MonitorDataDto>();
        }

        private async Task<List<PrometheusDataResult>> GetNetworkTrafficSentUseAsync(MonitorDataInputDto inputDto)
        {
            var url = string.Empty;

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.LinuxGetNetworkTrafficTransmit, ref url);

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);

            if (res is not null)
            {
                var resList = new List<PrometheusDataResult>();

                foreach (var entity in res.data.result)
                {
                    entity.metric.customization = PrometheusConsts.SENT;

                    resList.Add(entity);
                }

                return resList;
            }

            return new List<PrometheusDataResult>();
        }

        private async Task<List<PrometheusDataResult>> GetNetworkTrafficReceivedUseAsync(MonitorDataInputDto inputDto)
        {
            var url = string.Empty;

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.LinuxGetNetworkTrafficReceive, ref url);

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);

            if (res is not null)
            {
                var resList = new List<PrometheusDataResult>();

                foreach (var entity in res.data.result)
                {
                    entity.metric.customization = PrometheusConsts.DOWNLOAD;

                    resList.Add(entity);
                }

                return resList;
            }

            return new List<PrometheusDataResult>();
        }
    }
}
