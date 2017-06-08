using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class DbColumnInfoAttribute: Attribute
    {
        public int ColumnIndex { get; set; }
        public string ColumnName { get; set; }

        public bool IsIndexKey { get; set; }
        public IndexKeyType IndexType { get; set; }
        
        public object Value { get; set; }
        
    }
}
