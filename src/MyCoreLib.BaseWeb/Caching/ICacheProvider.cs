
namespace MyCoreLib.BaseWeb.Caching
{
    /// <summary>
    /// The very basic method for each cache provider.
    /// </summary>
    public interface ICacheProvider : ICacheManager
    {
        object GetData(string key);
        void CacheData(int entityType, string key, object data);
    }
}
