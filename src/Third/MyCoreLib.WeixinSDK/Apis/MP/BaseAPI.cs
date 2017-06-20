using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyCoreLib.WeixinSDK.Helper;

namespace MyCoreLib.WeixinSDK.Apis.MP
{
    public abstract class BaseAPI
    {
        public static dynamic GetAsync(string url)
        {
            return HttpClientHelper.HttpClientGetAsync(url);
        }
        public static dynamic PostStringAsync(string url, string content)
        {
            return PostAsync(url, new StringContent(content));
        }
        public static dynamic PostByteAsync(string url, byte[] content)
        {
            return PostAsync(url, new ByteArrayContent(content));
        }
        public static dynamic PostStreamAsync(string url, Stream content)
        {
            return PostAsync(url, new StreamContent(content));
        }
        public static dynamic PostAsync(string url, HttpContent content)
        {
            return HttpClientHelper.HttpClientPostAsync(url, content);
        }
    }
}
