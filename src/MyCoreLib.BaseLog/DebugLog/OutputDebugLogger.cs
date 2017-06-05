
namespace MyCoreLib.BaseLog.DebugLog
{
    public class OutputDebugLogger
    {
        /// <summary>
        /// Send a text to debug output.
        /// </summary>
        public static void Log(string message)
        {
            SafeNativeMethods.OutputDebugString(message ?? string.Empty);
        }

        /// <summary>
        /// Send a text to debug output.
        /// </summary>
        public static void Log(string format, params object[] args)
        {
            string msg = string.Format(format, args);
            SafeNativeMethods.OutputDebugString(msg ?? string.Empty);
        }
    }
}
