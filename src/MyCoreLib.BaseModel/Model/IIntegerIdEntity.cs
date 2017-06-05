using System;

namespace MyCoreLib.BaseModel.Model
{
    /// <summary>
    /// Defines an interface for entities who have an integer id.
    /// </summary>
    public interface IIntegerIdEntity
    {
        int Id { get; }
        string Name { get; }
    }

    public class IntegerIdEntity : IIntegerIdEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
