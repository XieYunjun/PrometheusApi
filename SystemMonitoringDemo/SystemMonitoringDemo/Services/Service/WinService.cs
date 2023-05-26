using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Runtime.Intrinsics.X86;
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
    /// WinService
    /// </summary>
    public class WinService : IOSService
    {
        private readonly IHttpClientExtension _httpClient;

        private readonly IConvertDataExtension _convertDataExtension;

        /// <summary>
        /// WinService
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="convertDataExtension"></param>
        public WinService(IHttpClientExtension httpClient, IConvertDataExtension convertDataExtension)
        {
            _httpClient = httpClient;
            _convertDataExtension = convertDataExtension;
        }

        /// <summary>
        /// Cpu使用率
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<MonitorDataDto>> GetCpuUsageAsync(MonitorDataInputDto inputDto)
        {
            // query_range?quety=
            // 100 - (avg by(instance)(irate(windows_cpu_time_total{instance="192.168.1.138:9182",mode="idle"}[5m]))*100)
            // {"status":"success","data":{"resultType":"matrix","result":[
            // {"metric":{"instance":"192.168.1.138:9182"},"values":[
            // [1684195789.088,"4.334610259826789"],
            // [1684195803.088,"7.837809125449951"],
            // [1684195817.088,"3.6460069675956674"]
            // ]}]}}
            var url = string.Empty;

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.WinGetCpuUsage, ref url);

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);

            if (res is not null)
            {
                var datas = _convertDataExtension.ConvertData(res.data.result, nameof(PrometheusResultMetric.mode));

                return datas;
            }

            return new List<MonitorDataDto>();
        }

        /// <summary>
        /// 磁盘使用率
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<MonitorDataDto>> GetDiskUsageAsync(MonitorDataInputDto inputDto)
        {
            // query_range?query=
            // rate(windows_logical_disk_split_ios_total{instance="192.168.1.138:9182"}[60m])

            // {"status":"success","data":{"resultType":"matrix","result":[
            // {"metric":{"instance":"192.168.1.138:9182","job":"x86-win","volume":"C:"},"values":[[1684196965.960,"0.1090654900610934"]]},
            // {"metric":{"instance":"192.168.1.138:9182","job":"x86-win","volume":"D:"},"values":[[1684196965.960,"0.005578797445580225"]]},
            // {"metric":{"instance":"192.168.1.138:9182","job":"x86-win","volume":"E:"},"values":[[1684196965.960,"0.0005578797445580225"]]},
            // {"metric":{"instance":"192.168.1.138:9182","job":"x86-win","volume":"F:"},"values":[[1684196965.960,"0.6410038264971678"]]}]}}
            var url = string.Empty;

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.WinGetDiskUsage, ref url);

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);

            if (res is not null)
            {
                var datas = _convertDataExtension.ConvertData(res.data.result, nameof(PrometheusResultMetric.volume));

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
            // 虚拟内存使用率 query_range?quety=
            // (windows_os_virtual_memory_bytes{instance = "192.168.1.138:9182"} - windows_os_virtual_memory_free_bytes{instance = "192.168.1.138:9182"}) / windows_os_virtual_memory_bytes{instance = "192.168.1.138:9182"}
            // {"status":"success","data":{"resultType":"matrix","result":[
            // {"metric":{"instance":"192.168.1.138:9182","job":"x86-win"},"values":[
            // [1684196295.713,"0.658931309416835"],
            // [1684196309.713,"0.6595236727923468"],
            // [1684196323.713,"0.6582989461608247"],
            // [1684196337.713,"0.6585498997679343"]
            // ]}]}}

            // 物理内存使用率 query_range?query=
            // (windows_cs_physical_memory_bytes{instance = "192.168.1.138:9182"} - windows_os_physical_memory_free_bytes{instance = "192.168.1.138:9182"}) / windows_cs_physical_memory_bytes{instance = "192.168.1.138:9182"} 
            // {"status":"success","data":{"resultType":"matrix","result":[
            // {"metric":{"instance":"192.168.1.138:9182","job":"x86-win"},"values":[
            // [1684196655.704,"0.6346970841978536"],
            // [1684196669.704,"0.63469326428631"],
            // [1684196683.704,"0.6344586580523417"]
            // ]}]}}


            var virtualData = await GetVirtualMemoryUsageAsync(inputDto);

            var physicalData = await GetPhysicalMemoryUsageAsync(inputDto);

            var allList = virtualData.Concat(physicalData).ToList();

            if (allList.Count() > 0)
            {
                var datas = _convertDataExtension.ConvertData(allList, nameof(PrometheusResultMetric.customization));

                return datas;
            }

            return new List<MonitorDataDto>();
        }

        /// <summary>
        /// 虚拟内存使用率
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        private async Task<List<PrometheusDataResult>> GetVirtualMemoryUsageAsync(MonitorDataInputDto inputDto)
        {
            var url = string.Empty;

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.WinGetVirtualMemoryUsage, ref url);

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);

            if (res is not null)
            {
                var resList = new List<PrometheusDataResult>();

                foreach (var entity in res.data.result)
                {
                    entity.metric.customization = PrometheusConsts.VIRTUAL;

                    resList.Add(entity);
                }

                return resList;
            }

            return new List<PrometheusDataResult>();
        }

        /// <summary>
        /// 物理内存使用率
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        private async Task<List<PrometheusDataResult>> GetPhysicalMemoryUsageAsync(MonitorDataInputDto inputDto)
        {
            var url = string.Empty;

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.WinGetPhysicalMemoryUsage, ref url);

            var res = await _httpClient.GetAsync<PrometheusDataDto>(url, dic);

            if (res is not null)
            {
                var resList = new List<PrometheusDataResult>();

                foreach (var entity in res.data.result)
                {
                    entity.metric.customization = PrometheusConsts.PHYSICAL;

                    resList.Add(entity);
                }

                return resList;
            }

            return new List<PrometheusDataResult>();
        }


        /// <summary>
        /// 网络带宽使用
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<MonitorDataDto>> GetNetworkBandwidthUseAsync(MonitorDataInputDto inputDto)
        {
            // 平均出网 query_range?quety=
            // rate(windows_net_bytes_sent_total{instance="192.168.1.138:9182"}[60m])
            // {"status":"success","data":{"resultType":"matrix","result":[
            // {"metric":{"instance":"192.168.1.138:9182","job":"x86-win","nic":"Broadcom_NetXtreme_Gigabit_Ethernet"},"values":[
            // [1684197945.649,"301.86150367991115"],[1684197959.649,"300.2332222127072"]
            // ]}]}}

            // 平均入网
            // rate(windows_net_bytes_received_total{instance="192.168.1.138:9182"}[60m])

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

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.WinGetNetworkBandwidthSentUse, ref url);

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

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.WinGetNetworkBandwidthReceivedUse, ref url);

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
            // 下载流量 query_range?quety=

            // sum(increase(windows_net_bytes_received_total{instance="192.168.1.138:9182"}[1h])) by (instance)
            // {"status":"success","data":{"resultType":"matrix","result":[
            // {"metric":{"instance":"192.168.1.138:9182"},"values":[
            // [1684198291.238,"4030714.8075303617"],
            // [1684198305.238,"4052441.050791452"]
            // ]}]}}

            // 上传流量 query_range?quety=
            // sum(increase(windows_net_bytes_sent_total{instance="192.168.1.138:9182"}[1h])) by (instance)


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

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.WinGetNetworkTrafficSent, ref url);

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

            var dic = _convertDataExtension.GetQueryDic(inputDto, QueryType.WinGetNetworkTrafficReceived, ref url);

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
