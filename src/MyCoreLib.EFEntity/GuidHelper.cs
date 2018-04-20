using System;


namespace EF.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class GuidHelper
    {
        /// <summary>
        /// 增加随机控制
        /// </summary>
        static byte GuidByte = 1;
        /// <summary>
        /// 带日期规则的Guid
        /// </summary>
        /// <returns></returns>
        public static Guid GetGuid()
        {
            DateTime DT = DateTime.Now;
            //bebe40b1-a6a2-4c28-ae86-f405eb260059
            Random R = new Random(GetRandomSeed());
            byte[] bs = new byte[8];
            R.NextBytes(bs);
            bs[7] = (++GuidByte);
            int i = Convert.ToInt32(DT.ToString("yyMMddHH"), 16);

            int value1 = R.Next(1000, 10000);
            short s1 = Convert.ToInt16(value1.ToString(), 16);
            int value2 = R.Next(1000, 10000);
            short s2 = Convert.ToInt16(value2.ToString(), 16);
            return new Guid(i, s1, s2, bs);
        }
        static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// 作者：lcq
        /// 时间：2016-03-28
        /// 功能描述：生成唯一的编号
        /// </summary>
        /// <returns>唯一的编号</returns>
        public static long GetKeyNo()
        {
            string strKeyNo = string.Concat(DateTime.Now.ToString("yyMMddHH", System.Globalization.CultureInfo.InvariantCulture), GetUniqueCode(8));
            return Convert.ToInt64(strKeyNo);
        }

        public static Guid GetGuid(string GuidNo)
        {
            if (string.IsNullOrEmpty(GuidNo))
            {
                return Guid.Empty;
            }
            else
            {
                try
                {
                    return new Guid(GuidNo);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        #region "  函数与过程   "

        /// <summary>
        /// 作者:lcq
        /// 时间:2016-03-28
        /// 功能:获取Guid的唯一HashCode值
        /// <param name="intDigit">位数</param>
        /// </summary>
        private static string GetUniqueCode(int intDigit = 7)
        {
            if (intDigit <= 0)
            {
                intDigit = 7;
            }
            string strReturn = Math.Abs(Guid.NewGuid().ToString().GetHashCode()).ToString();
            if (strReturn.Length < intDigit)
            {
                return strReturn.PadRight(intDigit, '0');
            }
            else
            {
                return strReturn.Substring(0, intDigit);
            }
        }

        #endregion
    }
}
