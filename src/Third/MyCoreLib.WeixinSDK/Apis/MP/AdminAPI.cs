using System.IO;
using MyCoreLib.WeixinSDK.Helper;
using MyCoreLib.WeixinSDK.Helper.Http;

namespace MyCoreLib.WeixinSDK.Apis.MP
{
    public class AdminAPI : BaseAPI
    {
        /// <summary>
        /// 第四步：拉取用户信息(需scope为 snsapi_userinfo)
        /// </summary>
        /// <param name="accessToekn">网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同</param>
        /// <param name="openId">用户的唯一标识</param>
        /// <param name="lang">返回国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语</param>
        /// <returns>
        /// 正常情况下，微信会返回下述JSON数据包给公众号：
        /// {
        ///     "subscribe": 1, 
        ///     "openid": "o6_bmjrPTlm6_2sgVt7hMZOPfL2M", 
        ///     "nickname": "Band", 
        ///     "sex": 1, 
        ///     "language": "zh_CN", 
        ///     "city": "广州", 
        ///     "province": "广东", 
        ///     "country": "中国", 
        ///     "headimgurl":    "http://wx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/0", 
        ///    "subscribe_time": 1382694957,
        ///    "unionid": " o6_bmasdasdsad6_2sgVt7hMZOPfL"
        ///    "remark": "",
        ///    "groupid": 0
        /// }
        ///
        ///错误时微信会返回JSON数据包如下（示例为openid无效）:
        ///{"errcode":40003,"errmsg":" invalid openid "}
        /// </returns>
        public static dynamic GetUserInfo(string accessToekn, string openId, string lang = "zh_CN")
        {
            var url = string.Format(ApiUrlHelper.MPApiUrl.GetUserInfo, accessToekn, openId, lang);
            return GetJsonAsync(url);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToekn"></param>
        /// <param name="openId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static dynamic GetUnionId(string accessToekn, string openId, string lang = "zh_CN")
        {
            try
            {
                dynamic _userInfo = GetUserInfo(accessToekn, openId, lang);
                return _userInfo.unionid;
            }
            catch
            {

            }
            return string.Empty;
        }
        /// <summary>
        /// 1) 创建二维码ticket
        /// </summary>
        /// <param name="accessToekn"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static dynamic CreateQRCodeTicket(string accessToekn, string jsonData)
        {
            var url = string.Format(ApiUrlHelper.MPApiUrl.CreateQRCodeTicket, accessToekn);
            return PostJsonAsync(url, jsonData, Enums.ContentType.String);
        }
        /// <summary>
        /// 2) 通过ticket换取二维码
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static Stream CreateQrcode(string ticket)
        {
            var url = string.Format(ApiUrlHelper.MPApiUrl.CreateQRCode, RequestUtility.UrlEncode(ticket));
            return GetStreamAsync(url);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToekn"></param>
        /// <param name="openId"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static dynamic GetUserList(string accessToekn, string nextId = "")
        {
            var url = string.Format(ApiUrlHelper.MPApiUrl.GetUserList, accessToekn, nextId);
            return GetJsonAsync(url);
        }
    }
}
