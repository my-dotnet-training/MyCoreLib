
using MyCoreLib.Common.Model;
using System;

namespace MyCoreLib.BaseLog.DebugLog
{
    public class OutputDebugLog : BaseSingleton<OutputDebugLog>, ILog
    {
        private string m_Name;
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        private const string m_MessageTemplate = "{0}-[{1}]: {2}";

        public OutputDebugLog()
        {
        }

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
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Debug), message));
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Debug(object message, Exception exception)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Debug), message + Environment.NewLine + exception.Message + exception.StackTrace));
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public void DebugFormat(string format, object arg0)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Debug), string.Format(format, arg0)));
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void DebugFormat(string format, params object[] args)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Debug), string.Format(format, args)));
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Debug), string.Format(provider, format, args)));
        }

        /// <summary>
        /// Logs the debug message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public void DebugFormat(string format, object arg0, object arg1)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Debug), string.Format(format, arg0, arg1)));
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
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Debug), string.Format(format, arg0, arg1, arg2)));
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(object message)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Error), message));
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Error(object message, Exception exception)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
                Enum.GetName(typeof(SystemLogType), SystemLogType.Error), message + Environment.NewLine + exception.Message + exception.StackTrace));
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public void ErrorFormat(string format, object arg0)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
                 Enum.GetName(typeof(SystemLogType), SystemLogType.Error), string.Format(format, arg0)));
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void ErrorFormat(string format, params object[] args)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Error), string.Format(format, args)));
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Error), string.Format(provider, format, args)));
        }

        /// <summary>
        /// Logs the error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public void ErrorFormat(string format, object arg0, object arg1)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Error), string.Format(format, arg0, arg1)));
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
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Error), string.Format(format, arg0, arg2)));
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Fatal(object message)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Fatal), message));
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Fatal(object message, Exception exception)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Fatal), message + Environment.NewLine + exception.Message + exception.StackTrace));
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public void FatalFormat(string format, object arg0)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Fatal), string.Format(format, arg0)));
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void FatalFormat(string format, params object[] args)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Fatal), string.Format(format, args)));
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Fatal), string.Format(provider, format, args)));
        }

        /// <summary>
        /// Logs the fatal error message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public void FatalFormat(string format, object arg0, object arg1)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Fatal), string.Format(format, arg0, arg1)));
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
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Fatal), string.Format(format, arg0, arg1, arg2)));
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(object message)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Info), message));
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Info(object message, Exception exception)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Info), message + Environment.NewLine + exception.Message + exception.StackTrace));
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public void InfoFormat(string format, object arg0)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Info), string.Format(format, arg0)));
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void InfoFormat(string format, params object[] args)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Info), string.Format(format, args)));
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Info), string.Format(provider, format, args)));
        }

        /// <summary>
        /// Logs the info message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public void InfoFormat(string format, object arg0, object arg1)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Info), string.Format(format, arg0, arg1)));
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
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Info), string.Format(format, arg0, arg1, arg2)));
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(object message)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Warning), message));
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public void Warn(object message, Exception exception)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Warning), message + Environment.NewLine + exception.Message + exception.StackTrace));
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        public void WarnFormat(string format, object arg0)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Warning), string.Format(format, arg0)));
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void WarnFormat(string format, params object[] args)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Warning), string.Format(format, args)));
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Warning), string.Format(provider, format, args)));
        }

        /// <summary>
        /// Logs the warning message.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1">The arg1.</param>
        public void WarnFormat(string format, object arg0, object arg1)
        {
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Warning), string.Format(format, arg0, arg1)));
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
            SafeNativeMethods.OutputDebugString(string.Format(m_MessageTemplate, m_Name,
             Enum.GetName(typeof(SystemLogType), SystemLogType.Warning), string.Format(format, arg0, arg1, arg2)));
        }
    }
}
