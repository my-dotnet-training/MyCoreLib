
namespace MyCoreLib.BaseWeb.Caching
{
    public class HttpContextCacheProvider : ICacheProvider
    {
        public object GetData(string key)
        {
            
            return HttpContext.Current.Items[key];
        }

        public void CacheData(int entityType, string key, object data)
        {
            HttpContext.Current.Items.Remove(key);
        }

        public void ClearCache()
        {
            // We can't simply call HttpContext.Current.Items.Clear() to remove all cached data from HttpContext,
            // because MgmtContext is also stored there. If this is really required, we will need to come up with
            // other solutions.
            //
        }
    }
}
