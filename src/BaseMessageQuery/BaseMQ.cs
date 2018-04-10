
using System.Threading;

namespace MyCoreLib.BaseMessageQuery
{
    public interface IBaseMQ
    {
        void InitMQ();
        void Receive();

        void Send(string message);

    }

    public class BaseSingleton<T> where T : class, new()
    {
        protected static readonly object lockHelper = new object();
        private static T _Instance;
        /// <summary>
        /// 获取单例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    //lock (lockHelper)
                    //{
                    //if (_Instance == null)
                    Interlocked.CompareExchange(ref _Instance, new T(), null);
                }

                return _Instance;
            }
        }

        public static void Refresh()
        {
            _Instance = null;
        }
    }
}
