
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace MyCoreLib.BaseWeb.Extensions
{

    public static class HttpExtensions
    {
        /// <summary>
        /// Get ip address of the current request.
        /// </summary>
        public static string GetClientIP(this HttpRequest request)
        {
            string ipAddr = request.Form["X_FORWARDED_FOR"];
            return string.IsNullOrWhiteSpace(ipAddr) ? request.Host.ToString() : ipAddr;
        }
    }
}
