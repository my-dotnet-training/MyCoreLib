using System;
using System.Data.SqlTypes;

namespace MyCoreLib.Data.MySql
{
    /// <summary>
    /// The utility class for converting data fetching from database.
    /// </summary>
    public class SqlValueConverter
    {
        /// <summary>
        /// always save UTC time to database.
        /// </summary>
        public static SqlDateTime GetSqlDateTime(DateTime value)
        {
            // Probably DateTime.MinValue
            if (value < SqlDateTime.MinValue.Value)
                return SqlDateTime.Null;

            if (value > SqlDateTime.MaxValue.Value)
                return SqlDateTime.MaxValue.Value;

            // always save UTC time to database.
            return new SqlDateTime(value.ToUniversalTime());
        }

        /// <summary>
        /// read time column from db
        /// </summary>
        public static DateTime ReadDateTime(SqlDateTime val)
        {
            return val.IsNull ? DateTime.MinValue : DateTime.SpecifyKind(val.Value, DateTimeKind.Utc);
        }

        /// <summary>
        /// read guid column from db
        /// </summary>
        public static Guid ReadGuid(SqlGuid val)
        {
            return val.IsNull ? Guid.Empty : val.Value;
        }

        /// <summary>
        /// read string column from db
        /// </summary>
        public static string ReadString(SqlString val)
        {
            return val.IsNull ? null : val.Value;
        }

        public static byte ReadByte(SqlByte val, byte defaultValue)
        {
            return val.IsNull ? defaultValue : val.Value;
        }

        public static short ReadInt16(SqlInt16 val, short defaultValue)
        {
            return val.IsNull ? defaultValue : val.Value;
        }

        public static int ReadInt32(SqlInt32 val, int defaultValue)
        {
            return val.IsNull ? defaultValue : val.Value;
        }

        public static long ReadInt64(SqlInt64 val, long defaultValue)
        {
            return val.IsNull ? defaultValue : val.Value;
        }

        public static double ReadDouble(SqlDouble val, double defaultValue)
        {
            return val.IsNull ? defaultValue : val.Value;
        }

        public static string ParseSqlString(object obj)
        {
            return DBConvertExtension.IsDBNull(obj) ? null : Convert.ToString(obj);
        }

        public static Boolean ParseSqlBoolean(object obj)
        {
            return DBConvertExtension.IsDBNull(obj) ? false : Convert.ToBoolean(obj);
        }

        public static int ParseSqlInt(object obj)
        {
            return DBConvertExtension.IsDBNull(obj) ? 0 : Convert.ToInt32(obj);
        }

        public static Int64 ParseSqlInt64(object obj)
        {
            return DBConvertExtension.IsDBNull(obj) ? 0 : Convert.ToInt64(obj);
        }

        public static DateTime ParseSqlDateTime(object obj)
        {
            return DBConvertExtension.IsDBNull(obj) ? DateTime.MinValue : Convert.ToDateTime(obj);
        }


        public static Guid ParseSqlGUID(object obj)
        {
            return DBConvertExtension.IsDBNull(obj) ? Guid.Empty : (Guid)obj;
        }

        public static short ParseSqlShort(object obj)
        {
            if (DBConvertExtension.IsDBNull(obj))
                return 0;
            else
                return Convert.ToInt16(obj);
        }

        public static Guid ParseSqlUniqueIdentifier(object obj)
        {
            return DBConvertExtension.IsDBNull(obj) ? Guid.Empty : new Guid(Convert.ToString(obj));
        }

        /// <summary>
        /// Return corrected ado.net datetime type when a sql datetime type is passed.
        /// </summary>
        public static DateTime ParseSqlDateTimeExact(object obj)
        {
            if (DBConvertExtension.IsDBNull(obj))
                return DateTime.MinValue;
            else if (Convert.ToDateTime(obj) == SqlDateTime.MinValue)
                return DateTime.MinValue;
            else
                return DateTime.SpecifyKind(Convert.ToDateTime(obj), DateTimeKind.Utc);
        }

        /// <summary>
        /// return corrected ado.net datetime type when a sql datetime type is passed.
        /// </summary>
        public static DateTime ParseSqlDateTimeLocal(object obj)
        {
            if (DBConvertExtension.IsDBNull(obj))
                return DateTime.MinValue;
            else if (Convert.ToDateTime(obj) == SqlDateTime.MinValue)
                return DateTime.MinValue;
            else
                return DateTime.SpecifyKind(Convert.ToDateTime(obj), DateTimeKind.Utc);
        }

        public static float ParseSqlFloat(object obj)
        {
            return DBConvertExtension.IsDBNull(obj) ? 0.0F : (float)Double.Parse(obj.ToString());
        }
    }
}
