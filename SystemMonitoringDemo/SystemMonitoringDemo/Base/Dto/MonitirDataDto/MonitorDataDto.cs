using SystemMonitoringDemo.Base.Enums.Promethues;

namespace SystemMonitoringDemo.Base.Dto.MonitirDataDto
{
    /// <summary>
    /// 坐标轴数据
    /// </summary>
    public class MonitorDataDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string? TypeName { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal Min { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal Max { get; set; }

        /// <summary>
        /// 平均值
        /// </summary>
        public decimal Avg { get; set; }

        /// <summary>
        /// 当前值
        /// </summary>
        public decimal Current { get; set; }

        /// <summary>
        /// 时间戳 列表
        /// </summary>
        public List<string> Time { get; set; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public List<decimal> Data { get; set; }
    }

    public class MonitorHistogramDataDto
    {

    }

    public class MonitorDataInputDto
    {
        /// <summary>
        /// PrometheusIp
        /// </summary>
        public string? PrometheusIpAddress { get; set; }

        /// <summary>
        /// Prometheus端口
        /// </summary>
        public string? PrometheusPort { get; set; }

        /// <summary>
        /// 目标Ip
        /// </summary>
        public string? TargetIpAddress { get; set; }

        /// <summary>
        /// 目标端口
        /// </summary>
        public string? TargetPort { get; set; }

        /// <summary>
        /// 目标系统类型
        /// </summary>
        public SystemType TargetSystemType { get; set; }

        /// <summary>
        /// 时间长度
        /// </summary>

        public int TimeCoutnt { get; set; }

        /// <summary>
        /// 时间类型
        /// </summary>

        public TimeType TimeType { get; set; }

        /// <summary>
        /// 查询时间步长
        /// </summary>
        public int Step { get; set; }
    }
}
