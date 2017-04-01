
namespace MyCoreLib.BaseIoC
{
    public interface IIoCContainer
    {
        /// <summary>
        /// 根据接口返回对应的实例
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        TInterface GetInterface<TInterface>();

        /// <summary>
        /// 根据key返回对应的实例
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        TType Get<TType>(string key);
    }
}
