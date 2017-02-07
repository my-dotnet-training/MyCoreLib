
using System;

namespace MyCoreLib.Data
{
    public static class DBConvertExtension
    {
        public static bool IsDBNull(object value)
        {
            if (value == DBNull.Value)
            {
                return true;
            }
            IConvertible convertible = value as IConvertible;
            return convertible != null && convertible.GetTypeCode() == TypeCode.Empty;
        }
    }
}
