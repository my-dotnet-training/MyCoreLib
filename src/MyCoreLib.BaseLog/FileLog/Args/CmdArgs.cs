
namespace MyCoreLib.BaseLog.FileLog.Args
{
    using System;

    public abstract class CmdArgs : ICmdArgs
    {
        public bool ShowUsage { get; set; }

        public void Usage(string errorInfo)
        {
            if (!string.IsNullOrEmpty(errorInfo))
            {
                Console.WriteLine(errorInfo);
            }
            ArgParser.Usage(this);
        }

        public virtual void Validate()
        {
            // Do nothing in this class.
        }

        public virtual void ProcessStandAloneArgument(string arg)
        {
            // Not allow standalone arguments in the base class.
            throw new InvalidArgException(arg);
        }
    }
}
