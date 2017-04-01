using System;
using System.Collections.Generic;
using System.Reflection;

namespace MyCoreLib.BaseIoC
{
    public class IoCConfig : IIoCConfig
    {
        /// <summary>
        /// 存放配置的字典对象，KEY是接口类型，VALUE是实现接口的类型
        /// </summary>
        private Dictionary<Type, Type> _configDictionary = new Dictionary<Type, Type>();
        public Dictionary<Type, Type> ConfigDictionary
        {
            get
            {
                return _configDictionary;
            }
        }

        private Dictionary<string, Type> _keyDictionary = new Dictionary<string, Type>();
        public Dictionary<string, Type> KeyDictionary
        {
            get
            {
                return _keyDictionary;
            }
        }
        
        private IoCType _iocType = IoCType.Reflect;
        public IoCType IoCType
        {
            get
            {
                return _iocType;
            }
        }

        public void AddConfig<TInterface, TType>()
        {
            //判断TType是否实现TInterface
            if (typeof(TInterface).IsAssignableFrom(typeof(TType)))
            {
                _configDictionary.Add(typeof(TInterface), typeof(TType));
            }
            else
            {
                throw new Exception("类型未实现接口");
            }
        }
        public void AddConfig<TType>(string key)
        {
            _keyDictionary.Add(key, typeof(TType));
        }

        private IoCConfig(IoCType iocType = IoCType.Reflect)
        {
            _iocType = iocType;
        }

        public static IoCConfig ReadConfig(string configFile)
        {
            return new IoCConfig();
        }

    }
}
