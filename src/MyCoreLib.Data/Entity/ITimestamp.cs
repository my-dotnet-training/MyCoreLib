using System;

namespace MyCoreLib.Data.Entity
{
    public interface ITimestamp
    {
        /// <summary>
        ///  Get the date and time when the entity was created.
        /// </summary>
        DateTime TimeCreated { get; }
    }
}
