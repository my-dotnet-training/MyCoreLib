
namespace MyCoreLib.BaseData.Exceptions
{
    using System;

    /// <summary>
    /// The exception will occur once a database instance wasn't found.
    /// </summary>
    public class DBNotFoundException : Exception
    {
        public DBNotFoundException(string message)
            : base(message)
        { }
    }
}
