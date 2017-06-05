
namespace MyCoreLib.BaseSMS.Netease
{
    /// <summary>
    /// Please refer to the following link for more details.
    /// http://dev.netease.im/docs?doc=server&#code状态表
    /// </summary>
    public enum SMSResultCode
    {
        /// <summary>
        /// 操作成功
        /// </summary>
        Success = 200,

        /// <summary>
        /// 被封禁
        /// </summary>
        AccountDisabled = 301,

        /// <summary>
        /// IP限制
        /// </summary>
        IPBanned = 315,

        /// <summary>
        /// 非法操作或没有权限
        /// </summary>
        InvalidOperation = 403,

        /// <summary>
        /// 对象不存在
        /// </summary>
        ObjectNotExist = 404,

        /// <summary>
        /// 验证失败(短信服务)
        /// </summary>
        ValidationFailed = 413,

        /// <summary>
        /// 参数错误
        /// </summary>
        ParameterError = 414,

        /// <summary>
        /// 频率控制
        /// </summary>
        FrequencyLimitationError = 416,

        /// <summary>
        /// 服务器内部错误
        /// </summary>
        ServerError = 500
    }
}
