using MyCoreLib.BaseData;
using MyCoreLib.Common.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MyCoreLib.BaseLog.DBLog
{
    public class SystemDBLog : BaseSingleton<SystemDBLog>, ILog
    {
        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        private const string m_MessageTemplate = "{0}-[{1}]: {2}";

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

        private void Log(string message, SystemLogType type = SystemLogType.Error, DBInstance instance = DBInstance.WeipanDB)
        {
            if (string.IsNullOrEmpty(message)) throw new ArgumentNullException("message");
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
        public void Clean(int startId, DBInstance instance = DBInstance.WeipanDB)
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
        public List<SystemDBLog> GetLogs(int startId, int maxRows, out int minLogId, out int maxLogId, DBInstance instance = DBInstance.WeipanDB_Slave)
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

        #region ILog

        /// <summary>
        /// Gets a value indicating whether this instance is debug enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is debug enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsDebugEnabled
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is error enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is error enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsErrorEnabled
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is fatal enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is fatal enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsFatalEnabled
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is info enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is info enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsInfoEnabled
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is warn enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is warn enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsWarnEnabled
        {
            get { return true; }
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(object message)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Debug), message), SystemLogType.Debug);
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Debug(object message, Exception exception)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Debug), message + Environment.NewLine + exception.Message + exception.StackTrace), SystemLogType.Debug);
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public void DebugFormat(string format, object arg0)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Debug), string.Format(format, arg0)), SystemLogType.Debug);
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void DebugFormat(string format, params object[] args)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Debug), string.Format(format, args)), SystemLogType.Debug);
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Debug), string.Format(provider, format, args)), SystemLogType.Debug);
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public void DebugFormat(string format, object arg0, object arg1)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Debug), string.Format(format, arg0, arg1)), SystemLogType.Debug);
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Debug), string.Format(format, arg0, arg1, arg2)), SystemLogType.Debug);
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(object message)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Error), message), SystemLogType.Error);
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Error(object message, Exception exception)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Error), message + Environment.NewLine + exception.Message + exception.StackTrace), SystemLogType.Error);
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public void ErrorFormat(string format, object arg0)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
                 Enum.GetName(typeof(SystemLogType), SystemLogType.Error), string.Format(format, arg0)), SystemLogType.Error);
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void ErrorFormat(string format, params object[] args)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Error), string.Format(format, args)), SystemLogType.Error);
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Error), string.Format(provider, format, args)), SystemLogType.Error);
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public void ErrorFormat(string format, object arg0, object arg1)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Error), string.Format(format, arg0, arg1)), SystemLogType.Error);
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Error), string.Format(format, arg0, arg2)), SystemLogType.Error);
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Fatal(object message)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Fatal), message), SystemLogType.Fatal);
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Fatal(object message, Exception exception)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Fatal), message + Environment.NewLine + exception.Message + exception.StackTrace), SystemLogType.Fatal);
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public void FatalFormat(string format, object arg0)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Fatal), string.Format(format, arg0)), SystemLogType.Fatal);
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void FatalFormat(string format, params object[] args)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Fatal), string.Format(format, args)), SystemLogType.Fatal);
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Fatal), string.Format(provider, format, args)), SystemLogType.Fatal);
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public void FatalFormat(string format, object arg0, object arg1)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Fatal), string.Format(format, arg0, arg1)), SystemLogType.Fatal);
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Fatal), string.Format(format, arg0, arg1, arg2)), SystemLogType.Fatal);
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(object message)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Info), message), SystemLogType.Info);
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Info(object message, Exception exception)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Info), message + Environment.NewLine + exception.Message + exception.StackTrace), SystemLogType.Info);
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public void InfoFormat(string format, object arg0)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Info), string.Format(format, arg0)), SystemLogType.Info);
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void InfoFormat(string format, params object[] args)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Info), string.Format(format, args)), SystemLogType.Info);
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Info), string.Format(provider, format, args)), SystemLogType.Info);
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public void InfoFormat(string format, object arg0, object arg1)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Info), string.Format(format, arg0, arg1)), SystemLogType.Info);
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Info), string.Format(format, arg0, arg1, arg2)), SystemLogType.Info);
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(object message)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Warning), message), SystemLogType.Warning);
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Warn(object message, Exception exception)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Warning), message + Environment.NewLine + exception.Message + exception.StackTrace), SystemLogType.Warning);
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public void WarnFormat(string format, object arg0)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Warning), string.Format(format, arg0)), SystemLogType.Warning);
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void WarnFormat(string format, params object[] args)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Warning), string.Format(format, args)), SystemLogType.Warning);
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Warning), string.Format(provider, format, args)), SystemLogType.Warning);
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public void WarnFormat(string format, object arg0, object arg1)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Warning), string.Format(format, arg0, arg1)), SystemLogType.Warning);
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        /// <param name="arg2">The arg2.</param>
        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            this.Log(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Warning), string.Format(format, arg0, arg1, arg2)), SystemLogType.Warning);
        }
        #endregion
    }
}
