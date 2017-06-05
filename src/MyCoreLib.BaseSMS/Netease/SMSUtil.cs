
namespace MyCoreLib.BaseSMS.Netease
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Weipan.Entity;
    using Weipan.SMS.Models;

    internal static class SMSUtil
    {
        private static DateTime s_baseTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static Regex s_phoneRegex;

        internal static bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            const int NumDigits = 11;
            if (phone.Length != NumDigits)
                return false;

            // Create the regular expression on demand
            Interlocked.CompareExchange(ref s_phoneRegex, new Regex(@"^1\d{10}$", RegexOptions.Compiled | RegexOptions.Singleline), null);
            return s_phoneRegex.IsMatch(phone);
        }

        /// <summary>
        /// 当前UTC时间戳，从1970年1月1日0点0 分0 秒开始到现在的秒数(String)
        /// </summary>
        internal static long GetTimestamp()
        {
            return (long)(DateTime.UtcNow - s_baseTime).TotalSeconds;
        }

        /// <summary>
        /// SHA1(AppSecret + Nonce + CurTime),三个参数拼接的字符串，进行SHA1哈希计算，转化成16进制字符(String，小写)
        /// </summary>
        internal static string ComputeChecksum(INetEaseSMSConfiguration config, string nonce, long timestamp)
        {
            string s = string.Concat(config.SMSAppSecret, nonce, timestamp.ToString());
            return s.SHA1();
        }

        /// <summary>
        /// Post data to the specified location.
        /// </summary>
        internal static async Task<T> PostAsync<T>(INetEaseSMSConfiguration config, string url, string content, int timeoutSeconds = 30)
            where T : GenericSMSResponse
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            if (timeoutSeconds != -1)
            {
                request.Timeout = timeoutSeconds * 1000;
            }
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            // Add customized headers
            string nonce = Guid.NewGuid().ToString("N");
            long timestamp = GetTimestamp();
            request.Headers["AppKey"] = config.SMSAppKey;
            request.Headers["Nonce"] = nonce;
            request.Headers["CurTime"] = timestamp.ToString();
            request.Headers["CheckSum"] = ComputeChecksum(config, nonce, timestamp);

            // Post data
            byte[] data = System.Text.Encoding.UTF8.GetBytes(content);
            request.ContentLength = data.Length;
            using (var reqStream = await request.GetRequestStreamAsync().ConfigureAwait(false))
            {
                await reqStream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
            }

            // Read server response
            using (var response = (HttpWebResponse)(await request.GetResponseAsync().ConfigureAwait(false)))
            {
                using (var stream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        string s = sr.ReadToEnd().Trim();
                        return s.FromJsonString<T>();
                    }
                }
            }
        }
    }
}
