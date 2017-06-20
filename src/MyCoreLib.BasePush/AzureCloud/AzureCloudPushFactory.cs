using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyCoreLib.BasePush.AzureCloud
{
    public class AzureCloudPushFactory : BasePushFactory
    {
        public AzureCloudPushFactory(string configFile) : base(configFile)
        {
        }

        public override IPush GetPush(string name)
        {
            var _push = AzureCloudPush.Instance;
            _push.Configure(this.ConfigFile);
            return _push;
        }
    }
}
