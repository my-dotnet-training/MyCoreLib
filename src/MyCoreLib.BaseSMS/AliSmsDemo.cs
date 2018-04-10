
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core;
using Aliyun.Acs.Sms.Model.V20160927;
using System;
using System.Collections.Generic;

namespace MyCoreLib.BaseSMS
{
    public class AliSmsDemo
    {
        public string SendStationSmsEm(string signName, string templateCode, string recNum, string paramString)
        {
            ReturnResult rr = new ReturnResult();
            string regionId = "cn-hangzhou";
            string accessKey = "LTAIDpUs8Ms4gY7e";
            string accessSecret = "X88cjTLTfVHb5BTeuWXxoHNBuH5ss1";
            IClientProfile profile = DefaultProfile.GetProfile(regionId, accessKey, accessSecret);
            IAcsClient client = new DefaultAcsClient(profile);
            SingleSendSmsRequest request = new SingleSendSmsRequest();
            try
            {
                //管理控制台中配置的短信签名（状态必须是验证通过）
                request.SignName = signName; //"ICT学堂";
                //管理控制台中配置的审核通过的短信模板的模板CODE（状态必须是验证通过）
                request.TemplateCode = templateCode; //"SMS_26570070";
                //接收号码，多个号码可以逗号分隔 
                request.RecNum = recNum;// "15017934696";
                //短信模板中的变量；数字需要转换为字符串；个人用户每个变量长度必须小于15个字符。
                paramString = paramString.Replace('"', '\"');
                request.ParamString = paramString; //"{'customer':'番茄'}";
                SingleSendSmsResponse httpResponse = client.GetAcsResponse(request);
                if (httpResponse.Model != null && httpResponse.RequestId != null)
                {
                    rr.status = 1;
                    rr.message = "发送成功！";
                }
                else
                {
                    rr.status = 0;
                    rr.message = "发送失败！";
                }
            }
            catch (Exception e)
            {
                rr.status = -1;
                rr.message = "发送异常！";
            }
            return rr.message;
        }
    }
    [Serializable]
    public class ReturnResult
    {
        public int status { get; set; }
        public string message { get; set; }
        public string url { get; set; }
        public string id { get; set; }

        public List<object> moblerList = new List<object>();
    }
}
