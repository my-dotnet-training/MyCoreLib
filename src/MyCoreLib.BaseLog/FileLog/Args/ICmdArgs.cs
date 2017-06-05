
namespace MyCoreLib.BaseLog.FileLog.Args
{
    public interface ICmdArgs
    {
        /// <summary>
        /// Get or set a value indicating whether to display usage information.
        /// </summary>
        bool ShowUsage
        {
            get;
            set;
        }

        void Usage(string errorInfo);

        /// <summary>
        /// Validate the arguments.
        /// </summary>
        void Validate();

        /// <summary>
        /// Process any free-form command-line arguments.
        /// </summary>
        void ProcessStandAloneArgument(string arg);
    }
}