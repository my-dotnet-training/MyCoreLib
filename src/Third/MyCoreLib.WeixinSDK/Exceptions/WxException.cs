using MyCoreLib.WeixinSDK.Models;
using System;
using MyCoreLib.Common.Extensions;

namespace MyCoreLib.WeixinSDK.Exceptions
{
    /// <summary>
    /// The exception will be thrown once got an error response from weixin servers.
    /// </summary>
    public class WxException : Exception
    {
        public int ErrorCode { get; private set; }
        public string ErrorMessage { get; private set; }
        public string Json { get; private set; }

        public WxException(int errcode, string errmsg = null, string json = null)
            : base("api error")
        {
            this.ErrorCode = errcode;
            this.ErrorMessage = errmsg ?? "api error";
            this.Json = json ?? new WeixinResponseModel { errcode = errcode, errmsg = this.ErrorMessage }.ToJsonString();
        }
    }
}
