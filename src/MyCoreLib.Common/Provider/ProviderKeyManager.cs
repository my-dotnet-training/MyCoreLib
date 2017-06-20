using MyCoreLib.Common.Model;
using System;
using System.Collections.Concurrent;
using MyCoreLib.Common.Extensions;

namespace MyCoreLib.Common.Provider
{
    public class ProviderKeyManager : BaseClassIndexable<ProviderKey>
    {
        public static ConcurrentDictionary<string, ProviderKey> Providers { get; private set; }
        static ProviderKeyManager()
        {
            Providers = new ConcurrentDictionary<string, ProviderKey>();
        }
        public static ProviderKey AddProvider<TInterface>(string key)
        {
            var _provider = new ProviderKey { Name = key, Type = typeof(TInterface) };
            return Providers.AddOrUpdate(key, _provider, (name, oldValue) => oldValue = _provider);
        }
        public static ProviderKey RemoveProvider(string key)
        {
            ProviderKey provider;
            Providers.TryRemove(key, out provider);
            return provider;
        }
        public override ProviderKey CallByIndex(int index)
        {
            return Providers.GetItemByIndex(index).Item2;
        }
        public override ProviderKey CallByKey(string key)
        {
            return Providers[key];
        }
        public override void SetByIndex(int index, ProviderKey value)
        {
            Providers[Providers.GetItemByIndex(index).Item1] = value;
        }
        public override void SetByKey(string key, ProviderKey value)
        {
            Providers[key] = value;
        }
    }
}
