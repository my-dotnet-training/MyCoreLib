using System.IO;
using MyCoreLib.WeixinSDK.Enums;
using MyCoreLib.WeixinSDK.Helper.Http;

namespace MyCoreLib.WeixinSDK.Apis
{
    public abstract class BaseAPI
    {
        public static dynamic GetJsonAsync(string url, ContentType type = ContentType.String, bool requesterHeader = false)
        {
            return DynamicJsonSend.SendAsync(url, null, RequestMethod.GET, type, requesterHeader);
        }
        public static dynamic GetXmlAsync(string url, bool requesterHeader = false)
        {
            return DynamicXmlSend.SendAsync(url, null, RequestMethod.GET, requesterHeader);
        }
        public static Stream GetStreamAsync(string url, bool requesterHeader = false)
        {
            return HttpClientHelper.HttpClientGetStreamAsync(url, requesterHeader);
        }
        public static dynamic PostJsonAsync(string url, object content, ContentType type = ContentType.String)
        {
            return DynamicJsonSend.SendAsync(url, content, RequestMethod.POST, type);
        }
        public static dynamic PostXmlAsync(string url, object content)
        {
            return DynamicXmlSend.SendAsync(url, content, RequestMethod.POST);
        }
    }
}
