using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.Common.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets the value by key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static T GetValue<T>(this IDictionary<object, object> dictionary, object key)
            where T : new()
        {
            T defaultValue = default(T);
            return GetValue<T>(dictionary, key, defaultValue);
        }

        /// <summary>
        /// Gets the value by key and default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T GetValue<T>(this IDictionary<object, object> dictionary, object key, T defaultValue)
        {
            object valueObj;

            if (!dictionary.TryGetValue(key, out valueObj))
                return defaultValue;
            else
                return (T)valueObj;
        }

        /// <summary>
        /// get item from dictionary by index
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Tuple<TKey, TValue> GetItemByIndex<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, int index)
        {
            TKey _key = new List<TKey>(dictionary.Keys)[index];
            TValue _value = new List<TValue>(dictionary.Values)[index];
            return new Tuple<TKey, TValue>(_key, _value);

        }
    }
}
