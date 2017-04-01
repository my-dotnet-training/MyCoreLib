using System;
using System.Collections.Generic;

namespace MyCoreLib.BaseIoC
{
    public class IoCContainerManager
    {
        /// <summary>
        /// 容器
        /// </summary>
        private static IIoCContainer _container;

        /// <summary>
        /// 获取IOC容器
        /// </summary>
        /// <param name="config">ioc配置</param>
        /// <returns></returns>
        public static IIoCContainer GetIoCContainer(IIoCConfig config)
        {

            if (_container == null)
            {
                if (config.IoCType == IoCType.Reflect)
                    //反射方式
                    _container = new ReflectionContainer(config);
                else
                    //EMIT方式
                    _container = new EmitContainer(config);
            }
            return _container;

        }
    }
}
