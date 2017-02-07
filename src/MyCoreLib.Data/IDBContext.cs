
namespace MyCoreLib.Data
{
    public interface IDBContext
    {
        IDBInstance GetDatabaseInstance(DBInstance type);
    }
}
