
namespace MyCoreLib.BaseLog.FileLog
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    /// <summary>
    /// Provider logging functions.
    /// </summary>
    public static class FileLogger
    {
        private static string[] s_labelArray;

        public static void Init(string path,bool supportConsoleOutput)
        {
            // Initialize label array
            s_labelArray = new string[(int)TraceLevel.Verbose + 1];
            s_labelArray[(int)TraceLevel.Info] = "Info";
            s_labelArray[(int)TraceLevel.Warning] = "Warning";
            s_labelArray[(int)TraceLevel.Error] = "Error";
            s_labelArray[(int)TraceLevel.Verbose] = "Verbose";

            // Make sure the logs directory exists.
            string logsFolder = Path.Combine(path, "logs");
            Directory.CreateDirectory(logsFolder);

            // Set up trace listeners
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new RolloverTraceListener(logsFolder, "log") { TraceOutputOptions = TraceOptions.Timestamp });
            if (supportConsoleOutput)
            {
                Trace.Listeners.Add(new TextWriterTraceListener());
            }

            // Enable auto-flush to avoid missing logs on app crash.
            Trace.AutoFlush = true;
            Trace.WriteLine(null);
            Trace.WriteLine("## Tracer starts to work in quotation service ##");
        }

        public static void WriteLine(TraceLevel level, string format, params object[] args)
        {
            WriteLine(level, string.Format(format, args));
        }

        public static void WriteLine()
        {
            Trace.WriteLine(null);
        }

        public static void WriteLine(TraceLevel level, string message)
        {
            Trace.WriteLine(string.Format("{0:yyyy-MM-ddTHH:mm:ss.fff} [{1}] {2}", DateTime.Now, s_labelArray[(int)level], message));
        }

        public static void LogException(Exception ex)
        {
            WriteLine(TraceLevel.Error, "Unexpected exception: {0}", ex);
        }

        internal static void LogTaskFailure(Task t)
        {
            Exception ex = null;

            try
            {
                t.Wait();
            }
            catch (AggregateException aggEx)
            {
                ex = aggEx;
                if (aggEx.InnerExceptions != null && aggEx.InnerExceptions.Count == 1)
                {
                    ex = aggEx.InnerExceptions[0];
                    if ((ex is IOException)
                        && (ex.InnerException != null)
                        && (ex.InnerException is SocketException))
                    {
                        ex = ex.InnerException;

                        // WinSock Error Code:
                        // 10054: The connection is reset by remote side.
                        if (((SocketException)ex).HResult == 10054)
                        {
                            // The client is already disconnected. We can ignore the error.
                            ex = null;
                        }
                    }
                }
            }
            catch (Exception _ex)
            {
                ex = _ex;
            }

            // Log the unknown exception.
            if (ex != null)
            {
                FileLogger.WriteLine(TraceLevel.Error, "Unexpected exception: {0}", ex);
            }
        }
    }
}
