using System.Text;
using MyCoreLib.WeixinSDK.Helper;

namespace MyCoreLib.WeixinSDK.Apis.MP
{
    /// <summary>
    /// 发送(主动)客服消息
    /// </summary>
    public class ReplayActiveMessageAPI : BaseAPI
    {
        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="access_token">调用接口凭证</param>
        /// <param name="touser">普通用户openid</param>
        /// <param name="content">文本消息内容</param>
        /// <param name="kf_account">完整客服账号，格式为：账号前缀@公众号微信号</param>
        /// <returns></returns>
        public static bool RepayText(string access_token, string touser, string content, string kf_account = null)
        {
            var builder = new StringBuilder();
            builder.Append("{")
                .Append('"' + "touser" + '"' + ":").Append('"' + touser + '"').Append(",")
                .Append('"' + "msgtype" + '"' + ":").Append('"' + "text" + '"').Append(",")
                .Append('"' + "text" + '"' + ":")
                .Append("{")
                .Append('"' + "content" + '"' + ":").Append('"' + content + '"')
                .Append("}");
            if (!string.IsNullOrEmpty(kf_account))
            {
                builder.Append(",");
                builder.Append('"' + "customservice" + '"' + ":")
                       .Append("{")
                       .Append('"' + "kfaccount" + '"' + ":").Append('"' + kf_account + '"')
                       .Append("}");
            }
            builder.Append("}");
            string _url = string.Format(ApiUrlHelper.MPApiUrl.RepayActiveText, access_token);
            var _result = PostJsonAsync(_url, builder.ToString());
            if (_result == null || _result.errcode > 0)
                return false;
            else if(_result.errcode==0)

                return true;
            return _result.Result.IsSuccessStatusCode;
        }
    }
}
