using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace MyCoreLib.BaseMessageQuery
{
    public class ActiveMQ : BaseSingleton<ActiveMQ>, IBaseMQ
    {
        private IConnectionFactory factory;
        private IConnection connection;
        private ISession session;
        private IMessageProducer prod;
        private IMessageConsumer consumer;
        private ITextMessage msg;

        private bool hasSelector = false;
        private const string ClientID = "clientid";
        private const string Selector = "filter='demo'";
        private bool sendSuccess = true;
        private bool receiveSuccess = true;
        private bool isTopic;
        private bool _isLocalMachine;
        private string _remoteAddress;
        /// <summary>  
        /// 初始化  
        /// </summary>  
        /// <param name="topic">选择是否是Topic</param>  
        /// <param name="name">队列名</param>  
        /// <param name="selector">是否设置过滤</param>  
        public bool InitQueueOrTopic(bool topic, string name, bool selector = false)
        {
            try
            {
                //通过会话创建生产者、消费者  
                if (topic)
                {
                    prod = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(name));
                    if (selector)
                    {
                        consumer = session.CreateDurableConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(name), ClientID, Selector, false);
                        hasSelector = true;
                    }
                    else
                    {
                        consumer = session.CreateDurableConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(name), ClientID, null, false);
                        hasSelector = false;
                    }
                    isTopic = true;
                }
                else
                {
                    prod = session.CreateProducer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(name));
                    if (selector)
                    {
                        consumer = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(name), Selector);
                        hasSelector = true;
                    }
                    else
                    {
                        consumer = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue(name));
                        hasSelector = false;
                    }
                    isTopic = false;
                }
                //创建一个发送的消息对象  
                msg = prod.CreateTextMessage();
            }
            catch (System.Exception e)
            {
                sendSuccess = false;
                receiveSuccess = false;
                Console.WriteLine("Exception:{0}", e.Message);
                Console.ReadLine();
                throw e;
            }

            return sendSuccess;
        }
        public ActiveMQ(bool isLocalMachine, string remoteAddress)
        {
            _isLocalMachine = isLocalMachine;
            _remoteAddress = remoteAddress;
            InitMQ();
        }
        public ActiveMQ() : this(true, "localhost")
        {
        }
        public void InitMQ()
        {
            try
            {
                //初始化工厂     
                if (_isLocalMachine)
                {
                    factory = new ConnectionFactory("tcp://localhost:61616/");
                }
                else
                {
                    factory = new ConnectionFactory("tcp://" + _remoteAddress + ":61616/"); //写tcp://192.168.1.111:61616的形式连接其他服务器上的ActiveMQ服务器             
                }
                //通过工厂建立连接  
                connection = factory.CreateConnection();
                connection.ClientId = ClientID;
                connection.Start();
                //通过连接创建Session会话  
                session = connection.CreateSession();
            }
            catch (System.Exception e)
            {
                sendSuccess = false;
                receiveSuccess = false;
                Console.WriteLine("Exception:{0}", e.Message);
                Console.ReadLine();
                throw e;
            }
            Console.WriteLine("Begin connection...");
        }

        public void Receive()
        {
            if (prod == null)
            {
                Console.WriteLine("call InitQueueOrTopic() first!!");
                return;
            }

            Console.WriteLine("Begin receive messages...");
            try
            {
                consumer.Listener += (message) =>
                {
                    if (message == null)
                    {
                        Console.WriteLine("No message received!");
                    }
                    else
                    {
                        Console.WriteLine("Received message with Correlation ID: " + message.NMSCorrelationID);
                        //Console.WriteLine("Received message with Properties'ID: " + revMessage.Properties["MyID"]);  
                        Console.WriteLine("Received message with text: " + (message as ITextMessage).Text);
                    }
                };
                //同步阻塞10ms,没消息就直接返回null,注意此处时间不能设太短，否则还没取到消息就直接返回null了！！！  
                //revMessage = consumer.Receive(new TimeSpan(TimeSpan.TicksPerMillisecond * 10)) as ITextMessage;
            }
            catch (System.Exception e)
            {
                receiveSuccess = false;
                Console.WriteLine("Exception:{0}", e.Message);
                Console.ReadLine();
                throw e;
            }
        }

        public void Send(string message)
        {
            Send(message, "defult", MsgPriority.Normal, true);
        }

        public void Send(string message, string msgId = "defult", MsgPriority priority = MsgPriority.Normal)
        {
            if (prod == null)
            {
                sendSuccess = false;
                Console.WriteLine("call InitQueueOrTopic() first!!");
                return;
            }

            Console.WriteLine("Begin send messages...");

            //给这个对象赋实际的消息  
            msg.NMSCorrelationID = msgId;
            msg.Properties["MyID"] = msgId;
            msg.NMSMessageId = msgId;
            msg.Text = message;
            Console.WriteLine(message);

            if (isTopic)
            {
                sendSuccess = ProducerSubcriber(message, priority);
            }
            else
            {
                sendSuccess = P2P(message, priority);
            }
        }


        /// <summary>
        /// P2P模式，一个生产者对应一个消费者  
        /// </summary>
        /// <param name="message"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        private bool P2P(string message, MsgPriority priority)
        {
            try
            {
                if (hasSelector)
                {
                    //设置消息对象的属性，这个很重要，是Queue的过滤条件，也是P2P消息的唯一指定属性  
                    msg.Properties.SetString("filter", "demo");  //P2P模式  
                }
                prod.Priority = priority;
                //设置持久化  
                prod.DeliveryMode = MsgDeliveryMode.Persistent;
                //生产者把消息发送出去，几个枚举参数MsgDeliveryMode是否持久化，MsgPriority消息优先级别，存活时间，当然还有其他重载  
                prod.Send(msg, MsgDeliveryMode.Persistent, priority, TimeSpan.MinValue);
            }
            catch (System.Exception e)
            {
                sendSuccess = false;
                Console.WriteLine("Exception:{0}", e.Message);
                Console.ReadLine();
                throw e;
            }

            return sendSuccess;
        }


        /// <summary>
        /// 发布订阅模式，一个生产者多个消费者
        /// </summary>
        /// <param name="message"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        private bool ProducerSubcriber(string message, MsgPriority priority)
        {
            try
            {
                prod.Priority = priority;
                //设置持久化,如果DeliveryMode没有设置或者设置为NON_PERSISTENT，那么重启MQ之后消息就会丢失  
                prod.DeliveryMode = MsgDeliveryMode.Persistent;
                prod.Send(msg, Apache.NMS.MsgDeliveryMode.Persistent, priority, TimeSpan.MinValue);
                //System.Threading.Thread.Sleep(1000);    
            }
            catch (System.Exception e)
            {
                sendSuccess = false;
                Console.WriteLine("Exception:{0}", e.Message);
                Console.ReadLine();
                throw e;
            }

            return sendSuccess;
        }

        public void ShutDown()
        {
            Console.WriteLine("Close connection and session...");
            session.Close();
            connection.Close();
        }


        static void MQTest()
        {
            ActiveMQ.Instance.InitQueueOrTopic(topic: false, name: "myqueue", selector: false);
            //mymq.InitQueueOrTopic(topic: false, name: "seletorqueue", selector: true);   
            //mymq.InitQueueOrTopic(topic: true, name: "noselectortopic", selector: false);  
            //mymq.InitQueueOrTopic(topic: true, name: "selectortopic", selector: true);  

            ActiveMQ.Instance.Send("{'0000', 'Lowest', 'img / p.jpg'}", "newid", priority: Apache.NMS.MsgPriority.Lowest);
            ActiveMQ.Instance.Receive();
            ActiveMQ.Instance.ShutDown();

            
        }
    }
}
