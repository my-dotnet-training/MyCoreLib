
namespace MyCoreLib.BaseData
{
    public class DBFactory
    {
        private static IDBFactoryImpl dbFactoryImplHandler;

        internal static void SetDBFactoryImpl(IDBFactoryImpl impl)
        {
            if (impl != null)
                dbFactoryImplHandler = impl;
            else
                throw new System.ArgumentNullException("Please provide an implementation for the factory, or use the default factory");
        }

        public static IDBFactoryImpl GetFactory()
        {
            if (dbFactoryImplHandler == null)
                System.Threading.Interlocked.CompareExchange(ref dbFactoryImplHandler, (IDBFactoryImpl)new DefaultFactoryImpl(), null);

            return dbFactoryImplHandler;
        }
        
    }
}
