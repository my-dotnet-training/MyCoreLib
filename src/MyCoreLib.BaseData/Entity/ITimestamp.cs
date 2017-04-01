using System;

namespace MyCoreLib.BaseData.Entity
{
    public interface ITimestamp
    {
        /// <summary>
        ///  Get the date and time when the entity was created.
        /// </summary>
        DateTime TimeCreated { get; }
    }
}
