using MyCoreLib.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MyCoreLib.BaseLog.DBLog
{
    public class SystemDBLogger
    {
        public int LogId { get; private set; }
        public SystemLogType Type { get; private set; }
        public string Message { get; private set; }
        public DateTime TimeCreated { get; private set; }

        public void ReadFromDataReader(IDataReader dr)
        {
            LogId = (int)dr["log_id"];
            Message = (string)dr["message"];
            TimeCreated = (DateTime)dr["time_created"];
            Type = (SystemLogType)Enum.Parse(typeof(SystemLogType), (string)dr["type"], true);
        }

        public static void Log(string tag, string message, SystemLogType type = SystemLogType.Error, DBInstance instance = DBInstance.WeipanDB)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentNullException("message");
            if (string.IsNullOrEmpty(tag)) throw new ArgumentNullException("tag");
            if (tag.Length > 32) throw new ArgumentException("Tag name is too long to use.", "tag");

            try
            {
                using (var db = DBFactory.GetFactory().GetSQLConnection(instance))
                {
                    //WeipanDB.sproc_sys_log_add.ExecuteNonQuery(db, tag, type.ToString(), message);
                }
            }
            catch
            {
                // Ignore all exceptions
            }
        }

        /// <summary>
        /// Remove logs whose ids are smaller than or equal to the specified id.
        /// </summary>
        public static void Clean(int startId, DBInstance instance = DBInstance.WeipanDB)
        {
            if (startId <= 0)
                throw new ArgumentOutOfRangeException("startId");

            using (var db = DBFactory.GetFactory().GetSQLConnection(instance))
            {
                //WeipanDB.sproc_sys_log_clean.ExecuteNonQuery(db, startId);
            }
        }

        /// <summary>
        /// Get a list of logs.
        /// </summary>
        public static List<SystemDBLogger> GetLogs(int startId, int maxRows, out int minLogId, out int maxLogId, DBInstance instance = DBInstance.WeipanDB_Slave)
        {
            if (startId <= 0)
                throw new ArgumentOutOfRangeException("startId");
            if (maxRows <= 0)
                throw new ArgumentOutOfRangeException("maxRows");

            using (var db = DBFactory.GetFactory().GetSQLConnection(instance))
            {
                //using (var dr = WeipanDB.sproc_sys_log_get_one_page.ExecuteReader(db, startId, maxRows))
                //{
                //    var logs = dr.ReadEntities<SystemDBLog>();

                //    if (dr.NextResult() && dr.Read())
                //    {
                //        minLogId = Convert.ToInt32(dr["min_id"]);
                //        maxLogId = Convert.ToInt32(dr["max_id"]);
                //    }
                //    else
                //    {
                //        minLogId = maxLogId = 0;
                //    }

                //    return logs;
                //}
                minLogId = maxLogId = 0;
                return null;
            }
        }

    }
}
