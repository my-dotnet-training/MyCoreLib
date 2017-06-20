using MyCoreLib.WeixinSDK.Enums;

namespace MyCoreLib.WeixinSDK.Entiyies
{
    public class WeixinMessage
    {
        public virtual WeixinMessageType Type { set; get; }
        public virtual dynamic Body { set; get; }
    }
}
