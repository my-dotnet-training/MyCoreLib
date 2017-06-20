using System;
using System.Collections.Generic;
using MyCoreLib.WeixinSDK.Entiyies;
using MyCoreLib.WeixinSDK.Helper;

namespace MyCoreLib.WeixinSDK.Apis.Pay.Business
{
    public class NativePay
    {
        /// <summary>
        /// 生成扫描支付模式一URL
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>模式一URL</returns>
        public static string GetPrePayUrl(string productId)
        {
            WxPayData data = new WxPayData();
            data.SetValue("appid", GlobalContext.AppID);//公众帐号id
            data.SetValue("mch_id", GlobalContext.MCHId);//商户号
            data.SetValue("time_stamp", CommonHelper.CreateTimestamp());//时间戳
            data.SetValue("nonce_str", CommonHelper.CreateNonceStr());//随机字符串
            data.SetValue("product_id", productId);//商品ID
            data.SetValue("sign", data.MakeSign());//签名
            string str = ToUrlParams(data.GetValues());//转换为URL串
            string url = "weixin://wxpay/bizpayurl?" + str;
            return url;
        }

        /// <summary>
        /// 生成直接支付url，支付url有效期为2小时,模式二
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>模式二URL</returns>
        public static string GetPayUrl(string productId)
        {
            WxPayData data = new WxPayData();
            data.SetValue("body", "test");//商品描述
            data.SetValue("attach", "test");//附加数据
            data.SetValue("out_trade_no", CommonHelper.CreateOutTradeNo());//随机字符串
            data.SetValue("total_fee", 1);//总金额
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
            data.SetValue("goods_tag", "jjj");//商品标记
            data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("product_id", productId);//商品ID
            WxPayData result = WxPayAPI2.UnifiedOrder(data);//调用统一下单接口
            string url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接
            return url;
        }

        /// <summary>
        /// 参数数组转换为url格式
        /// </summary>
        /// <param name="map">参数名与参数值的映射表</param>
        /// <returns>URL字符串</returns>
        private static string ToUrlParams(SortedDictionary<string, object> map)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in map)
            {
                buff += pair.Key + "=" + pair.Value + "&";
            }
            buff = buff.Trim('&');
            return buff;
        }
    }
}