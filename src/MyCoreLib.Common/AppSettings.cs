
using System.Runtime.InteropServices;

namespace MyCoreLib.Common
{
    public class AppSettings
    {
        /// <summary>
        /// CharSet
        /// </summary>
        public static CharSet DefaultCharSet = CharSet.Unicode;

        /// <summary>
        /// AppName
        /// </summary>
        public static string AppName { get; set; }

        private static string m_ConnectionString = "server=localhost; database=weipandb_unittest; username=root; password=MySql2015;";
        /// <summary>
        /// Connection string
        /// </summary>
        public static string ConnectionString { get { return m_ConnectionString; } set { m_ConnectionString = value; } }

        public static bool Antiforgery { get; set; }
        /// <summary>
        /// HttpRuntime.AppDomainAppVirtualPath
        /// </summary>
        public static string AppDomainAppVirtualPath { get; set; }
        /// <summary>
        /// AppDomain.CurrentDomain.FriendlyName
        /// </summary>
        public static string CurrentDomain_FriendlyName { get; set; }

    }
}
