using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseOrm
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class DbTableInfoAttribute : Attribute
    {
        public string TableName;
    }
}
