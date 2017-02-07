using System;
using System.Reflection;
using System.Reflection.Emit;

namespace MyCoreLib.IoC
{
    public class EmitContainer : IIoCContainer
    {
        /// <summary>
        /// 配置实例
        /// </summary>
        private IIoCConfig _config;

        public EmitContainer(IIoCConfig config)
        {
            _config = config;
        }

        public TType Get<TType>(string key)
        {
            Type type;
            var can = _config.KeyDictionary.TryGetValue(key, out type);
            if (can)
            {
                BindingFlags defaultFlags = BindingFlags.Public | BindingFlags.Instance;
                var constructors = type.GetConstructors(defaultFlags);//获取默认构造函数
                var t = (TType)this.CreateInstanceByEmit(constructors[0]);
                return t;
            }
            else
            {
                throw new Exception("未找到对应的类型");
            }
        }

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <typeparam name="TInterface">接口</typeparam>
        /// <returns></returns>
        public TInterface GetInterface<TInterface>()
        {
            Type type;
            var can = _config.ConfigDictionary.TryGetValue(typeof(TInterface), out type);
            if (can)
            {
                BindingFlags defaultFlags = BindingFlags.Public | BindingFlags.Instance;
                var constructors = type.GetConstructors(defaultFlags);//获取默认构造函数
                var t = (TInterface)this.CreateInstanceByEmit(constructors[0]);
                return t;
            }
            else
            {
                throw new Exception("未找到对应的类型");
            }
        }
        /// <summary>
        /// 实例化对象 用EMIT
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constructor"></param>
        /// <returns></returns>
        private Object CreateInstanceByEmit(ConstructorInfo constructor)
        {
            //动态方法
            var dynamicMethod = new DynamicMethod(Guid.NewGuid().ToString("N"), typeof(Object), new[] { typeof(object[]) }, true);
            //方法IL
            ILGenerator il = dynamicMethod.GetILGenerator();
            //实例化命令
            il.Emit(OpCodes.Newobj, constructor);
            //如果是值类型装箱
            if (constructor.DeclaringType.GetTypeInfo().IsValueType)
                il.Emit(OpCodes.Box, constructor.DeclaringType);
            //返回
            il.Emit(OpCodes.Ret);
            //用FUNC去关联方法
            var func = (Func<Object>)dynamicMethod.CreateDelegate(typeof(Func<Object>));
            //执行方法
            return func.Invoke();
        }

    }
}
