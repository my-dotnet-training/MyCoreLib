using System;

namespace MyCoreLib.BaseLog.FileLog.Args
{
    /// <summary>
    /// Specifies the description for an enum member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ArgEnumValueDescriptionAttribute : Attribute
    {
        public ArgEnumValueDescriptionAttribute(string description)
        {
            Description = description;
        }

        public string Description
        {
            get;
            set;
        }
    }
}