namespace SystemMonitoringDemo.Base.Dto.MonitirDataDto
{
    /// <summary>
    /// 坐标轴数据
    /// </summary>
    public class MonitorAxisDataDto
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
}
