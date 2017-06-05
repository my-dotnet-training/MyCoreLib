using System;
using System.Collections.Generic;

namespace MyCoreLib.BaseLog.FileLog.Args
{
    /// <summary>
    /// Specifies how <see cref="ArgParser"/> reads the environment variables.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ArgAttribute : Attribute
    {
        #region constructors
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        public ArgAttribute(string name)
            : this(name, null)
        { }

        public ArgAttribute(string name, string shortName)
            : this(name, shortName, null, ArgRequiredValue.No, true)
        { }

        public ArgAttribute(string name, string shortName, string description)
            : this(name, shortName, description, ArgRequiredValue.No, true)
        { }

        public ArgAttribute(string name, string shortName, string description, ArgRequiredValue requiredValue)
            : this(name, shortName, description, requiredValue, true)
        { }

        public ArgAttribute(string name, string shortName, string description, ArgRequiredValue requiredValue, bool visible)
        {
            Name = name;
            ShortName = shortName;
            Description = description;
            RequiredValue = requiredValue;
            Visible = visible;
        }
        #endregion constructors

        /// <summary>
        /// Gets the name of the argument.
        /// </summary>
        /// <remarks>
        /// The name can be composed of alphabetic and numeric characters, or underscores.
        /// </remarks>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the short name.
        /// </summary>
        /// <remarks>
        /// The name can be composed of alphabetic and numeric characters, or underscores.
        /// </remarks>
        public string ShortName
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether the associated argument switch requires a value.
        /// </summary>
        public ArgRequiredValue RequiredValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description of the argument.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the argument is required or not.
        /// </summary>
        public bool Required
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether the argument is visible in usage information.
        /// </summary>
        public bool Visible
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Compares 2 <see cref="ArgAttribute"/> instances.
    /// </summary>
    public class ArgAttributeComparer : IComparer<ArgAttribute>
    {
        public int Compare(ArgAttribute x, ArgAttribute y)
        {
            if (x == null || y == null)
            {
                return -1;
            }

            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            return string.Compare(x.Name, y.Name);
        }
    }
}
