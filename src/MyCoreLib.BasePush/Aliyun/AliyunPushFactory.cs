using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BasePush.Aliyun
{
    public class AliyunPushFactory : BasePushFactory
    {
        public AliyunPushFactory(string configFile) : base(configFile)
        {
        }

        public override IPush GetPush(string name)
        {
            var _push = AliyunPush.Instance;
            _push.Configure(this.ConfigFile);
            return _push;
        }
    }
}
