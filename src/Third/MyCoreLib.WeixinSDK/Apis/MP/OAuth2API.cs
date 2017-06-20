using MyCoreLib.WeixinSDK.Helper;
using MyCoreLib.WeixinSDK.Helper.Http;

namespace MyCoreLib.WeixinSDK.Apis.MP
{
    /// <summary>
    /// 对应微信API的  "用户管理"=> "网页授权获取用户基本信息”
    /// http://mp.weixin.qq.com/wiki/17/c0f37d5704f0b64713d5d2c37b468d75.html
    /// </summary>
    public class OAuth2API : BaseAPI
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static dynamic GetCode(string appId, string redirectUri, string scope, string state)
        {
            string _url = string.Format(ApiUrlHelper.MPApiUrl.GetCodeOauth2, appId, RequestUtility.UrlEncode(redirectUri), scope, state);
            return GetJsonAsync(_url);
        }
        /// <summary>
        /// 第二步：通过code换取网页授权access_token
        /// </summary>
        /// <param name="code">第一步获取的code参数</param>
        /// <param name="appId">公众号的唯一标识</param>
        /// <param name="appSecret">公众号的appsecret</param>
        /// 正确时返回的JSON数据包如下：
        ///{
        ///"access_token":"ACCESS_TOKEN",
        ///"expires_in":7200,
        ///"refresh_token":"REFRESH_TOKEN",
        ///"openid":"OPENID",
        ///"scope":"SCOPE"
        ///}
        ///错误时微信会返回JSON数据包如下（示例为Code无效错误）:
        ///{"errcode":40029,"errmsg":"invalid code"}
        /// <returns></returns>
        public static dynamic GetAccessToken(string appId, string appSecret, string code)
        {
            string _url = string.Format(ApiUrlHelper.MPApiUrl.GetAccessTokenOauth2, appId, appSecret, code);
            return GetJsonAsync(_url);
        }
        /// <summary>
        /// 第三步：刷新access_token（如果需要）
        /// 由于access_token拥有较短的有效期，当access_token超时后，可以使用refresh_token进行刷新，refresh_token拥有较长的有效期（7天、30天、60天、90天），当refresh_token失效的后，需要用户重新授权。
        /// </summary>
        /// <param name="refreshToken">填写通过access_token获取到的refresh_token参数</param>
        /// <param name="appId">公众号的唯一标识</param>
        /// <returns>
        /// 正确时返回的JSON数据包如下：
        /// {
        ///   "access_token":"ACCESS_TOKEN",
        ///   "expires_in":7200,
        ///   "refresh_token":"REFRESH_TOKEN",
        ///  "openid":"OPENID",
        ///   "scope":"SCOPE"
        /// }
        /// 
        /// 错误时微信会返回JSON数据包如下（示例为Code无效错误）:
        ///{"errcode":40029,"errmsg":"invalid code"}
        /// </returns>
        public static dynamic RefreshAccess_token(string refreshToken, string appId)
        {
            string _url = string.Format(ApiUrlHelper.MPApiUrl.RefreshAccessTokenOauth2, appId, refreshToken);
            return GetJsonAsync(_url);
        }
        /// <summary>
        /// 第四步：拉取用户信息(需scope为 snsapi_userinfo)
        /// </summary>
        /// <param name="accessToekn">网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同</param>
        /// <param name="openId">用户的唯一标识</param>
        /// <param name="lang">返回国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语</param>
        /// <returns>
        /// 正确时返回的JSON数据包如下：
        /// {
        /// "openid":" OPENID",
        /// "nickname": NICKNAME,
        /// "sex":"1",
        ///  "province":"PROVINCE"
        /// "city":"CITY",
        /// "country":"COUNTRY",
        /// "headimgurl":    "http://wx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/46", 
        ///"privilege":[
        ///"PRIVILEGE1"
        ///"PRIVILEGE2"
        ///  ]
        ///}
        ///
        ///错误时微信会返回JSON数据包如下（示例为openid无效）:
        ///{"errcode":40003,"errmsg":" invalid openid "}
        /// </returns>
        public static dynamic GetUserInfo(string accessToekn, string openId, string lang = "zh_CN")
        {
            string _url = string.Format(ApiUrlHelper.MPApiUrl.GetUserInfoOauth2, accessToekn, openId, lang);
            return GetJsonAsync(_url);
        }
    }
}
