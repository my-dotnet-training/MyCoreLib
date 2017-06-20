using System.Linq;
using System.Text;
using MyCoreLib.WeixinSDK.Helper;

namespace MyCoreLib.WeixinSDK.Apis.MP
{
    public class BasicAPI : BaseAPI
    {
        /// <summary>
        /// 检查签名是否正确:
        /// http://mp.weixin.qq.com/wiki/index.php?title=%E6%8E%A5%E5%85%A5%E6%8C%87%E5%8D%97
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token">AccessToken</param>
        /// <returns>
        /// true: check signature success
        /// false: check failed, 非微信官方调用!
        /// </returns>
        public static bool CheckSignature(string signature, string timestamp, string nonce, string token, out string ent)
        {
            var arr = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
            var arrString = string.Join("", arr);
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }
            ent = enText.ToString();
            return signature == enText.ToString();
        }

        /// <summary>
        /// 获取AccessToken
        /// http://mp.weixin.qq.com/wiki/index.php?title=%E8%8E%B7%E5%8F%96access_token
        /// </summary>
        /// <param name="grant_type"></param>
        /// <param name="appid"></param>
        /// <param name="secrect"></param>
        /// <returns>access_toke</returns>
        public static dynamic GetAccessToken(string appid, string secrect)
        {
            var url = string.Format(ApiUrlHelper.MPApiUrl.GetAccessToken, "client_credential", appid, secrect);
            return GetJsonAsync(url);
        }
        /// <summary>
        /// 获取微信服务器IP地址
        ///http://mp.weixin.qq.com/wiki/0/2ad4b6bfd29f30f71d39616c2a0fcedc.html
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns>{"ip_list":["127.0.0.1","127.0.0.1"]}</returns>
        public static dynamic GetCallbackIP(string accessToekn)
        {
            var url = string.Format(ApiUrlHelper.MPApiUrl.GetCallbackIP, accessToekn);
            return GetJsonAsync(url);
        }
        /// <summary>
        /// 长链接转短链接接口
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>

        public static dynamic ShortUrl(string accessToekn, string jsonData)
        {
            var url = string.Format(ApiUrlHelper.MPApiUrl.ShortUrl, accessToekn);
            return PostJsonAsync(url, jsonData, Enums.ContentType.String);
        }
    }
}