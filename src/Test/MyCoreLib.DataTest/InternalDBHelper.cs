using MyCoreLib.BaseOrm.DbHelper;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace MyCoreLib.DataTest
{
    public class InternalDBHelper : DbHelper
    {
        public InternalDBHelper()
            : base(SqlClientFactory.Instance, "fake connection string")
        { }

        public string CreateParameterName_(int index)
        {
            return base.CreateParameterName(index);
        }

        public void FillFromReader_(DbDataReader reader, int startRecord, int maxRecords, Action<DbDataReader> action)
        {
            FillFromReader(reader, startRecord, maxRecords, action);
        }

        protected override void OnExecuteCommand(DbCommand command)
        {
            OnExecuteCommand_(command);
        }

        public void OnExecuteCommand_(DbCommand command)
        {
            base.OnExecuteCommand(command);
        }
    }
}
