
namespace MyCoreLib.Data.Entity
{
    public interface IExtendedAttributesFactory
    {
        IExtendedAttributes CreateAttributes(ExtendableEntity entity);
        void FillEntity(ExtendableEntity entity, IExtendedAttributes attrs);
        IExtendedAttributes DeserializeJson(string json, Newtonsoft.Json.JsonSerializerSettings settings);
    }
}
