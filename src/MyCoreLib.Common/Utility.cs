
namespace MyCoreLib.Common
{
    using MyCoreLib.Common;
    using System;
    using System.Net;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Provides some common methods which could potentially be useful for all projects.
    /// </summary>
    public static class Utility
    {
        private static string s_machineName;

        /// <summary>
        /// Get the name of the current machine.
        /// </summary>
        public static string MachineName
        {
            get
            {
                if (s_machineName == null)
                {
                    Interlocked.CompareExchange(ref s_machineName, GetMachineName(), null);
                }
                return s_machineName;
            }
        }

        /// <summary>
        /// Remove millisecond component for the specified DateTime.
        /// </summary>
        public static DateTime TrimMilliseconds(DateTime t)
        {
            return new DateTime(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second, t.Kind);
        }

        public static string FormatDateTime(DateTime dateTime, bool hasSeconds = false)
        {
            if (hasSeconds)
                return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            return dateTime.ToString("yyyy-MM-dd HH:mm");
        }

        public static string FormatDate(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Obfuscate the specified string.
        /// </summary>
        public static string ObfuscateString(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return val;

            int len = val.Length;
            if (len == 1)
            {
                return "*";
            }

            int numHidden = Math.Max((int)Math.Ceiling(len / 3d), val.Length - 8);
            int numRemaining = val.Length - numHidden;

            var sb = new StringBuilder(val.Length);
            sb.Append(val.Substring(0, numRemaining / 2));

            for (int i = 0; i < numHidden; i++)
            {
                sb.Append('*');
            }

            int left = numRemaining - numRemaining / 2;
            sb.Append(val.Substring(len - left));

            return sb.ToString();
        }

        public static string GetExternalIP()
        {
            IPHostEntry entry = Dns.GetHostEntryAsync(Environment.MachineName).Result;
            if (entry != null)
            {
                foreach (IPAddress addr in entry.AddressList)
                {
                    // Only use IPv4 addresses for now
                    if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        byte[] bytes = addr.GetAddressBytes();
                        if (bytes[0] != 192     // 192.*** is for LAN.
                            && bytes[0] != 169  // 169.* is the internal address for aliyun.
                            && bytes[0] != 10)  // 10.* is the internal address for aliyun.
                        {
                            return addr.ToString();
                        }
                    }
                }
            }

            throw new InvalidOperationException("Failed to get external ip.");
        }

        /// <summary>
        /// Send a text to debug output.
        /// </summary>
        public static void OutputDebugString(string message)
        {
            SafeNativeMethods.OutputDebugString(message ?? string.Empty);
        }

        /// <summary>
        /// Send a text to debug output.
        /// </summary>
        public static void OutputDebugString(string format, params object[] args)
        {
            string msg = string.Format(format, args);
            SafeNativeMethods.OutputDebugString(msg ?? string.Empty);
        }

        private static string GetMachineName()
        {
            try
            {
                return GetExternalIP();
            }
            catch (System.Net.Sockets.SocketException)
            {
                // Ignore socket exceptions.
            }
            catch (InvalidOperationException) { }

            return Environment.MachineName;
        }
        public static TimeSpan ConvertToTimeSpan(System.DateTime time)
        {
            return System.TimeZoneInfo.Local.GetUtcOffset(time);
        }
    }
}
