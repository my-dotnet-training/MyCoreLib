using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MyCoreLib.BaseSMS
{
    public class _5cSmsDemo
    {
        public static string Send(string mobilePhone, string code)
        {

            String spec = "http://m.5c.com.cn/api/send/index.php";
            // 传递的数据

            //String data = "username=zrgj&password_md5=" + MD5Security.MD5("asdf123") + "&apikey=1f39b68bbdc02553f04d55636d49cd8b&mobile=" + mobilePhone + "&content=" + HttpUtility.UrlEncode("【ICT学堂】手机动态验证码是:" + code + "") + "&encode=utf-8";
            string content = "您正在注册验证，" + code + "，请在15分钟内按页面提示提交验证码，切勿将验证码泄露于他人。";

            String data = "username=hwpx&password_md5=" + MD5Security.MD5("asdf1234a") + "&apikey=7c77e199ef9b0e31324f6034bb2a7265&mobile=" + mobilePhone + "&content=" + HttpUtility.UrlEncode(content) + "&encode=utf-8";

            // string rt = HttpPost(spec, data);
            string rt = WebRequestGet(spec + "?" + data, 120000);
            return rt;
        }
        /// <summary>
        /// 获取远程服务器ATN结果
        /// </summary>
        /// <param name="strUrl">指定URL路径地址</param>
        /// <param name="timeout">超时时间设置</param>
        /// <returns>服务器ATN结果</returns>
        public static string WebRequestGet(string strUrl, int timeout)
        {
            string strResult;
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);
                myReq.Timeout = timeout;
                //if (false)
                {
                    WebProxy proxy = new WebProxy(); //代理对象
                    proxy.Address = new Uri("http://proxycn2.huawei.com:8080"); //代理服务器地址:端口 
                    proxy.Credentials = new NetworkCredential("jwx286442", "9#EEszNj"); //用戶名,密码 
                    myReq.Proxy = proxy;
                }
                HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
                Stream myStream = HttpWResp.GetResponseStream();
                StreamReader sr = new StreamReader(myStream, Encoding.Default);
                StringBuilder strBuilder = new StringBuilder();
                while (-1 != sr.Peek())
                {
                    strBuilder.Append(sr.ReadLine());
                }
                strResult = strBuilder.ToString();
            }
            catch (Exception exp)
            {
                strResult = "错误：" + exp.Message;
            }
            return strResult;
        }
    }

    public class MD5Security
    {
        public static string MD5(string strText)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            return BitConverter.ToString(provider.ComputeHash(Encoding.UTF8.GetBytes(strText))).Replace("-", "");
        }
    }
}
