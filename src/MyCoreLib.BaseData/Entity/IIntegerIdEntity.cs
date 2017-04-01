namespace MyCoreLib.BaseData.Entity
{
    /// <summary>
    /// Defines an interface for entities who have an integer id.
    /// </summary>
    public interface IIntegerIdEntity
    {
        int Id { get; }
        string Name { get; }
    }
}