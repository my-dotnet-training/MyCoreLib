using System;

namespace MyCoreLib.Data.Exceptions
{
    /// <summary>
    /// The exception will be throw once an entity doesn't exist.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        { }

        public EntityNotFoundException(string message)
            : base(message)
        { }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
