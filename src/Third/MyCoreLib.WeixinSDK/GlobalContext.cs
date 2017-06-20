using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MyCoreLib.WeixinSDK.Enums;
using MyCoreLib.WeixinSDK.Helper.Http;

namespace MyCoreLib.WeixinSDK
{
    public static class GlobalContext
    {
        /// <summary>
        /// 10秒
        /// </summary>
        public const int TIME_OUT = 10;
        public static string Token { set; get; }
        public static string EncodingAESKey { set; get; }
        public static string AppID { set; get; }
        public static string AppSecret { set; get; }
        public static string AccessToken { set; get; }
        public static string WebAccessToken { set; get; }
        public static string Domain { set; get; }
        public static string APIDomain { set; get; }

        #region pay
        public static string PartnerKey { set; get; }
        public static string MCHId { set; get; }
        public static string DeviceInfo { set; get; }
        public static string SpbillCreateIp { set; get; }
        public static int Report_Levenl { set; get; }
        public static string PayNotifyUrl { set; get; }
        #endregion

        #region http request header help

        public static string RequestHeader = "Requester";
        public static string RequesterName = "LYWorld.Mobile.Web";
        public static string EncryptKey = "@&*%fn(^442o+(^TWNI8^.";
        public static byte[] RgbIVKeys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private static TimeSpan? _serviceTimeSpan = null;
        private static DateTime GetServiceTime()
        {
            try
            {
                if (!_serviceTimeSpan.HasValue)
                {
                    DateTime serviceTime = DateTime.FromOADate(GetServiceOADate());
                    _serviceTimeSpan = serviceTime - DateTime.Now;
                }

                return DateTime.Now.AddMilliseconds(_serviceTimeSpan.GetValueOrDefault(new TimeSpan(0)).TotalMilliseconds);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 创建API访问头信息
        /// </summary>
        /// <returns></returns>
        public static string CreateApiRequestHeader()
        {
            string headerStr = string.Format("{0}:{1}", RequesterName, GetServiceTime().ToOADate());
            return Encrypt(headerStr, EncryptKey);
        }

        public static double GetServiceOADate()
        {
            var url = "http://api.t.totyu.cn/api/Common/GetServiceOADate";
            return DynamicJsonSend.SendAsync(url, null, RequestMethod.GET, ContentType.String);
        }

        ///   <summary> 
        ///   DES加密字符串 
        ///   </summary> 
        ///   <param   name= "encryptString "> 待加密的字符串 </param> 
        ///   <param   name= "encryptKey "> 加密密钥,要求为8位 </param> 
        ///   <returns> 加密成功返回加密后的字符串，失败返回空 </returns> 
        private static string Encrypt(string encryptString, string encryptKey)
        {
            try
            {
                if (encryptKey.Length >= 8)
                {
                    encryptKey = encryptKey.Substring(0, 8);
                }
                else
                {
                    encryptKey = encryptKey.PadRight(8, '$');
                }
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey);
                byte[] rgbIV = RgbIVKeys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
                //54y1+FydYHnfi3OsfcTW++ESSvl/L9zJczfc4HrjoQM5NyBUfTlaN0/qhU807H/B
            }
            catch
            {
                return "";
            }
        }

        #endregion
    }
}
