using System;

namespace MyCoreLib.BaseLog.FileLog.Args
{
    /// <summary>
    /// The exception will be thrown if invalid or unknown argument is found.
    /// </summary>
    public class InvalidArgException : Exception
    {
        public InvalidArgException(string name)
            : this(name, string.Format("Invalid command-line argument: '{0}'.", name))
        { }

        public InvalidArgException(string name, string message)
            : this(name, message, null)
        { }

        public InvalidArgException(string name, string message, Exception innerException)
            : base(message, innerException)
        {
            ArgName = name;
        }

        /// <summary>
        /// Gets or sets the name of the argument.
        /// </summary>
        public string ArgName
        {
            get;
            private set;
        }
    }
}