using System;
using System.IO;
using System.Net.Http;
using MyCoreLib.WeixinSDK.Entiyies;
using MyCoreLib.WeixinSDK.Entiyies.Dynamic;
using MyCoreLib.WeixinSDK.Enums;

namespace MyCoreLib.WeixinSDK.Helper.Http
{
    /// <summary>
    /// HttpClient实现http请求
    /// </summary>
    internal static class HttpClientHelper
    {
        public static StringContent CreateStringContent(string param)
        {
            return new StringContent(param);
        }
        public static string HttpClientGetStringAsync(string url, bool requesterHeader = false)
        {
            var client = CreateHttpClient();
            if (requesterHeader)
                client.DefaultRequestHeaders.Add(GlobalContext.RequestHeader, GlobalContext.CreateApiRequestHeader());
            var result = client.GetStringAsync(url).Result;
            return result;
        }
        public static Stream HttpClientGetStreamAsync(string url, bool requesterHeader = false)
        {
            var client = CreateHttpClient();
            if (requesterHeader)
                client.DefaultRequestHeaders.Add(GlobalContext.RequestHeader, GlobalContext.CreateApiRequestHeader());
            var result = client.GetStreamAsync(url).Result;
            return result;
        }
        public static byte[] HttpClientGetByteArrayAsync(string url, bool requesterHeader = false)
        {
            var client = CreateHttpClient();
            if (requesterHeader)
                client.DefaultRequestHeaders.Add(GlobalContext.RequestHeader, GlobalContext.CreateApiRequestHeader());
            var result = client.GetByteArrayAsync(url).Result;
            return result;
        }
        public static HttpContent HttpClientGetContentAsync(string url, bool requesterHeader = false)
        {
            var client = CreateHttpClient();
            if (requesterHeader)
                client.DefaultRequestHeaders.Add(GlobalContext.RequestHeader, GlobalContext.CreateApiRequestHeader());
            var result = client.GetAsync(url).Result;
            return result.Content;
        }
        public static dynamic HttpClientGetJsonAsync(string url, ContentType type = ContentType.String, bool requesterHeader = false)
        {
            if (type == ContentType.Stream)
                return DynamicJson.Parse(HttpClientGetStreamAsync(url, requesterHeader));
            else if (type == ContentType.ByteArray)
            {
                byte[] _data = HttpClientGetByteArrayAsync(url, requesterHeader);
                return DynamicJson.Parse(_data, 0, _data.Length);
            }
            else if (type == ContentType.String)
                return DynamicJson.Parse(HttpClientGetStringAsync(url, requesterHeader));
            else
                return HttpClientGetContentAsync(url, requesterHeader);
        }

        public static string HttpClientPostStringAsync(string url, string content)
        {
            var client = CreateHttpClient();
            var result = client.PostAsync(url, new StringContent(content)).Result;
            if (!result.IsSuccessStatusCode) return string.Empty;
            return result.Content.ReadAsStringAsync().Result;
        }
        public static Stream HttpClientPostStreamAsync(string url, Stream content)
        {
            var client = CreateHttpClient();
            var result = client.PostAsync(url, new StreamContent(content)).Result;
            if (!result.IsSuccessStatusCode) return null;
            return result.Content.ReadAsStreamAsync().Result;
        }
        public static byte[] HttpClientPostByteArrayAsync(string url, byte[] content)
        {
            var client = CreateHttpClient();
            var result = client.PostAsync(url, new ByteArrayContent(content)).Result;
            if (!result.IsSuccessStatusCode) return null;
            return result.Content.ReadAsByteArrayAsync().Result;
        }

        public static dynamic HttpClientPostJsonAsync(string url, object content, ContentType type = ContentType.String)
        {
            if (type == ContentType.Stream)
                return DynamicJson.Parse(HttpClientPostStreamAsync(url, (content as Stream)));
            else if (type == ContentType.ByteArray)
            {
                byte[] _data = HttpClientPostByteArrayAsync(url, (content as byte[]));
                return DynamicJson.Parse(_data, 0, _data.Length);
            }
            else if (type == ContentType.String)
                return DynamicJson.Parse(HttpClientPostStringAsync(url, content.ToString()));
            else
                return null;
        }

        /// <summary>
        /// 默认10秒
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static HttpClient CreateHttpClient(int hours = 0, int minutes = 0, int seconds = GlobalContext.TIME_OUT)
        {
            HttpClient client = new HttpClient();
            TimeSpan timeSpan = new TimeSpan(hours, minutes, seconds);
            client.Timeout = timeSpan;
            return client;
        }
    }
}
