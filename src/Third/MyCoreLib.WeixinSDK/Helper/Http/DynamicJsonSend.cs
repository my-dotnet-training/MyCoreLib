using System;
using System.IO;
using System.Net.Http;
using MyCoreLib.WeixinSDK.Enums;

namespace MyCoreLib.WeixinSDK.Helper.Http
{
    public class DynamicJsonSend
    {
        /// <summary>
        /// 向需要AccessToken的API发送消息的公共方法
        /// </summary>
        /// <param name="accessToken">这里的AccessToken是通用接口的AccessToken，非OAuth的。如果不需要，可以为null，此时urlFormat不要提供{0}参数</param>
        /// <param name="urlFormat"></param>
        /// <param name="data">如果是Get方式，可以为null</param>
        /// <param name="sendType"></param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static dynamic SendAsync(string url, object data = null, RequestMethod sendType = RequestMethod.GET, ContentType contentType = ContentType.String, bool requesterHeader = false)
        {
            try
            {
                switch (sendType)
                {
                    case RequestMethod.GET:
                        return HttpClientHelper.HttpClientGetJsonAsync(url, contentType, requesterHeader);
                    case RequestMethod.POST:
                        return HttpClientHelper.HttpClientPostJsonAsync(url, data, contentType);
                    default:
                        throw new ArgumentOutOfRangeException("sendType");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 向需要AccessToken的API发送消息的公共方法
        /// </summary>
        /// <param name="accessToken">这里的AccessToken是通用接口的AccessToken，非OAuth的。如果不需要，可以为null，此时urlFormat不要提供{0}参数</param>
        /// <param name="urlFormat">用accessToken参数填充{0}</param>
        /// <param name="data">如果是Get方式，可以为null</param>
        /// <param name="sendType"></param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <param name="checkValidationResult"></param>
        /// <returns></returns>
        public static T SendAsync<T>(string url, object data = null, RequestMethod sendType = RequestMethod.POST, ContentType contentType = ContentType.String, bool requesterHeader = false)
        {
            try
            {
                switch (sendType)
                {
                    case RequestMethod.GET:
                        return HttpClientHelper.HttpClientGetJsonAsync(url, contentType, requesterHeader);
                    case RequestMethod.POST:
                        return HttpClientHelper.HttpClientPostJsonAsync(url, data, contentType);
                    default:
                        throw new ArgumentOutOfRangeException("sendType");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
