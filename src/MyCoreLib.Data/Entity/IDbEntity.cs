
using System.Data;

namespace MyCoreLib.Data.Entity
{
    public interface IDbEntity
    {
        /// <summary>
        /// Fill in the current instance using the information read from the specified data reader.
        /// </summary>
        void ReadFromDataReader(IDataReader dr);
    }
}
