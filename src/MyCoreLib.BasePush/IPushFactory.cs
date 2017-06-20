
namespace MyCoreLib.BasePush
{
    public interface IPushFactory
    {
        /// <summary>
        /// Gets the log by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        IPush GetPush(string name);
    }
}
