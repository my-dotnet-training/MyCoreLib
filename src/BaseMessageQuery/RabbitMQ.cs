using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MyCoreLib.BaseMessageQuery
{
    public class RabbitMQ : BaseSingleton<RabbitMQ>, IBaseMQ
    {
        private string QueueName = "MY_MQ";
        public RabbitMQ()
        {
            InitMQ();
        }
        private static ConnectionFactory factory;

        public void InitMQ()
        {
            //1.创建基于本地的连接工厂
            factory = new ConnectionFactory();
            factory.HostName = "127.0.0.1";
            //默认端口
            factory.Port = 5672;
        }
        public void Receive()
        {
            //2. 建立连接
            using (IConnection conn = factory.CreateConnection())
            {
                //3. 创建信道
                using (IModel channel = conn.CreateModel())
                {
                    //4. 申明队列
                    channel.QueueDeclare(QueueName, false, false, false, null);

                    //输入1，那如果接收一个消息，但是没有应答，则客户端不会收到下一个消息
                    //设置prefetchCount : 1来告知RabbitMQ，在未收到消费端的消息确认时，不再分发消息，也就确保了当消费端处于忙碌状态时，不再分配任务。
                    channel.BasicQos(0, 1, false);

                    Console.WriteLine("Listening...");

                    //5. 构造消费者实例
                    var consumer = new EventingBasicConsumer(channel);
                    //6. 绑定消息接收后的事件委托
                    consumer.Received += (model, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body);
                        Console.WriteLine(" [x] Received {0}", message);
                        //Thread.Sleep(20000);//模拟耗时
                        Console.WriteLine(" [x] Done");
                        // 7. 发送消息确认信号（手动消息确认）
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    };
                    //8. 启动消费者
                    //autoAck:true；自动进行消息确认，当消费端接收到消息后，就自动发送ack信号，不管消息是否正确处理完毕
                    //autoAck:false；关闭自动消息确认，通过调用BasicAck方法手动进行消息确认
                    channel.BasicConsume(queue: "hello", autoAck: false, consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                }
            }
        }
        public void Send(string message)
        {
            //2. 建立连接
            using (IConnection conn = factory.CreateConnection())
            {
                //3. 创建频道
                using (IModel channel = conn.CreateModel())
                {
                    //4. 申明队列(指定durable:true,告知rabbitmq对消息进行持久化)
                    channel.QueueDeclare(QueueName, true, false, false, null);
                    while (true)
                    {
                        IBasicProperties properties = channel.CreateBasicProperties();
                        //将消息标记为持久性 - 将IBasicProperties.SetPersistent设置为true
                        properties.Persistent = true;
                        properties.DeliveryMode = 2;
                        //5. 构建byte消息数据包
                        byte[] buffer = Encoding.UTF8.GetBytes(message);
                        //6. 发送数据包
                        channel.BasicPublish("RabbitTest", QueueName, properties, buffer);
                        Console.WriteLine("消息发送成功：" + message);
                    }
                }
            }
        }
        public void EmitLog(string message)
        {
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    // 生成随机队列名称
                    var queueName = channel.QueueDeclare().QueueName;
                    //使用fanout exchange type，指定exchange名称
                    channel.ExchangeDeclare(exchange: "logs", type: "fanout");

                    var body = Encoding.UTF8.GetBytes(message);
                    //发布到指定exchange，fanout类型无需指定routingKey
                    channel.BasicPublish(exchange: "logs", routingKey: "", basicProperties: null, body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        public void ReceiveLog()
        {
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //申明exchange
                    channel.ExchangeDeclare(exchange: "logs", type: "fanout");
                    //申明随机队列名称
                    var queuename = channel.QueueDeclare().QueueName;
                    //绑定队列到指定exchange,使用默认路由
                    channel.QueueBind(queue: queuename, exchange: "logs", routingKey: "");
                    Console.WriteLine("[*] Waitting for logs.");
                    //申明consumer
                    var consumer = new EventingBasicConsumer(channel);
                    //绑定消息接收后的事件委托
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine("[x] {0}", message);

                    };

                    channel.BasicConsume(queue: queuename, autoAck: true, consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
        public void EmitLogDirect(string message, string level = "info")
        {
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs",
                    type: "direct");

                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "direct_logs",
                    routingKey: level,
                    basicProperties: null,
                    body: body);
                Console.WriteLine(" [x] Sent '{0}':'{1}'", level, message);
            }
        }
        /// <summary>
        /// Use one of parameters: [info] [warning] [error]
        /// </summary>
        /// <param name="logLevels"></param>
        public void ReceiveLogsDirect(string[] args)
        {
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs",
                                        type: "direct");
                var queueName = channel.QueueDeclare().QueueName;

                if (args.Length < 1)
                {
                    Console.Error.WriteLine("Use one of parameters: [info] [warning] [error]");
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                    Environment.Exit(1);
                    return;
                }

                foreach (var logLevel in args)
                {
                    channel.QueueBind(queue: queueName,
                                      exchange: "direct_logs",
                                      routingKey: logLevel);
                }

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received '{0}':'{1}'", routingKey, message);

                    channel.BasicAck(ea.DeliveryTag, false);
                };

                channel.BasicConsume(queue: queueName,
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
        public void EmitLogTopic(string message, string routingKey = "anonymous.info")
        {
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "topic_logs", routingKey: routingKey, body: body);
                    Console.WriteLine("[x] Sent '{0}':'{1}'", routingKey, message);
                }
            }
        }
        public void ReceiveLogsTopic(string[] args)
        {
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");
                    var queuename = channel.QueueDeclare().QueueName;
                    if (args.Length < 1)
                    {
                        Console.Error.WriteLine("Specify binding_key");
                        Environment.Exit(1);
                        return;
                    }
                    foreach (var bindingKey in args)
                    {
                        channel.QueueBind(queue: queuename, exchange: "topic_logs", routingKey: bindingKey);
                    }

                    Console.WriteLine("[*] Waiting for message.");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        var routingKey = ea.RoutingKey;
                        Console.WriteLine("[x] Received '{0}':'{1}'", ea.RoutingKey, message);
                    };

                    channel.BasicConsume(queue: queuename, autoAck: true, consumer: consumer);

                    Console.WriteLine("Press any key exit.");
                    Console.ReadLine();
                }
            }
        }

    }

    public class RPCServer
    {
        public static void Start()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var conection = factory.CreateConnection())
            {
                using (var channel = conection.CreateModel())
                {
                    channel.QueueDeclare(queue: "rpc_queue", durable: false,
                        exclusive: false, autoDelete: false, arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    Console.WriteLine("[*] Waiting for message.");

                    consumer.Received += (model, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body);
                        int n = int.Parse(message);
                        Console.WriteLine($"Receive request of Fib({n})");
                        int result = Fib(n);

                        var properties = ea.BasicProperties;
                        var replyProerties = channel.CreateBasicProperties();
                        replyProerties.CorrelationId = properties.CorrelationId;

                        channel.BasicPublish(exchange: "", routingKey: properties.ReplyTo,
                            basicProperties: replyProerties, body: Encoding.UTF8.GetBytes(result.ToString()));

                        channel.BasicAck(ea.DeliveryTag, false);
                        Console.WriteLine($"Return result: Fib({n})= {result}");

                    };
                    channel.BasicConsume(queue: "rpc_queue", autoAck: false, consumer: consumer);

                    Console.ReadLine();
                }
            }

        }

        private static int Fib(int n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }
            return Fib(n - 1) + Fib(n - 2);
        }
    }

    public class RPCClient
    {
        public static void Start(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var correlationId = Guid.NewGuid().ToString();
                    var replyQueue = channel.QueueDeclare().QueueName;

                    var properties = channel.CreateBasicProperties();
                    properties.ReplyTo = replyQueue;
                    properties.CorrelationId = correlationId;

                    var body = Encoding.UTF8.GetBytes(message);
                    //发布消息
                    channel.BasicPublish(exchange: "", routingKey: "rpc_queue", basicProperties: properties, body: body);

                    Console.WriteLine($"[*] Request fib({message})");

                    // //创建消费者用于消息回调
                    var callbackConsumer = new EventingBasicConsumer(channel);
                    channel.BasicConsume(queue: replyQueue, autoAck: true, consumer: callbackConsumer);

                    callbackConsumer.Received += (model, ea) =>
                    {
                        if (ea.BasicProperties.CorrelationId == correlationId)
                        {
                            var responseMsg = $"Get Response: {Encoding.UTF8.GetString(ea.Body)}";
                            Console.WriteLine($"[x]: {responseMsg}");
                        }
                    };

                    Console.ReadLine();

                }
            }
        }
    }
}
