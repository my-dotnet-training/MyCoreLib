
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MyCoreLib.BasePay.Payment.AliPay
{
    public class AlipayHelper
    {
        /// <summary>
        /// 前往支付宝支付
        /// </summary>
        /// <param name="orderId">订单号（必须唯一，一个订单号只能支付一次）</param>
        /// <param name="orderName">订单名称</param>
        /// <param name="money">支付金额</param>
        /// <param name="url_done">支付成功跳转url</param>
        /// <param name="url_cancel">支付中断跳转url</param>
        /// <param name="url_done_async">支付成功异步请求url</param>
        /// <returns></returns>
        public static ContentResult Pay(int userId, string orderId, string orderName, decimal money, string url_done, string url_cancel = null, string url_done_async = null)
        {
            string username = (CurrentUser.IsAuthenticated ? CurrentUser.UserName + "：" : string.Empty);
            Log4NetFactory.OperationLogger.DebugWithUser(string.Format("支付宝接口参数 - orderId: {0}, orderName: {1}, money: {2}, url_done: {3}, url_cancel: {4}", orderId, orderName, money, url_done, url_cancel), username);

            //支付宝网关地址
            string GATEWAY_NEW = "https://wappaygw.alipay.com/service/rest.htm?";

            ////////////////////////////////////////////调用授权接口alipay.wap.trade.create.direct获取授权码token////////////////////////////////////////////

            //返回格式
            string format = "xml";
            //必填，不需要修改

            //返回格式
            string v = "2.0";
            //必填，不需要修改

            //请求号
            string req_id = DateTime.Now.ToString("yyyyMMddHHmmss");
            //必填，须保证每次请求都是唯一

            //req_data详细信息

            //服务器异步通知页面路径
            string notify_url = url_done_async;
            //需http://格式的完整路径，不允许加?id=123这类自定义参数

            //页面跳转同步通知页面路径
            string call_back_url = url_done;
            //需http://格式的完整路径，不允许加?id=123这类自定义参数

            //操作中断返回地址
            string merchant_url = url_cancel;
            //用户付款中途退出返回商户的地址。需http://格式的完整路径，不允许加?id=123这类自定义参数

            //卖家支付宝帐户
            string seller_email = CustomCfgHelper.Get("SellerEmail");
            //必填

            //商户订单号
            string out_trade_no = string.Format("{0}|{1}", userId, orderId);
            //商户网站订单系统中唯一订单号，必填

            //订单名称
            string subject = orderName;
            //必填

            //付款金额
            string total_fee = money.ToString("0.00");
            //必填

            //请求业务参数详细
            string req_dataToken = "<direct_trade_create_req><notify_url>" + notify_url + "</notify_url><call_back_url>" + call_back_url + "</call_back_url><seller_account_name>" + seller_email + "</seller_account_name><out_trade_no>" + out_trade_no + "</out_trade_no><subject>" + subject + "</subject><total_fee>" + total_fee + "</total_fee><merchant_url>" + merchant_url + "</merchant_url></direct_trade_create_req>";
            //必填

            //把请求参数打包成数组
            Dictionary<string, string> sParaTempToken = new Dictionary<string, string>();
            sParaTempToken.Add("partner", Config.Partner);
            sParaTempToken.Add("_input_charset", Config.Input_charset.ToLower());
            sParaTempToken.Add("sec_id", Config.Sign_type.ToUpper());
            sParaTempToken.Add("service", "alipay.wap.trade.create.direct");
            sParaTempToken.Add("format", format);
            sParaTempToken.Add("v", v);
            sParaTempToken.Add("req_id", req_id);
            sParaTempToken.Add("req_data", req_dataToken);

            //建立请求
            string sHtmlTextToken = Submit.BuildRequest(GATEWAY_NEW, sParaTempToken);
            //URLDECODE返回的信息
            Encoding code = Encoding.GetEncoding(Config.Input_charset);
            sHtmlTextToken = HttpUtility.UrlDecode(sHtmlTextToken, code);

            //解析远程模拟提交后返回的信息
            Dictionary<string, string> dicHtmlTextToken = Submit.ParseResponse(sHtmlTextToken);

            //获取token
            string request_token = dicHtmlTextToken["request_token"];

            ////////////////////////////////////////////根据授权码token调用交易接口alipay.wap.auth.authAndExecute////////////////////////////////////////////


            //业务详细
            string req_data = "<auth_and_execute_req><request_token>" + request_token + "</request_token></auth_and_execute_req>";
            //必填

            //把请求参数打包成数组
            Dictionary<string, string> sParaTemp = new Dictionary<string, string>();
            sParaTemp.Add("partner", Config.Partner);
            sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
            sParaTemp.Add("sec_id", Config.Sign_type.ToUpper());
            sParaTemp.Add("service", "alipay.wap.auth.authAndExecute");
            sParaTemp.Add("format", format);
            sParaTemp.Add("v", v);
            sParaTemp.Add("req_data", req_data);

            //建立请求
            string sHtmlText = Submit.BuildRequest(GATEWAY_NEW, sParaTemp, "get", "确认");

            Log4NetFactory.OperationLogger.InfoWithUser("调用支付宝接口开始", username);
            return new ContentResult { Content = sHtmlText };
        }

        /// <summary>
        /// 支付宝支付完成
        /// </summary>
        /// <param name="controller">当前controller</param>
        /// <param name="failMsg">支付失败提示信息</param>
        /// <param name="failUrl">支付失败调整url</param>
        /// <param name="success">成功委托，参数为当前订单号，返回ActionResult</param>
        /// <returns></returns>
        public static ActionResult PayDone(BaseController controller, string failMsg, string failUrl, Func<int, string, ActionResult> success)
        {
            return PayDone(controller, failMsg, failUrl, (userId, orderId, alipayDno) => { return success(userId, orderId); });
        }

        /// <summary>
        /// 支付宝支付完成
        /// </summary>
        /// <param name="controller">当前controller</param>
        /// <param name="failMsg">支付失败提示信息</param>
        /// <param name="failUrl">支付失败调整url</param>
        /// <param name="success">成功委托，参数为当前订单号，返回ActionResult</param>
        /// <returns></returns>
        public static ActionResult PayDone(BaseController controller, string failMsg, string failUrl, Func<int, string, string, ActionResult> success)
        {
            Dictionary<string, string> sPara = GetRequestGet(controller);
            string username = (CurrentUser.IsAuthenticated ? CurrentUser.UserName + "：" : string.Empty);

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.VerifyReturn(sPara, controller.Request.QueryString["sign"]);

                if (verifyResult)//验证成功
                {
                    //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表

                    //商户订单号
                    string out_trade_no = controller.Request.QueryString["out_trade_no"];

                    //支付宝交易号
                    string trade_no = controller.Request.QueryString["trade_no"];

                    //交易状态
                    string result = controller.Request.QueryString["result"];

                    if (result == "success")
                    {
                        Log4NetFactory.OperationLogger.InfoWithUser("调用支付宝接口成功", username);

                        //解析用户ID和订单号
                        if (out_trade_no.Contains("|"))
                        {
                            string userIdStr = out_trade_no.Split('|')[0];
                            string orderId = out_trade_no.Split('|')[1];
                            int userId = 0;
                            if (!int.TryParse(userIdStr, out userId))
                                return new ContentResult { Content = "<script>alert('订单号格式不正确！');location.href='" + failUrl + "';</script>" };
                            else
                                return success(userId, orderId, trade_no);
                        }
                        else
                            return new ContentResult { Content = "<script>alert('订单号格式不正确！');location.href='" + failUrl + "';</script>" };
                    }
                    else
                    {
                        Log4NetFactory.OperationLogger.InfoWithUser("调用支付宝接口失败", username);
                        return new ContentResult { Content = "<script>alert('" + failMsg + "');location.href='" + failUrl + "';</script>" };
                    }
                }
                else//验证失败
                {
                    Log4NetFactory.OperationLogger.InfoWithUser("调用支付宝接口失败", username);
                    return new ContentResult { Content = "<script>alert('" + failMsg + "');location.href='" + failUrl + "';</script>" };
                }
            }
            else
            {
                Log4NetFactory.OperationLogger.InfoWithUser("调用支付宝接口失败", username);
                return new ContentResult { Content = "<script>alert('" + failMsg + "');location.href='" + failUrl + "';</script>" };
            }
        }

        /// <summary>
        /// 支付宝支付完成异步请求页面
        /// </summary>
        /// <param name="controller">当前controller</param>
        /// <param name="failMsg">支付失败提示信息</param>
        /// <param name="failUrl">支付失败调整url</param>
        /// <param name="success">成功委托，参数为用户名、订单号</param>
        /// <returns></returns>
        public static ContentResult PayDoneAsync(BaseController controller, Action<int, string> success, string failMsg = null, string failUrl = null)
        {
            return PayDoneAsync(controller, (userId, orderId, alipayDno, buyAccount) => { success(userId, orderId); });
        }

        /// <summary>
        /// 支付宝支付完成异步请求页面
        /// </summary>
        /// <param name="controller">当前controller</param>
        /// <param name="failMsg">支付失败提示信息</param>
        /// <param name="failUrl">支付失败调整url</param>
        /// <param name="success">成功委托，参数为用户名、订单号、支付宝交易号、买家帐号</param>
        /// <returns></returns>
        public static ContentResult PayDoneAsync(BaseController controller, Action<int, string, string, string> success, string failMsg = null, string failUrl = null)
        {
            string username = (CurrentUser.IsAuthenticated ? CurrentUser.UserName + "：" : string.Empty);
            Dictionary<string, string> sPara = GetRequestPost(controller);

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.VerifyNotify(sPara, controller.Request.Form["sign"]);

                if (verifyResult)//验证成功
                {
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                    //解密
                    sPara = aliNotify.Decrypt(sPara);

                    //XML解析notify_data数据
                    try
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(sPara["notify_data"]);
                        //商户订单号
                        string out_trade_no = xmlDoc.SelectSingleNode("/notify/out_trade_no").InnerText;
                        //支付宝交易号
                        string trade_no = xmlDoc.SelectSingleNode("/notify/trade_no").InnerText;
                        //支付宝买家帐号
                        string buyer_account = xmlDoc.SelectSingleNode("/notify/buyer_email").InnerText;
                        //交易状态
                        string trade_status = xmlDoc.SelectSingleNode("/notify/trade_status").InnerText;

                        //解析用户ID和订单号
                        int userId = 0;
                        string orderId = string.Empty;
                        if (out_trade_no.Contains("|"))
                        {
                            string userIdStr = out_trade_no.Split('|')[0];
                            orderId = out_trade_no.Split('|')[1];
                            if (!int.TryParse(userIdStr, out userId))
                                return new ContentResult { Content = "fail" };
                        }
                        else
                            return new ContentResult { Content = "fail" };

                        if (trade_status == "TRADE_FINISHED")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //如果有做过处理，不执行商户的业务程序

                            //注意：
                            //该种交易状态只在两种情况下出现
                            //1、开通了普通即时到账，买家付款成功后。
                            //2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。

                            Log4NetFactory.OperationLogger.InfoWithUser("调用支付宝接口成功，状态：TRADE_FINISHED", username);
                            success(userId, orderId, trade_no, buyer_account);
                            return new ContentResult { Content = "success" };  //请不要修改或删除
                        }
                        else if (trade_status == "TRADE_SUCCESS")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //如果有做过处理，不执行商户的业务程序

                            //注意：
                            //该种交易状态只在一种情况下出现——开通了高级即时到账，买家付款成功后。

                            Log4NetFactory.OperationLogger.InfoWithUser("调用支付宝接口成功，状态：TRADE_SUCCESS", username);
                            success(userId, orderId, trade_no, buyer_account);
                            return new ContentResult { Content = "success" };  //请不要修改或删除
                        }
                        else
                        {
                            Log4NetFactory.OperationLogger.InfoWithUser("调用支付宝接口失败", username);
                            return new ContentResult { Content = trade_status };
                        }
                    }
                    catch (Exception exc)
                    {
                        Log4NetFactory.OperationLogger.InfoWithUser("调用支付宝接口失败", username);
                        return new ContentResult { Content = exc.ToString() };
                    }
                }
                else//验证失败
                {
                    Log4NetFactory.OperationLogger.InfoWithUser("调用支付宝接口失败", username);
                    return new ContentResult { Content = "fail" };
                }
            }
            else
            {
                Log4NetFactory.OperationLogger.InfoWithUser("调用支付宝接口失败", username);
                return new ContentResult { Content = "无通知参数" };
            }
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public static Dictionary<string, string> GetRequestGet(BaseController controller)
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = controller.Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], controller.Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public static Dictionary<string, string> GetRequestPost(BaseController controller)
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = controller.Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], controller.Request.Form[requestItem[i]]);
            }

            return sArray;
        }
    }
}
