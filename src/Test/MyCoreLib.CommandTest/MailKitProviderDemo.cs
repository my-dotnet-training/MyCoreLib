using MyCoreLib.BaseMail;
using MyCoreLib.BaseMail.MailKit;
using MyCoreLib.Common.Provider;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.CommandTest
{
    public class MailKitProviderDemo
    {
        public IMailProvider MailKitProvider()
        {
            ProviderFactoryInfo _pf = ProviderFactoryInfoManager.ProviderFactoryInfos["MailKit"];
            if (_pf == null)
                _pf = ProviderFactoryInfoManager.AddProviderFactoryInfo<MailKitProvider>("MailKit");
            return _pf.ExportFactory.CreateExport<MailKitProvider>();
        }
    }
}
