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
    public interface IConvertDataExtension
    {
        /// <summary>
        /// 转换数据
        /// </summary>
        /// <param name="results"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        List<MonitorDataDto> ConvertData(List<PrometheusDataResult> results, string typeName);

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
        /// <returns></returns>
        string GetTimeStr(int timeCount, TimeType type,out int minutes);
    }

    public class ConvertDataExtension : IConvertDataExtension
    {
        public ConvertDataExtension()
        {

        }

        public List<MonitorDataDto> ConvertData(List<PrometheusDataResult> results, string typeName)
        {
            var entities = new List<MonitorDataDto>();

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

        public ValueTuple<List<string>,List<decimal>> GetValue(List<object[]> values)
        {
            var tms = new List<string>();

            var datas = new List<decimal>();

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

            return new (tms, datas);
        }

        public bool ContainProperty<T>(T instace,string propertyName,out string? value)
        {
            value = string.Empty;

            if (instace != null && !string.IsNullOrEmpty(propertyName))
            {
                PropertyInfo? propertyInfo = instace.GetType().GetProperty(propertyName);

                if( propertyInfo != null && propertyInfo.GetValue(instace, null) != null)
                {
                    value = propertyInfo.GetValue(instace, null)!.ToString();

                    return true;
                }
            }
            return false;
        }


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
                inputDto.Step = PrometheusConsts.DEFAULT_INT;
            }

            if (inputDto.TimeCoutnt <= decimal.Zero)
            {
                inputDto.TimeCoutnt = PrometheusConsts.DEFAULT_INT;
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
    
        public string GetTimeStr(int timeCount, TimeType type,out int minutes)
        {
            var str = string.Empty;

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
    }
}
