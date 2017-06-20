using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.WeixinSDK.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizationFilterAttribute : ActionFilterAttribute
    { 
        //cookie检查自动登录委托
        public Action<string> CookieAction;
        //cookie检查微信自动登录委托
        public Action<string> WeixinLoginAction;

        //权限检查委托
        public Func<string, string, bool> AuthAction;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext == null)
            {
                throw new Exception("此特性只适合于Web应用程序使用！");
            }
            else
            {
                //当使用outputCache时，mvc客户端缓存有Bug需要加上本句
                HttpContext.Current.Response.Cache.SetOmitVaryStar(true);
                string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string action = filterContext.ActionDescriptor.ActionName;
                MethodInfo fun = (filterContext.ActionDescriptor as ReflectedActionDescriptor).MethodInfo;
                //当没有登录
                if (!CurrentUser.IsAuthenticated)
                {
                    //检查Cookie，自动登录
                    if (!(controller.ToLower().Equals("user") && action.ToLower().Equals("login")))
                    {
                        if (CookieAction != null)
                        {
                            if (HttpContext.Current.Request.Cookies[GlobalConst.COOKIE_ACCOUNTTOKEN] != null)
                            {
                                //回调user.CookieLogin方式试登录
                                CookieAction(HttpContext.Current.Request.Cookies[GlobalConst.COOKIE_ACCOUNTTOKEN].Value);
                            }

                        }
                    }
                    // 检查是否微信过来
                    if (!CurrentUser.IsAuthenticated && HttpContext.Current.Request.QueryString.AllKeys.Contains(GlobalConst.ACTION_WEIXINPARAMETER))
                    {
                        var _weixinUnionId = HttpContext.Current.Request.QueryString.Get(GlobalConst.ACTION_WEIXINPARAMETER);
                        if (_weixinUnionId != null && !string.IsNullOrEmpty(_weixinUnionId.ToString()))
                        {
                            if (WeixinLoginAction != null)
                                WeixinLoginAction(_weixinUnionId.ToString());
                        }
                    }
                }

                //需要登录的Action
                if (!fun.IsDefined(typeof(IgnoreLoginAttribute)))
                {
                    if (!CurrentUser.IsAuthenticated)
                    {
                        if (!(controller.ToLower().Equals("user") && action.ToLower().Equals("login")))
                        {

                            filterContext.Result = new RedirectResult(string.Format("/User/Login?returnUrl={0}", Uri.EscapeDataString(Common.GlobalFunc.Encrypt(HttpContext.Current.Request.Url.PathAndQuery, "UrlHelper"))));
                            return;
                        }

                    }

                }
                else
                {
                    return;
                }
                //需要权限判断的Action
                if (!fun.IsDefined(typeof(IgnoreAuthAttribute)))
                {
                    //已经登录判断是否有操作权限
                    if (AuthAction != null)
                    {
                        string powerAction = action;
                        //判断是否有action权限别名
                        if (fun.IsDefined(typeof(PowerActionAttribute)))
                        {
                            PowerActionAttribute pa = fun.GetCustomAttribute<PowerActionAttribute>();
                            if (string.IsNullOrEmpty(pa.TypeParamName))
                            {
                                //无参数区别
                                powerAction = pa.Name;
                            }
                            else
                            {

                                if (filterContext.Controller.ValueProvider.GetValue(pa.TypeParamName) != null)
                                {
                                    string paramValue = filterContext.Controller.ValueProvider.GetValue(pa.TypeParamName).AttemptedValue;
                                    string dValue = "";
                                    if (pa.Names.TryGetValue(paramValue, out dValue))
                                    {
                                        powerAction = dValue;
                                    }
                                }
                            }
                        }

                        if (!AuthAction(controller, powerAction))
                        {
                            throw new ExceptionBiz() { Msg = "您没有该操作的权限！" };
                        }
                    }
                }
            }
        }
    }
}
