namespace SystemMonitoringDemo.Base.Dto.PrometheusDto
{
    public class PrometheusDataDto
    {
        public string status { get; set; }

        public PrometheusData data { get; set; }
    }

    public class PrometheusData
    {
        public string resultType { get; set; }

        public List<PrometheusDataResult> result { get; set; }
    }

    public class PrometheusDataResult
    {
        public PrometheusResultMetric metric { get; set; }

        public List<object[]> values { get; set; }
    }

    public class PrometheusResultMetric
    {
        public string? mode { get; set; }

        public string? instance { get; set; }

        public string? device { get; set; }

        public string? fstype { get; set; }

        public string? job { get; set; }

        public string? mountpoint { get; set; }

        public string? volume { get; set; }

        public string? nic { get; set; }
    }
}
