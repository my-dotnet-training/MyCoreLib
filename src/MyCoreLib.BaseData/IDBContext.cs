
namespace MyCoreLib.BaseData
{
    public interface IDBContext
    {
        IDBInstance GetDatabaseInstance(DBInstance type);
    }
}
