using System.Reflection;
using SystemMonitoringDemo.Base.Dto.MonitirDataDto;
using SystemMonitoringDemo.Base.Dto.PrometheusDto;

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
        List<MonitorAxisDataDto> ConvertData(List<PrometheusDataResult> results, string typeName);
    }

    public class ConvertDataExtension : IConvertDataExtension
    {
        public ConvertDataExtension()
        {

        }

        public List<MonitorAxisDataDto> ConvertData(List<PrometheusDataResult> results, string typeName)
        {
            var entities = new List<MonitorAxisDataDto>();

            foreach (var result in results)
            {
                var entity = new MonitorAxisDataDto();

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

    }
}
