using MyCoreLib.Common.Provider;
using MyCoreLib.BaseLog;
using MyCoreLib.BaseLog.DBLog;
using System.Linq;

namespace MyCoreLib.CommandTest
{
    public class LogProviderDemo
    {
        public ILoggerProvider SystemDBLog()
        {
            //ProviderKey demoKey = ProviderKeyManager.Providers["LogDemo"];
            //if (demoKey == null)
            //    demoKey = ProviderKeyManager.AddProvider<SystemDBLoggFactory>("LogDemo");
            ProviderFactoryInfo _pf = ProviderFactoryInfoManager.ProviderFactoryInfos["SystemDBLog"];
            if (_pf == null)
                _pf = ProviderFactoryInfoManager.AddProviderFactoryInfo<SystemDBLogFactory>("SystemDBLog");
            return _pf.ExportFactory.CreateExport<SystemDBLogFactory>();
        }
    }
}
