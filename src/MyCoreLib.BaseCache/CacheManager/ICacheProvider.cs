
using System.Threading.Tasks;

namespace MyCoreLib.BaseCache.CacheManager
{
    /// <summary>
    /// The very basic method for each cache provider.
    /// </summary>
    public interface ICacheProvider : ICacheManager
    {
        #region sync
        T GetData<T>(string key);

        bool CacheData<T>(int entityType, string key, T data);

        bool RemoveData(string key);

        bool ExistsData(string key);
        #endregion

        #region async
        T GetDataAsync<T>(string key);
        Task<bool> CacheDataAsync<T>(int entityType, string key, T data);
        Task<bool> RemoveDataAsync(string key);
        Task<bool> ExistsDataAsync(string key);
        #endregion
    }
}
