using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace MyCoreLib.WeixinSDK.Helper
{
    public class RequestHelper
    {
        /// <summary>
        /// 获取post过来数据
        /// </summary>
        /// <returns></returns>
        //public string ResponseMsg()
        //{
        //    Stream postStr = Request.InputStream;
        //    StreamReader sr = new StreamReader(postStr, Encoding.UTF8);
        //    string content = sr.ReadToEnd();
        //    return content;
        //}
        /// <summary>
        /// 通过url获取json格式数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        internal static string RequestMsg(string url, string method, string data = null, bool requesterHeader = false)
        {
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            if (requesterHeader)
                req.Headers.Add(GlobalContext.RequestHeader, GlobalContext.CreateApiRequestHeader());
            Stream reqStream = null;
            if (method == "POST")
            {
                byte[] byData = Encoding.Default.GetBytes(data);
                req.ContentLength = byData.Length;
                reqStream = req.GetRequestStream();
                reqStream.Write(byData, 0, byData.Length);
            }

            req.ContentType = "application/json";
            req.Method = method;
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;
            Stream inStream = res.GetResponseStream();
            StreamReader sr = new StreamReader(inStream, Encoding.Default);
            string content = sr.ReadToEnd();
            sr.Close();
            inStream.Close();
            res.Close();
            if (null != reqStream) reqStream.Close();
            return content;
        }
        /// <summary>
        /// 获取HttpWebResponse
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        internal static HttpWebResponse RequestMsg(string url, string method, string contentType)
        {
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.ContentType = contentType;
            req.Method = method;
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;
            res.Close();
            return res;
        }
        /// <summary>
        /// 推送数据
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        internal static string ResponseMsg(string postData, string url)
        {
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] body = Encoding.GetEncoding("UTF-8").GetBytes(postData);
            req.ContentLength = body.Length;

            Stream outStream = req.GetRequestStream();
            outStream.Write(body, 0, body.Length);
            outStream.Close();
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
            string content = sr.ReadToEnd();
            sr.Close();
            res.Close();
            return content;
        }

    }
}
