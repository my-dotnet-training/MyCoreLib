using MyCoreLib.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.BasePush.Aliyun
{
    public class AliyunPush : BaseSingleton<AliyunPush>, IPush
    {
        public void Configure(string configFile)
        {
        }
        public void AddMessage<T>(T messageEntity)
        {
        }

        public string ReadMessage()
        {
            return string.Empty;
        }
    }
}
