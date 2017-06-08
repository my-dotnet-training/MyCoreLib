using MyCoreLib.BaseOrm.DbHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BaseOrm.Aop
{
    public class DbColumnInfo<TProrerty>
    {
        public string Name { get; set; }

        public bool IsIndexKey { get; set; }
        public IndexKeyType IndexType { get; set; }

        /// <summary>
        /// if i want make this column to a where condition, set this property to true
        /// </summary>
        public bool IsWhere { get; set; }
        public TProrerty WhereValue { get; set; }

        /// <summary>
        /// if i want to set this column value, set this property to true
        /// </summary>
        public bool IsSetValue { get; set; }
        public TProrerty Value { get; set; }

        /// <summary>
        /// if i want to ruery this column value, set this property to true
        /// </summary>
        public bool IsQuery { get; set; }

        public void ClearCondition()
        {
            this.IsIndexKey = false;
            this.IsWhere = false;
            this.IsSetValue = false;
            this.IsQuery = false;

            this.WhereValue = default(TProrerty);
            this.Value = default(TProrerty);
            this.IndexType = IndexKeyType.None;
        }
    }
}
