using System.Threading;
using MyCoreLib.WeixinSDK.Entiyies;
using MyCoreLib.WeixinSDK.Helper;
using MyCoreLib.WeixinSDK.Exceptions;

namespace MyCoreLib.WeixinSDK.Apis.Pay.Business
{
    public class MicroPay
    {
        /// <summary>
        /// 刷卡支付完整业务流程逻辑
        /// </summary>
        /// <param name="body">商品描述</param>
        /// <param name="total_fee">总金额</param>
        /// <param name="auth_code">支付授权码</param>
        /// <returns>刷卡支付结果</returns>
        public static string Run(string body, string total_fee, string auth_code)
        {
            WxPayData data = new WxPayData();
            data.SetValue("auth_code", auth_code);//授权码
            data.SetValue("body", body);//商品描述
            data.SetValue("total_fee", int.Parse(total_fee));//总金额
            data.SetValue("out_trade_no", CommonHelper.CreateOutTradeNo());//产生随机的商户订单号

            WxPayData result = WxPayAPI2.Micropay(data, 10); //提交被扫支付，接收返回结果

            //如果提交被扫支付接口调用失败，则抛异常
            if (!result.IsSet("return_code") || result.GetValue("return_code").ToString() == "FAIL")
            {
                string returnMsg = result.IsSet("return_msg") ? result.GetValue("return_msg").ToString() : "";
                throw new WxPayException("Micropay API interface call failure, return_msg : " + returnMsg);
            }
            //签名验证
            result.CheckSign();
            //刷卡支付直接成功
            if (result.GetValue("return_code").ToString() == "SUCCESS" &&
                result.GetValue("result_code").ToString() == "SUCCESS")
                return result.ToPrintStr();

            /******************************************************************
             * 剩下的都是接口调用成功，业务失败的情况
             * ****************************************************************/
            //1）业务结果明确失败
            if (result.GetValue("err_code").ToString() != "USERPAYING" &&
            result.GetValue("err_code").ToString() != "SYSTEMERROR")
                return result.ToPrintStr();

            //2）不能确定是否失败，需查单
            //用商户订单号去查单
            string out_trade_no = data.GetValue("out_trade_no").ToString();

            //确认支付是否成功,每隔一段时间查询一次订单，共查询10次
            int queryTimes = 10;//查询次数计数器
            while (queryTimes-- > 0)
            {
                int succResult = 0;//查询结果
                WxPayData queryResult = Query(out_trade_no, out succResult);
                //如果需要继续查询，则等待2s后继续
                if (succResult == 2)
                {
                    Thread.Sleep(2000);
                    continue;
                }
                //查询成功,返回订单查询接口返回的数据
                else if (succResult == 1)
                    return queryResult.ToPrintStr();
                //订单交易失败，直接返回刷卡支付接口返回的结果，失败原因会在err_code中描述
                else
                    return result.ToPrintStr();
            }

            if (!Cancel(out_trade_no))
                throw new WxPayException("Reverse order failure！");

            return result.ToPrintStr();
        }
        
        /// <summary>
        /// 查询订单情况
        /// </summary>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="succCode">查询订单结果：0表示订单不成功，1表示订单成功，2表示继续查询</param>
        /// <returns>订单查询接口返回的数据，参见协议接口</returns>
        public static WxPayData Query(string out_trade_no, out int succCode)
        {
            WxPayData queryOrderInput = new WxPayData();
            queryOrderInput.SetValue("out_trade_no", out_trade_no);
            WxPayData result = WxPayAPI2.OrderQuery(queryOrderInput);

            if (result.GetValue("return_code").ToString() == "SUCCESS"
                && result.GetValue("result_code").ToString() == "SUCCESS")
            {
                //支付成功
                if (result.GetValue("trade_state").ToString() == "SUCCESS")
                {
                    succCode = 1;
                    return result;
                }
                //用户支付中，需要继续查询
                else if (result.GetValue("trade_state").ToString() == "USERPAYING")
                {
                    succCode = 2;
                    return result;
                }
            }

            //如果返回错误码为“此交易订单号不存在”则直接认定失败
            if (result.GetValue("err_code").ToString() == "ORDERNOTEXIST")
                succCode = 0;
            else //如果是系统错误，则后续继续
                succCode = 2;
            return result;
        }

        /// <summary>
        /// 撤销订单，如果失败会重复调用10次
        /// </summary>
        /// <param name="out_trade_no">商户订单号</param>
        /// <param name="depth">调用次数，这里用递归深度表示</param>
        /// <returns>false表示撤销失败，true表示撤销成功</returns>
        public static bool Cancel(string out_trade_no, int depth = 0)
        {
            if (depth > 10)
                return false;

            WxPayData reverseInput = new WxPayData();
            reverseInput.SetValue("out_trade_no", out_trade_no);
            WxPayData result = WxPayAPI2.Reverse(reverseInput);

            //接口调用失败
            if (result.GetValue("return_code").ToString() != "SUCCESS")
                return false;

            //如果结果为success且不需要重新调用撤销，则表示撤销成功
            if (result.GetValue("result_code").ToString() != "SUCCESS" && result.GetValue("recall").ToString() == "N")
                return true;
            else if (result.GetValue("recall").ToString() == "Y")
                return Cancel(out_trade_no, ++depth);
            return false;
        }
    }
}