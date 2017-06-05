
namespace MyCoreLib.BaseLog
{
    /// <summary>
    /// IMPORTANT: the length of a tag is not allowed to exceed 32.
    /// </summary>
    public static class SystemLogTag
    {
        public const string SiteToken = "sitetoken";
        public const string User = "user #";
        public const string Withdraw = "withdraw";
        public const string Notification = "notification";
        public const string Mgmt = "mgmt";
        public const string OrderProcessing = "order";
    }
}
