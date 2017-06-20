using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.WeixinSDK.Models
{
    /// <summary>
    /// 微信OAuth
    /// </summary>
    public class WeixinOAuthAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 授权登陆        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var target_uri = RequestUtility.UrlEncode(WeixinCfgHelper.Domain);//filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery;
            var identity = filterContext.HttpContext.User.Identity;
            if (!identity.IsAuthenticated)
            {
                var userAgent = filterContext.RequestContext.HttpContext.Request.UserAgent;
                //这里需要完整url地址，对应Controller里面的OAuthController的Callback
                var redirect_uri = string.Format("{0}/WeiXin/OAuth2Callback", WeixinCfgHelper.Domain);
                redirect_uri = RequestUtility.UrlEncode(redirect_uri);
                var scope = WeixinCfgHelper.OauthScope;
                //state保证唯一即可,可以用其他方式生成
                var state = Math.Abs(DateTime.Now.ToBinary()).ToString();
                //这里为了实现简单，将state和target_uri保存在Cache中，并设置过期时间为2分钟。可以采用其他方法!!!
                HttpContext.Current.Cache.Add(state, target_uri, null, DateTime.Now.AddMinutes(2), TimeSpan.Zero, CacheItemPriority.Normal, null);
                var weixinOAuth2Url = string.Format(ApiUrlHelper.MPApiUrl.GetCodeOauth2, WeixinCfgHelper.AppID, redirect_uri, scope, state);
                filterContext.Result = new RedirectResult(weixinOAuth2Url);
            }
            else
            {
                base.OnAuthorization(filterContext);
            }
        }
    }
}
