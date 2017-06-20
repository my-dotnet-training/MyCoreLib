
using System;

namespace MyCoreLib.BasePush
{

    public interface IPush<TQueueMessage>
    {
        void Configure(string configFile);
        MessageItem<TQueueMessage> ReadMessage();
        void AddMessage(object messageEntity);
        void DeleteMessage(MessageItem<TQueueMessage> msg);
    }
}
