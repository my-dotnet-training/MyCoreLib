using System;
using MyCoreLib.WeixinSDK.Entiyies.Dynamic;
using MyCoreLib.WeixinSDK.Enums;

namespace MyCoreLib.WeixinSDK.Helper.Http
{
    public class DynamicXmlSend
    {
        /// <summary>
        /// 向需要AccessToken的API发送消息的公共方法
        /// </summary>        
        /// <param name="url"></param>
        /// <param name="data">如果是Get方式，可以为null</param>
        /// <param name="sendType"></param>
        /// <param name="requesterHeader"></param>
        /// <returns></returns>
        public static dynamic SendAsync(string url, object data = null, RequestMethod sendType = RequestMethod.GET, bool requesterHeader = false)
        {
            try
            {
                switch (sendType)
                {
                    case RequestMethod.GET:
                        return new DynamicXml(HttpClientHelper.HttpClientGetStringAsync(url, requesterHeader));
                    case RequestMethod.POST:
                        return new DynamicXml(HttpClientHelper.HttpClientPostStringAsync(url, data.ToString()));
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
