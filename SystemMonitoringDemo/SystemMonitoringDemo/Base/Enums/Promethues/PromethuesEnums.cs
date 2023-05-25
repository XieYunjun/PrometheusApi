using System.ComponentModel;

namespace SystemMonitoringDemo.Base.Enums.Promethues
{
    /// <summary>
    /// Promerheus 状态错误码
    /// </summary>
    public enum ReponseErrorCode
    {
        /// <summary>
        /// 参数错误或者缺失
        /// </summary>
        [Description("参数错误或者缺失")]
        BadRequest = 404,

        /// <summary>
        /// 表达式无法执行
        /// </summary>
        [Description("表达式无法执行")]
        Unprocessable = 422,

        /// <summary>
        /// 请求超时或者被中断
        /// </summary>
        [Description("请求超时或者被中断")]
        ServiceUnavailiable = 503
    }

    /// <summary>
    /// 响应数据类型
    /// </summary>
    public enum ResponseDataType
    {
        /// <summary>
        /// 区间向量
        /// </summary>
        [Description("区间向量")]
        matrix = 0,

        /// <summary>
        /// 瞬时向量
        /// </summary>
        [Description("瞬时向量")]
        vector = 1,

        /// <summary>
        /// 标量
        /// </summary>
        [Description("标量")] 
        scalar = 2,

        /// <summary>
        /// 字符串
        /// </summary>
        [Description("字符串")]
        _string = 3
    }

    /// <summary>
    /// 时间类型
    /// </summary>
    public enum TimeType
    {
        /// <summary>
        /// 分钟
        /// </summary>
        [Description("分钟")]
        Minutes = 0,

        /// <summary>
        /// 小时
        /// </summary>
        [Description("小时")]
        Hour = 1,
    }

    /// <summary>
    /// 系统类型
    /// </summary>
    public enum SystemType
    {
        /// <summary>
        /// 默认本机系统
        /// </summary>
        [Description("默认本机系统")]
        Default = 0,

        /// <summary>
        /// Win
        /// </summary>
        [Description("Win")]
        Windows = 1,

        /// <summary>
        /// Linux
        /// </summary>
        [Description("Linux")]
        Linux = 2,
    }

}
