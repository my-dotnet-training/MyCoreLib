using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace MyCoreLib.BaseLog.FileLog
{
    /// <summary>
    /// Write log entries to the destination file.
    /// </summary>
    public static class ToolLogger
    {
        private static TextWriter s_output;

        public static void Init(TextWriter sw)
        {
            s_output = sw;
        }

        /// <summary>
        /// Initialize the logger using console
        /// </summary>
        public static void Init(string path, string logFilePrefix)
        {
            string logFileName = string.Format("{0}_{1:yyyyMMddHHmmss}.log", logFilePrefix, DateTime.Now);
            string logFileFullPath = Path.Combine(path, logFileName);

            var listener = new TextWriterTraceListener(File.Open(logFileFullPath, FileMode.OpenOrCreate));
            listener.IndentSize = 2;
            listener.TraceOutputOptions = TraceOptions.Timestamp;
            Trace.Listeners.Add(listener);
            s_output = null;
        }

        public static void Flush()
        {
            if (s_output == null)
            {
                Trace.Flush();
            }
            else
            {
                s_output.Flush();
            }
        }

        public static void WriteLine(int index, string objectName)
        {
            WriteLine(" {0}: {1}", index.ToString().PadLeft(4), objectName);
        }

        public static void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }

        public static void WriteLine()
        {
            WriteLine(null);
        }

        public static void Write(string information)
        {
            Trace.Write(information);
            Console.Write(information);
        }

        public static void WriteLine(string information)
        {
            Trace.WriteLine(information);
            Console.WriteLine(information);
            if (s_output != null)
            {
                s_output.WriteLine(information);
            }
        }

        public static void WriteError(string format, params object[] args)
        {
            WriteError(string.Format(format, args));
        }

        public static void WriteError(string error)
        {
            Trace.WriteLine(error);

            if (s_output != null)
            {
                s_output.WriteLine(error);
                Console.WriteLine(error);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(error);
                Console.ResetColor();
            }
        }

        public static void LogException(Exception exp)
        {
            var sb = new StringBuilder();
            bool rootExp = true;

            while (true)
            {
                sb.Append(rootExp ? "EXCEPTION: " : "INNER EXCEPTION: ");
                sb.AppendLine(exp.Message);

                sb.Append("TYPE: ");
                sb.AppendLine(exp.GetType().FullName);

                rootExp = false;
                exp = exp.InnerException;
                if (exp == null)
                {
                    break;
                }
            }

            WriteError(sb.ToString());
        }
    }
}
