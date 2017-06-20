namespace MyCoreLib.WeixinSDK.Entiyies.Message.Request
{
    public abstract class RequestMessageBase : BaseMessage, IRequestMessageBase
    {
        public long MsgId { get; set; }
    }
}
