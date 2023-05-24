namespace SystemMonitoringDemo.Extensions
{

    public static class TimeStampExtension 
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="time">DateTime.Now: 电脑上当前时间  DateTime.UtcNow: 世界当前时间（比北京时间少8小时）</param>
        /// <returns>精确到秒</returns>
        public static string GetSecondTimeStamp(DateTime time)
        {
            TimeSpan ts = time - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="time">DateTime.Now: 电脑上当前时间  DateTime.UtcNow: 世界当前时间（比北京时间少8小时）</param>
        /// <returns>精确到毫秒</returns>
        public static string GetMilliTimeStamp(DateTime time)
        {
            TimeSpan ts = time - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        /// <summary>
        /// 时间戳转时间
        /// </summary>
        /// <param name="tm"></param>
        /// <returns></returns>
        public static DateTime TimeStampToDateTime(long tm)
        {
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);

            long iTime = long.Parse(tm.ToString() + "0000");

            TimeSpan toNow = new TimeSpan(iTime);

            return startTime.Add(toNow);
        }
    }
}
