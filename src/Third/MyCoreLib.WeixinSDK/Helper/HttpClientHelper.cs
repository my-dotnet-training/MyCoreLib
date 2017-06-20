using System.Net.Http;
using MyCoreLib.WeixinSDK.Entiyies;
using MyCoreLib.WeixinSDK.Entiyies.Dynamic;

namespace MyCoreLib.WeixinSDK.Helper
{
    public class HttpClientHelper
    {
        public static StringContent CreateStringContent(string param)
        {
            return new StringContent(param);
        }
        public static dynamic HttpClientGetAsync(string url, bool requesterHeader = false)
        {
            var client = new HttpClient();
            if (requesterHeader)
                client.DefaultRequestHeaders.Add(GlobalContext.RequestHeader, GlobalContext.CreateApiRequestHeader());
            var result = client.GetAsync(url).Result;
            if (!result.IsSuccessStatusCode) return string.Empty;
            return DynamicJson.Parse(result.Content.ReadAsStringAsync().Result);
        }

        public static dynamic HttpClientPostAsync(string url, HttpContent content)
        {
            var client = new HttpClient();
            var result = client.PostAsync(url, content).Result;
            if (!result.IsSuccessStatusCode) return string.Empty;
            return DynamicJson.Parse(result.Content.ReadAsStringAsync().Result);
        }

    }
}
