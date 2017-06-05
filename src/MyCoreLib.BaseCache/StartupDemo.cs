using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using MyCoreLib.BaseCache.CacheManager;
using MyCoreLib.BaseCache.MemoryCache;
using MyCoreLib.BaseCache.RedisCached;
using System;

namespace MyCoreLib.BaseCache
{
    public class StartupDemo
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMemoryCache();
            //if (_cacheProvider._isUseRedis)
            //{
            //    //Use Redis
            //    services.AddSingleton(typeof(ICacheProvider), new CoreRedisCacheProvider(new RedisCacheOptions
            //    {
            //        Configuration = _cacheProvider._connectionString,
            //        InstanceName = _cacheProvider._instanceName
            //    }, 0));
            //}
            //else
            //{
            //    //Use MemoryCache
            //    services.AddSingleton<IMemoryCache>(factory =>
            //    {
            //        var cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());
            //        return cache;
            //    });
            //    services.AddSingleton<ICacheProvider, CoreMemoryCacheProvider>();
            //}
        }
    }
}
