using System;
using System.Collections.Generic;

namespace MyCoreLib.IoC
{
    public class ReflectionContainer : IIoCContainer
    {
        /// <summary>
        /// 配置实例
        /// </summary>
        private IIoCConfig _config;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">ioc配置</param>
        public ReflectionContainer(IIoCConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public TType Get<TType>(string key)
        {
            Type type;
            var can = _config.KeyDictionary.TryGetValue(key, out type);
            //type = Type.GetType(key);
            if (type != null)
            {
                //反射实例化对象
                return (TType)Activator.CreateInstance(type);
            }
            else
            {
                throw new Exception("未找到对应的类型");
            }
        }

        /// <summary>
        /// 根据接口获取实例对象
        /// </summary>
        /// <typeparam name="TInterface">接口</typeparam>
        /// <returns></returns>
        public TInterface GetInterface<TInterface>()
        {
            Type type;
            var can = _config.ConfigDictionary.TryGetValue(typeof(TInterface), out type);
            if (can)
            {
                //反射实例化对象
                return (TInterface)Activator.CreateInstance(type);
            }
            else
            {
                throw new Exception("未找到对应的类型");
            }
        }
    }
}
