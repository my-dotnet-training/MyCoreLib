
namespace MyCoreLib.BaseLog.DebugLog
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Import some win32 APIs.
    /// </summary>
    internal static class SafeNativeMethods
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        internal static extern void OutputDebugString(string message);
    }
}
