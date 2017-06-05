
namespace MyCoreLib.BaseLog.FileLog.Args
{
    /// <summary>
    /// Specifies whether an argument swich require a value.
    /// </summary>
    public enum ArgRequiredValue
    {
        /// <summary>
        /// The argument switch requires a value.
        /// </summary>
        Yes,

        /// <summary>
        /// The argument switch does not require a value.
        /// </summary>
        No,

        /// <summary>
        /// A value is optional for an argument switch.
        /// </summary>
        Optional,
    }
}