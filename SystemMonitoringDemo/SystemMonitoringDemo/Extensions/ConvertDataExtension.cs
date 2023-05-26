using Microsoft.OpenApi.Models;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using SystemMonitoringDemo.Base.Const;
using SystemMonitoringDemo.Base.Dto.MonitirDataDto;
using SystemMonitoringDemo.Base.Dto.PrometheusDto;
using SystemMonitoringDemo.Base.Enums.Promethues;

namespace SystemMonitoringDemo.Extensions
{
    /// <summary>
    /// IConvertDataExtension
    /// </summary>
    public interface IConvertDataExtension
    {
        /// <summary>
        /// 转换数据
        /// </summary>
        /// <param name="results"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        List<MonitorDataDto> ConvertData(List<PrometheusDataResult>? results, string typeName);

        /// <summary>
        /// InputDto属性数据处理
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        MonitorDataInputDto ConvertMonitorInputDto(MonitorDataInputDto inputDto);

        /// <summary>
        /// 获取查询时间段
        /// </summary>
        /// <param name="timeCount"></param>
        /// <param name="type"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        string GetTimeStr(int timeCount, TimeType type, out int minutes);

        /// <summary>
        /// 获取查询Dic
        /// </summary>
        /// <param name="inputDto"></param>
        /// <param name="enum"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        Dictionary<string, string> GetQueryDic(MonitorDataInputDto inputDto, QueryType @enum, ref string url);

    }

    /// <summary>
    /// ConvertDataExtension
    /// </summary>
    public class ConvertDataExtension : IConvertDataExtension
    {
        /// <summary>
        /// ConvertDataExtension
        /// </summary>
        public ConvertDataExtension()
        {

        }

        /// <summary>
        /// 转换数据
        /// </summary>
        /// <param name="results"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public List<MonitorDataDto> ConvertData(List<PrometheusDataResult>? results, string typeName)
        {
            var entities = new List<MonitorDataDto>();

            if (results is null)
            {
                return entities;
            }

            foreach (var result in results)
            {
                var entity = new MonitorDataDto();

                if (this.ContainProperty(result.metric, typeName, out string? name))
                {
                    entity.TypeName = name;
                }

                var tmData = this.GetValue(result.values);

                entity.Time = tmData.Item1;

                entity.Data = tmData.Item2;

                entity.Max = tmData.Item2.Max();

                entity.Min = tmData.Item2.Min();

                entity.Avg = tmData.Item2.Average();

                entity.Current = tmData.Item2.Last();

                entities.Add(entity);
            }

            return entities;
        }

        /// <summary>
        /// 获取时间数据
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public ValueTuple<List<string>, List<decimal>> GetValue(List<object[]>? values)
        {
            var tms = new List<string>();

            var datas = new List<decimal>();

            if (values is null)
            {
                return new(tms, datas);
            }

            foreach (var obj in values)
            {
                var tm = obj[0].ToString();

                var data = obj[1].ToString();

                if (!string.IsNullOrEmpty(tm) && !string.IsNullOrEmpty(data))
                {
                    tms.Add(tm);

                    datas.Add(Convert.ToDecimal(data));
                }
            }

            return new(tms, datas);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="instace"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool ContainProperty<T>(T instace, string propertyName, out string? value)
        {
            value = string.Empty;

            if (instace != null && !string.IsNullOrEmpty(propertyName))
            {
                PropertyInfo? propertyInfo = instace.GetType().GetProperty(propertyName);

                if (propertyInfo != null && propertyInfo.GetValue(instace, null) != null)
                {
                    value = propertyInfo.GetValue(instace, null)!.ToString();

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 转换Dto
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public MonitorDataInputDto ConvertMonitorInputDto(MonitorDataInputDto inputDto)
        {
            if (inputDto.TargetSystemType == SystemType.Default)
            {
                inputDto.TargetSystemType = this.GetSystemType();
            }

            var localIp = this.GetLocalIP();

            if (string.IsNullOrEmpty(inputDto.PrometheusIpAddress))
            {
                inputDto.PrometheusIpAddress = localIp;

                if (string.IsNullOrEmpty(inputDto.PrometheusPort))
                {
                    inputDto.PrometheusPort = PrometheusConsts.PROMETHEUS_DEFAULT_PORT;
                }
            }

            if (string.IsNullOrEmpty(inputDto.TargetIpAddress))
            {
                inputDto.TargetIpAddress = localIp;

                if (string.IsNullOrEmpty(inputDto.TargetPort))
                {
                    inputDto.TargetPort = inputDto.TargetSystemType == SystemType.Windows ? PrometheusConsts.WIN_DEFAULT_PORT : PrometheusConsts.LINUX_DEFAULT_PORT;
                }
            }

            if (inputDto.Step <= decimal.Zero)
            {
                inputDto.Step = PrometheusConsts.DEFAULT_STEP;
            }

            if (inputDto.TimeCoutnt <= decimal.Zero)
            {
                inputDto.TimeCoutnt = PrometheusConsts.DEFAULT_TIMECOUNT;
            }

            return inputDto;
        }

        /// <summary>
        /// 获取本机操作系统
        /// </summary>
        /// <returns></returns>
        /// <exception cref="SystemException"></exception>
        public SystemType GetSystemType()
        {
            bool isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            bool isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            if (isWindows)
            {
                return SystemType.Windows;
            }
            else if (isLinux)
            {
                return SystemType.Linux;
            }
            else
            {
                throw new SystemException("本机操作系统暂不支持！");
            }
        }

        /// <summary>
        /// 获取本机ip
        /// </summary>
        /// <returns></returns>
        public string GetLocalIP()
        {
            string address = string.Empty;

            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());

            foreach (var ipAddress in ips)
            {
                if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    address = ipAddress.ToString();
                }
            }

            return address;
        }

        /// <summary>
        /// 获取时间段Str
        /// </summary>
        /// <param name="timeCount"></param>
        /// <param name="type"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public string GetTimeStr(int timeCount, TimeType type, out int minutes)
        {
            string? str;

            switch (type)
            {
                case TimeType.Minutes:
                    str = timeCount.ToString() + PrometheusConsts.MINUTES_STR;
                    minutes = timeCount;
                    break;
                case TimeType.Hour:
                    str = timeCount.ToString() + PrometheusConsts.HOUR_STR;
                    minutes = timeCount * 60;
                    break;
                default:
                    str = PrometheusConsts.DEFAULT_INT.ToString() + PrometheusConsts.MINUTES_STR;
                    minutes = timeCount;
                    break;
            }

            return str;
        }

        /// <summary>
        /// 获取查询Query
        /// </summary>
        public Dictionary<string, string> GetQueryDic(MonitorDataInputDto inputDto, QueryType @enum, ref string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                url = "http://" + inputDto.PrometheusIpAddress + ":" + inputDto.PrometheusPort + "/api/v1/query_range?";
            }

            var timeStr = this.GetTimeStr(inputDto.TimeCoutnt, inputDto.TimeType, out int minutes);

            var queryStr = string.Empty;

            switch (@enum)
            {
                case QueryType.WinGetCpuUsage:
                    queryStr = "(avg without(cpu)(sum(irate(windows_cpu_time_total{instance =\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\",mode !=\"idle\"}[" + timeStr + "])) by (mode)) / 16)* 100";
                    break;
                case QueryType.WinGetDiskUsage:
                    queryStr = "rate(windows_logical_disk_split_ios_total{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"}[" + timeStr + "])";
                    break;
                case QueryType.WinGetVirtualMemoryUsage:
                    queryStr = "(windows_os_virtual_memory_bytes{instance = \"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"} - windows_os_virtual_memory_free_bytes{instance = \"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"}) / windows_os_virtual_memory_bytes{instance = \"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"}";
                    break;
                case QueryType.WinGetPhysicalMemoryUsage:
                    queryStr = "(windows_cs_physical_memory_bytes{instance = \"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"} - windows_os_physical_memory_free_bytes{instance = \"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"}) / windows_cs_physical_memory_bytes{instance = \"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"}";
                    break;
                case QueryType.WinGetNetworkBandwidthSentUse:
                    queryStr = "rate(windows_net_bytes_sent_total{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"}[" + timeStr + "])";
                    break;
                case QueryType.WinGetNetworkBandwidthReceivedUse:
                    queryStr = "rate(windows_net_bytes_received_total{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"}[" + timeStr + "])";
                    break;
                case QueryType.WinGetNetworkTrafficSent:
                    queryStr = "sum(increase(windows_net_bytes_sent_total{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"}[" + timeStr + "])) by (instance)";
                    break;
                case QueryType.WinGetNetworkTrafficReceived:
                    queryStr = "sum(increase(windows_net_bytes_received_total{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"}[" + timeStr + "])) by (instance)";
                    break;
                case QueryType.LinuxGetCpuUsageSystem:
                    queryStr = "avg(rate(node_cpu_seconds_total{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\",mode=\"system\"}[" + timeStr + "])) by (instance) *100";
                    break;
                case QueryType.LinuxGetCpuUsageUser:
                    queryStr = "avg(rate(node_cpu_seconds_total{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\",mode=\"user\"}[" + timeStr + "])) by (instance) *100";
                    break;
                case QueryType.LinuxGetCpuUsageIowait:
                    queryStr = "avg(rate(node_cpu_seconds_total{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\",mode=\"iowait\"}[" + timeStr + "])) by (instance) *100";
                    break;
                case QueryType.LinuxGetCpuUsageTotal:
                    queryStr = "(1 - avg(rate(node_cpu_seconds_total{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\",mode=\"idle\"}[" + timeStr + "])) by (instance))*100";
                    break;
                case QueryType.LinuxGetDiskUsage:
                    queryStr = "node_filesystem_free_bytes{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"} / node_filesystem_size_bytes{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"}";
                    break;
                case QueryType.LinuxGetMemoryUsage:
                    queryStr = "(node_memory_MemTotal_bytes{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"}  - node_memory_MemFree_bytes{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"} ) / node_memory_MemTotal_bytes{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"}";
                    break;
                case QueryType.LinuxGetNetworkBandwidthUseSent:
                    queryStr = "irate(node_network_transmit_bytes_total{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"}[" + timeStr + "])";
                    break;
                case QueryType.LinuxGetNetworkBandwidthUseReceived:
                    queryStr = "irate(node_network_receive_bytes_total{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"}[" + timeStr + "])";
                    break;
                case QueryType.LinuxGetNetworkTrafficTransmit:
                    queryStr = "increase(node_network_transmit_bytes_total{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"}[" + timeStr + "])";
                    break;
                case QueryType.LinuxGetNetworkTrafficReceive:
                    queryStr = "increase(node_network_receive_bytes_total{instance=\"" + inputDto.TargetIpAddress + ":" + inputDto.TargetPort + "\"}[" + timeStr + "])";
                    break;
                default:
                    throw new SystemException("枚举类型错误！");
            }

            var dic = new Dictionary<string, string>
            {
                { "query", queryStr },
                { "start", TimeStampExtension.GetSecondTimeStamp(DateTime.UtcNow.AddMinutes(-minutes)) },
                { "end", TimeStampExtension.GetSecondTimeStamp(DateTime.UtcNow) },
                { "step", inputDto.Step.ToString() }
            };

            return dic;
        }

    }
}
