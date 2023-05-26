namespace SystemMonitoringDemo.Base.Dto.PrometheusDto
{
    /// <summary>
    /// PrometheusDataDto
    /// </summary>
    public class PrometheusDataDto
    {
        /// <summary>
        /// 状态
        /// </summary>
        /// <value></value>
        public string status { get; set; } = string.Empty;

        /// <summary>
        /// data
        /// </summary>
        /// <value></value>
        public PrometheusData data { get; set; } = new PrometheusData();
    }

    /// <summary>
    /// PrometheusData
    /// </summary>
    public class PrometheusData
    {
        /// <summary>
        /// resultType
        /// </summary>
        /// <value></value>
        public string resultType { get; set; } = string.Empty;

        /// <summary>
        /// result
        /// </summary>
        /// <value></value>
        public List<PrometheusDataResult> result { get; set; } = new List<PrometheusDataResult>();
    }

    /// <summary>
    /// PrometheusDataResult
    /// </summary>
    public class PrometheusDataResult
    {
        /// <summary>
        /// metric
        /// </summary>
        /// <value></value>
        public PrometheusResultMetric metric { get; set; } = new PrometheusResultMetric();

        /// <summary>
        /// values
        /// </summary>
        /// <value></value>
        public List<object[]> values { get; set; } = new List<object[]>();
    }

    /// <summary>
    /// PrometheusResultMetric
    /// </summary>
    public class PrometheusResultMetric
    {
        /// <summary>
        /// mode
        /// </summary>
        /// <value></value>
        public string? mode { get; set; }

        /// <summary>
        /// instance
        /// </summary>
        /// <value></value>
        public string? instance { get; set; }

        /// <summary>
        /// device
        /// </summary>
        /// <value></value>
        public string? device { get; set; }

        /// <summary>
        /// fstype
        /// </summary>
        /// <value></value>
        public string? fstype { get; set; }

        /// <summary>
        /// job
        /// </summary>
        /// <value></value>
        public string? job { get; set; }

        /// <summary>
        /// mountpoint
        /// </summary>
        /// <value></value>
        public string? mountpoint { get; set; }

        /// <summary>
        /// volume
        /// </summary>
        /// <value></value>
        public string? volume { get; set; }

        /// <summary>
        /// nic
        /// </summary>
        /// <value></value>
        public string? nic { get; set; }

        /// <summary>
        /// 自定义字段
        /// </summary>
        public string? customization { get; set; }
    }
}
