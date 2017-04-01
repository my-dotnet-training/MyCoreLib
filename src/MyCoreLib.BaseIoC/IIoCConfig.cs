using System;
using System.Collections.Generic;

namespace MyCoreLib.BaseIoC
{
    public interface IIoCConfig
    {
        IoCType IoCType { get; }
        /// <summary>
        /// 添加配置
        /// </summary>
        /// <typeparam name="TInterface">接口</typeparam>
        /// <typeparam name="TType">实现接口的类型</typeparam>
        void AddConfig<TInterface, TType>();

        Dictionary<Type, Type> ConfigDictionary { get; }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="key"></param>
        void AddConfig<TType>(string key);
        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, Type> KeyDictionary { get; }
    }
}
