﻿namespace MyCoreLib.Data.Entity
{
    /// <summary>
    /// Support reading extended attributes for this entity.
    /// </summary>
    public interface IExtendableEntity
    {
        string GetAttributesAsString();
    }
}
