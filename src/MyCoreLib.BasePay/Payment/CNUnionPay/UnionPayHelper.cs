using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ChinaPay_JY;
using System.Web.Mvc;

namespace com.totyu.LYWorld.Pay.UnionPay
{
    /// <summary>
    /// 支付版本 20070129
    /// </summary>
    public class UnionPayHelper
    {
        private static string siteRootPath = HttpContext.Current.Request.PhysicalApplicationPath; //获取网站根目录物理路径
        public UnionPayHelper()
        {

        }

        /// <summary>
        /// 订单签名函数sign
        /// </summary>
        /// <param name="MerId">商户号，长度为15个字节的数字串，由ChinaPay或清算银行分配</param>
        /// <param name="OrdId">订单号，长度为16个字节的数字串，由用户系统/网站生成，失败的订单号允许重复支付</param>
        /// <param name="TransAmt">交易金额，长度为12个字节的数字串，例如：数字串"000000001234"表示12.34元</param>
        /// <param name="CuryId">货币代码, 长度为3个字节的数字串，目前只支持人民币，取值为"156"</param>
        /// <param name="TransDate">交易日期，长度为8个字节的数字串，表示格式为：YYYYMMDD</param>
        /// <param name="TransType">交易类型，长度为4个字节的数字串，取值范围为："0001"和"0002"， 其中"0001"表示消费交易，"0002"表示退货交易</param>
        /// <returns>string CheckValue[256]  即NetPayClient根据上述输入参数生成的商户数字签名，长度为256字节的字符串</returns>
        public string getSign(string MerId, string OrdId, string TransAmt, string CuryId, string TransDate, string TransType, string KeyPath)
        {
            NetPayClientClass npc = new NetPayClientClass(); //实例NetPay签名
            //npc.setMerKeyFile("Bin/MerPrK.key");          //设置商户密钥文件地址 d:\\MerPrK.key
            npc.setMerKeyFile(KeyPath + "\\MerPrK.key");
            string strChkValue = "";                         //chinapay返回的商户数字签名
            strChkValue = npc.sign(MerId, OrdId, TransAmt, CuryId, TransDate, TransType);


            npc.signData(MerId, "");

            return strChkValue.Trim();
        }

        /// <summary>
        /// 对一段字符进行签名 signData
        /// </summary>
        /// <param name="MerId">商户号，长度为15个字节的数字串，由ChinaPay分配</param>
        /// <param name="SignMsg">用于要签名的字符串</param>
        /// <returns>String CheckValue[256]即NetPayClient根据上述输入参数生成的商户数字签名，长度为256字节的字符串</returns>
        public string signData(string MerId, string SignMsg, string KeyPath)
        {
            NetPayClientClass npc = new NetPayClientClass(); //实例NetPay签名
            //npc.setMerKeyFile("Bin/MerPrK.key");          //设置商户密钥文件地址 d:\\MerPrK.key
            npc.setMerKeyFile(KeyPath + "\\MerPrK.key");
            string strChkValueData = "";
            strChkValueData = npc.signData(MerId, SignMsg);
            return strChkValueData.Trim();
        }

        /// <summary>
        /// 验证交易应答函数check
        /// </summary>
        /// <param name="MerId">商户号，长度为15个字节的数字串，由ChinaPay分配</param>
        /// <param name="OrdId">订单号，长度为16个字节的数字串，由商户系统生成，失败的订单号允许重复支付</param>
        /// <param name="TransAmt">交易金额，长度为12个字节的数字串，例如：数字串"000000001234"表示12.34元</param>
        /// <param name="CuryId">货币代码, 长度为3个字节的数字串，目前只支持人民币，取值为"156"</param>
        /// <param name="TransDate">交易日期，长度为8个字节的数字串，表示格式为： YYYYMMDD</param>
        /// <param name="TransType">交易类型，长度为4个字节的数字串，取值范围为："0001"和"0002"， 其中"0001"表示消费交易，"0002"表示退货交易</param>
        /// <param name="OrderStatus">交易状态，长度为4个字节的数字串。详见交易状态码说明</param>
        /// <param name="CheckValue">校验值，即ChinaPay对交易应答的数字签名，长度为256字节的字符串</param>
        /// <returns>true 表示成功，即该交易应答为ChinaPay所发送，商户根据“交易状态”进行后续处理；否则表示失败，即无效应答，商户可忽略该应答</returns>
        public bool getCheck(string MerId, string OrdId, string TransAmt, string CuryId, string TransDate, string TransType, string OrderStatus, string CheckValue, string KeyPath)
        {
            NetPayClientClass npc = new NetPayClientClass(); //实例NetPay签名
            //npc.setPubKeyFile("Bin/PgPubk.key");          //设置chinapay公共密钥文件地址 d:\\PgPubk.key
            npc.setPubKeyFile(KeyPath + "\\PgPubk.key");
            string strFlag = "";
            bool bolFlag = false;
            strFlag = npc.check(MerId, OrdId, TransAmt, CuryId, TransDate, TransType, OrderStatus, CheckValue); // ChkValue 为ChinaPay返回给商户的域段内容
            if (strFlag == "0") //“0”表示验签成功
            {
                bolFlag = true;
            }
            return bolFlag;
        }
        /// <summary>
        /// 对一段字符串进行签名验证 checkData
        /// </summary>
        /// <param name="PlainData">用于数字签名的字符串</param>
        /// <param name="CheckValue">校验值，要验证的字符串的数字签名，长度为256字节的字符串</param>
        /// <returns>true 表示验证通过成功；否则表示失败</returns>
        public bool checkData(string PlainData, string CheckValue, string KeyPath)
        {
            NetPayClientClass npc = new NetPayClientClass(); //实例NetPay签名
            //npc.setPubKeyFile("Bin/PgPubk.key");          //设置chinapay公共密钥文件地址 d:\\PgPubk.key
            npc.setPubKeyFile(KeyPath + "\\PgPubk.key");
            string strFlagData = "";
            bool bolFlagData = false;
            strFlagData = npc.checkData(PlainData, CheckValue);
            if (strFlagData == "true")
            {
                bolFlagData = true;
            }
            return bolFlagData;
        }

        //支付函数
        /// <summary>
        /// 支付函数
        /// </summary>
        /// <param name="OrderID">程序 订单编号</param>
        /// <param name="TransAmt">交易钱数</param>
        /// <param name="proName">产品名称 可选</param>
        public static ContentResult Pay(int userId, string orderID, string proName, decimal TransAmt, string cpyPageRetUrl, string cpyBgRetUrl, string cancelUrl, string merId, string merKeyPath)
        {
            UnionPayHelper cpy = new UnionPayHelper();
            //获取传递给银联chinapay的各个参数-----------------------------------------------
            //string cpyUrl = "http://payment-test.chinapay.com/pay/TransGet"; //测试地址，测试的时候用这个地址，应用到网站时用下面那个地址
            string cpyUrl = "https://payment.chinapay.com/pay/TransGet";

            //ChinaPay统一分配给商户的商户号，15位长度，必填
            string cpyOrdId = "00" + DateTime.Now.ToString("yyyyMMddhhmmss");// 16位长度，必填，由于站内各订单的编号长度不一致，所以生成临时订单号给银联，实际以本站订单号进行操作
            string cpyTransAmt = getTransAmt(TransAmt); //订单交易金额，12位长度，左补0，必填,单位为分，000000000001 表示 12.34 元
            string cpyCuryId = "156";            //订单交易币种，3位长度，固定为人民币156，必填
            string cpyTransDate = DateTime.Now.ToString("yyyyMMdd");            //订单交易日期，8位长度，必填，格式yyyyMMdd
            string cpyTransType = "0001";        //交易类型，4位长度，必填，0001表示消费交易，0002表示退货交易
            string cpyVersion = "20070129";// "20070129";      //支付接入版本号，808080开头的商户用此版本，必填,另一版本为"20070129"
            //  string cpyBgRetUrl = "https://test003.abc.cc/Chinapay_Bgreturn.aspx";   //后台交易接收URL，为后台接受应答地址，用于商户记录交易信息和处理，对于使用者是不可见的，长度不要超过80个字节，必填
            //string cpyPageRetUrl = "https://test003.abc.cc/Chinapay_Pgreturn.aspx"; //页面交易接收URL，为页面接受应答地址，用于引导使用者返回支付后的商户网站页面，长度不要超过80个字节，必填
            string cpyGateId = "8607";  //支付网关号，可选，参看银联网关类型，如填写GateId（支付网关号），则消费者将直接进入支付页面，否则进入网关选择页面,可登陆商户管理平台 查看各个银行的网管号
            string cpyPriv1 = orderID + "|" + Convert.ToString(userId);// proName;  //商户私有域，长度不要超过60个字节,商户通过此字段向Chinapay发送的信息，Chinapay依原样填充返回给商户

            string strChkValue = ""; //256字节长的ASCII码,此次交易所提交的关键数据的数字签名，必填
            string plainValue = merId + cpyOrdId + cpyTransAmt + cpyCuryId + cpyTransDate + cpyTransType + cpyPriv1;// TransType & Priv1;
            // strChkValue = cpy.getSign(merId, cpyOrdId, cpyTransAmt, cpyCuryId, cpyTransDate, cpyTransType, merKeyPath);
            //Plain = MER_ID & OrdId & TransAmt & CuryId & TransDate & TransType & Priv1
            strChkValue = cpy.signData(merId, plainValue, merKeyPath);


            if (strChkValue.Length != 256)
            {
                return new ContentResult { Content = "<script>alert('签名失败！');location.href='" + cancelUrl + "';</script>" };
            }
            StringBuilder shtml = new StringBuilder();
            shtml.Append("<form name='chinapayForm' method='post' action='" + cpyUrl + "'>");         //支付地址
            shtml.Append("<input type='hidden' name='MerId' value='" + merId + "' />");            //商户号
            shtml.Append("<input type='hidden' name='OrdId' value='" + cpyOrdId + "' />");            //订单号
            shtml.Append("<input type='hidden' name='TransAmt' value='" + cpyTransAmt + "' />");      //支付金额
            shtml.Append("<input type='hidden' name='CuryId' value='" + cpyCuryId + "' />");          //交易币种
            shtml.Append("<input type='hidden' name='TransDate' value='" + cpyTransDate + "' />");    //交易日期
            shtml.Append("<input type='hidden' name='TransType' value='" + cpyTransType + "' />");    //交易类型
            shtml.Append("<input type='hidden' name='Version' value='" + cpyVersion + "' />");        //支付接入版本号
            shtml.Append("<input type='hidden' name='BgRetUrl' value='" + cpyBgRetUrl + "' />");      //后台接受应答地址
            shtml.Append("<input type='hidden' name='PageRetUrl' value='" + cpyPageRetUrl + "' />");  //为页面接受应答地址
            shtml.Append("<input type='hidden' name='GateId' value='" + cpyGateId + "' />");          //支付网关号
            shtml.Append("<input type='hidden' name='Priv1' value='" + cpyPriv1 + "' />");            //商户私有域，这里将订单自增编号放进去了
            shtml.Append("<input type='hidden' name='ChkValue' value='" + strChkValue + "' />");      //此次交易所提交的关键数据的数字签名
            shtml.Append("<script>");
            shtml.Append("document.chinapayForm.submit();");
            shtml.Append("</script></form>");


            return new ContentResult { Content = shtml.ToString() };
        }

        //返回交易金额
        private static string getTransAmt(decimal money)
        {
            string moneyCount = Convert.ToString(Convert.ToInt32(money * 100));
            //string moneyCount = count.ToString().Replace(".", "");
            return moneyCount.PadLeft(12, '0');
        }

        public static ActionResult PayDone(BaseController controller, string merId, string merKeyPath, Func<int, string, string, string, ActionResult> success, string failMsg = "", string failUrl = "")
        {
            string username = (CurrentUser.IsAuthenticated ? CurrentUser.UserName + "：" : string.Empty);
            int currUserId = CurrentUser.IsAuthenticated ? (int)CurrentUser.UserId : 0;
            string merid = merId;
            string orderno = controller.Request["orderno"];//和银联支付交易的订单号，额外生成的。
            string transdate = controller.Request["transdate"];
            string amount = controller.Request["amount"];
            string transtype = controller.Request["transtype"];
            string CuryId = controller.Request["currencycode"];
            string status = controller.Request["status"];
            string bcheckvalue = controller.Request["checkvalue"];
            string GateId = controller.Request["GateId"];
            string Priv1 = controller.Request["Priv1"];
            string ly_orderId = Priv1.Split('|')[0];//本站订单号 
            int userId = Convert.ToInt32(Priv1.Split('|')[1]);
            //if (!currUserId.Equals(userId))
            //{
            //    Log4NetFactory.OperationLogger.InfoWithUser("订单号格式不正确", username);
            //    return new ContentResult { Content = "<script>alert('" + failMsg + "');location.href='" + failUrl + "';</script>" };
            //}
            UnionPayHelper cpy = new UnionPayHelper();

            string strChkValue = ""; //256字节长的ASCII码,此次交易所提交的关键数据的数字签名  
            string plainValue = merId + orderno + amount + CuryId + transdate + transtype + Priv1;
            strChkValue = cpy.signData(merId, plainValue, merKeyPath);
            if (strChkValue.Length != 256)
            {
                return new ContentResult { Content = "<script>alert('签名失败！');location.href='" + failUrl + "';</script>" };
            }

            if (status.Equals("1001"))
            {
                return success(userId, ly_orderId, orderno, ""/*似乎没有账号*/);
            }
            else
            {
                Log4NetFactory.OperationLogger.InfoWithUser("银联支付状态失败", username);
                return new ContentResult { Content = "<script>alert('" + failMsg + "');location.href='" + failUrl + "';</script>" };
            }
            //Log4NetFactory.OperationLogger.InfoWithUser("银联支付状态失败", username);
            //return new ContentResult { Content = "<script>alert('" + failMsg + "');location.href='" + failUrl + "';</script>" };
        }


        public static ActionResult PayDoneAsync(BaseController controller, string merId, string merKeyPath, Func<int, string, string, string, ActionResult> success, string failMsg = "", string failUrl = "")
        {
            string username = (CurrentUser.IsAuthenticated ? CurrentUser.UserName + "：" : string.Empty);
            int currUserId = CurrentUser.IsAuthenticated ? (int)CurrentUser.UserId : 0;
            string merid = merId;
            string orderno = controller.Request["orderno"];//和银联支付交易的订单号，额外生成的。
            string transdate = controller.Request["transdate"];
            string amount = controller.Request["amount"];
            string transtype = controller.Request["transtype"];
            string CuryId = controller.Request["currencycode"];
            string status = controller.Request["status"];
            string bcheckvalue = controller.Request["checkvalue"];
            string GateId = controller.Request["GateId"];
            string Priv1 = controller.Request["Priv1"];
            string ly_orderId = Priv1.Split('|')[0];//本站订单号 
            int userId = Convert.ToInt32(Priv1.Split('|')[1]);//用户Id
            //if (!currUserId.Equals(userId))
            //{
            //    Log4NetFactory.OperationLogger.InfoWithUser("订单号格式不正确", username);
            //    return new ContentResult { Content = "<script>alert('" + failMsg + "');location.href='" + failUrl + "';</script>" };
            //}
            UnionPayHelper cpy = new UnionPayHelper();

            string strChkValue = ""; //256字节长的ASCII码,此次交易所提交的关键数据的数字签名 


            //检验是否是银联chinapay返回的交易数据
            bool checkStatus = cpy.getCheck(merid, orderno, amount, CuryId, transdate, transtype, status, bcheckvalue, merKeyPath);

            if (!checkStatus)
            {
                Log4NetFactory.OperationLogger.InfoWithUser("检验交易数据失败", username);
                return new ContentResult { Content = "<script>alert('" + failMsg + "');location.href='" + failUrl + "';</script>" };
            }
            if (status.Equals("1001"))
            {
                return success(userId, ly_orderId, orderno, ""/*似乎没有账号*/);
            }
            else
            {
                Log4NetFactory.OperationLogger.InfoWithUser("银联支付状态失败", username);
                return new ContentResult { Content = "<script>alert('" + failMsg + "');location.href='" + failUrl + "';</script>" };
            }
            //Log4NetFactory.OperationLogger.InfoWithUser("银联支付失败", username);
            //return new ContentResult { Content = "<script>alert('" + failMsg + "');location.href='" + failUrl + "';</script>" };
        }

    }

    public class Config
    {
        #region 字段


        private static string cpyCuryId = string.Empty;
        /// <summary>
        /// value="156"（订单交易币种，3位长度，固定为人民币156，必填）
        /// </summary>
        public static string CpyCuryId
        {
            get { return cpyCuryId; }
            set { cpyCuryId = value; }
        }

        private static string cpyTransType = string.Empty;
        /// <summary>
        /// value="0001"交易类型，必填，0001表示消费交易，0002表示退货交易
        /// </summary>
        public static string CpyTransType
        {
            get { return cpyTransType; }
            set { cpyTransType = value; }
        }

        private static string cpyVersion = string.Empty;
        /// <summary>
        /// value="20070129" （支付接入版本号，必填）
        /// </summary>
        public static string CpyVersion
        {
            get { return cpyVersion; }
            set { cpyVersion = value; }
        }

        private static string cpyGateId = string.Empty;
        /// <summary>
        /// 支付网关号，可选，参看银联网关类型，如填写GateId（支付网关号），则消费者将直接进入支付页面，否则进入网关选择页面,可登陆商户管理平台 查看各个银行的网管号
        /// </summary>
        public static string CpyGateId
        {
            get { return cpyGateId; }
            set { cpyGateId = value; }
        }

        private static string merPrk = string.Empty;
        /// <summary>
        /// 密钥
        /// </summary>
        public static string MerPrk
        {
            get { return merPrk; }
            set { merPrk = value; }
        }

        private static string pgPubk = string.Empty;
        /// <summary>
        /// 公钥
        /// </summary>
        public static string PgPubk
        {
            get { return pgPubk; }
            set { pgPubk = value; }
        }

        private static string reqUrlPay = string.Empty;
        /// <summary>
        /// 提交交易数据的URL地址
        /// </summary>
        public static string ReqUrlPay
        {
            get { return reqUrlPay; }
            set { reqUrlPay = value; }
        }

        private static string reqUrlQry = string.Empty;
        public static string ReqUrlQry
        {
            get { return reqUrlQry; }
            set { reqUrlQry = value; }
        }

        private static string reqUrlRef = string.Empty;
        public static string ReqUrlRef
        {
            get { return reqUrlRef; }
            set { reqUrlRef = value; }
        }

        #endregion

        public Config()
        {
            ReqUrlPay = "https://payment.chinapay.com/pay/TransGet";
            //ReqUrlPay = "https://payment-test.chinapay.com/pay/TransGet";
            ReqUrlQry = "https://console.chinapay.com/QueryWeb/processQuery.jsp";
            ReqUrlRef = "https://bak.chinapay.com/refund/SingleRefund.jsp";
            MerPrk = "MerPrK.key";
            PgPubk = "PgPubk.key";

            CpyCuryId = "156";
            CpyTransType = "0001";
            CpyVersion = "20070129";
            CpyGateId = "8607";
        }

    }

}