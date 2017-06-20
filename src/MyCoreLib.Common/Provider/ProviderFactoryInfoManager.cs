using MyCoreLib.Common.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using MyCoreLib.Common.Extensions;

namespace MyCoreLib.Common.Provider
{
    public class ProviderFactoryInfoManager : BaseClassIndexable<ProviderFactoryInfo>
    {
        public static ConcurrentDictionary<string, ProviderFactoryInfo> ProviderFactoryInfos { get; private set; }
        static ProviderFactoryInfoManager()
        {
            ProviderFactoryInfos = new ConcurrentDictionary<string, ProviderFactoryInfo>();
        }

        public static ProviderFactoryInfo AddProviderFactoryInfo<TProvider>(string key)
        {
            ProviderKey providerKey = ProviderKeyManager.Providers[key];
            if (providerKey == null)
                providerKey = ProviderKeyManager.AddProvider<TProvider>(key);

            ProviderFactoryInfo _pf = new ProviderFactoryInfo(providerKey, key, typeof(TProvider));
            return ProviderFactoryInfos.AddOrUpdate(key, _pf, (name, oldValue) => oldValue = _pf);
        }
        public static ProviderFactoryInfo RemoveProvider(string key)
        {
            ProviderFactoryInfo providerInfo;
            ProviderFactoryInfos.TryRemove(key, out providerInfo);
            return providerInfo;
        }
        public override ProviderFactoryInfo CallByIndex(int index)
        {
            return ProviderFactoryInfos.GetItemByIndex(index).Item2;
        }
        public override ProviderFactoryInfo CallByKey(string key)
        {
            return ProviderFactoryInfos[key];
        }
        public override void SetByIndex(int index, ProviderFactoryInfo value)
        {
            ProviderFactoryInfos[ProviderFactoryInfos.GetItemByIndex(index).Item1] = value;
        }
        public override void SetByKey(string key, ProviderFactoryInfo value)
        {
            ProviderFactoryInfos[key] = value;
        }
    }
}
