using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using MyCoreLib.Common.Model;
using MyCoreLib.Common.Extensions;
using System.IO;
using System;

namespace MyCoreLib.BasePush.AzureCloud
{
    public class AzureCloudPush : BaseSingleton<AzureCloudPush>, IPush<CloudQueueMessage>
    {
        private CloudStorageAccount m_sAccount;
        private CloudQueueClient m_queueClient;
        private CloudQueue m_queue;
        private string m_queueName = "withdrawqueue";

        public void Configure(string configFile)
        {
            //create a account for an azure queue
            m_sAccount = CloudStorageAccount.Parse(File.ReadAllText(configFile));
            //create a queue;
            m_queueClient = m_sAccount.CreateCloudQueueClient();
            m_queue = m_queueClient.GetQueueReference(m_queueName);
        }

        public MessageItem<CloudQueueMessage> ReadMessage()
        {
            if (CreateQueue())
            {
                var cqMessage = m_queue.GetMessageAsync();
                MessageItem<CloudQueueMessage> _msg = cqMessage.Result.AsString.FromJsonString<MessageItem<CloudQueueMessage>>();
                _msg.OrigMessage = cqMessage.Result;
                return _msg;
            }
            return null;
        }

        public void AddMessage(object messageEntity)
        {
            if (CreateQueue())
            {
                var json = messageEntity.ToJsonString();
                CloudQueueMessage message = new CloudQueueMessage(json);
                m_queue.AddMessageAsync(message);
            }
        }

        private bool CreateQueue()
        {
            if (m_queue == null)
                return false;
            return m_queue.CreateIfNotExistsAsync().Result;
        }

        public void DeleteMessage(MessageItem<CloudQueueMessage> msg)
        {
            if (CreateQueue())
            {
                m_queue.DeleteMessageAsync(msg.OrigMessage);
            }
        }
    }
}
