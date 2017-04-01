
namespace MyCoreLib.BaseData
{
    /// <summary>
    /// Specifies the basic information for a database instance.
    /// </summary>
    public interface IDBInstance
    {
        string Name { get; }
        DBInstance DbType { get; }
        string Server { get; }
        string DatabaseName { get; }
        bool Enabled { get; }
    }
}
