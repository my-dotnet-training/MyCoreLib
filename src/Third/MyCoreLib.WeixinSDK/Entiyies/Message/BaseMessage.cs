using MyCoreLib.WeixinSDK.Enums;

namespace MyCoreLib.WeixinSDK.Entiyies.Message
{
    public class BaseMessage : IMessageBase
    {
        public string Title { get; set; }
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public string CreateTime { get; set; }
        public WeixinMessageType MsgType { get; set; }
        public string Context { get; set; }
        
        public override string ToString()
        {
            //TODO:直接输出XML
            return base.ToString();
        }
    }
}
