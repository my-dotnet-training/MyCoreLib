using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseOrm.Aop
{
    public static class SqlConvert
    {
        public static string ToSelectSql<T>(this T obj)
        {

            return "";
        }

        public static T FromSql<T>(this string s)
        {

            return default(T);
        }
    }
}
