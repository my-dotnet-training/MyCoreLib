using MyCoreLib.Common.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyCoreLib.BasePush
{
    public class BaseMessageReceiver : BaseSingleton<BaseMessageReceiver>
    {
        private Timer _configRefreshTimer;
        //private Aliyun.MNS.IQueue _queue;
        private string _storageConnectionString;
        private Task _readMsgTask;
        private volatile bool _stopped;
        private static readonly object s_locker = new object();
        public BaseMessageReceiver()
        {
            // Do not enable the timer for now.
            _configRefreshTimer = new Timer(RefreshMNSConfigs, "", Timeout.Infinite, Timeout.Infinite);
        }        
        
        private void RefreshMNSConfigs(object state)
        {
            if (!_stopped)
            {
                _storageConnectionString = File.ReadAllText(state.ToString());
            }
        }

        public void Start()
        {
            // Refresh MNS configuration every 5 minutes
            _configRefreshTimer.Change(10, 5 * 60 * 1000);

            // create a task to read order messages
            _readMsgTask = Task.Run((Action)ReadMessages);
        }

        public void Stop()
        {
            _stopped = true;
        }

        public void Dispose()
        {
            Timer oldTimer = Interlocked.Exchange(ref _configRefreshTimer, null);
            if (oldTimer != null)
            {
                oldTimer.Dispose();
            }
        }

        private void ReadMessages()
        {
            bool _gotconfig = false;

            while (!_gotconfig)
            {
                if (_storageConnectionString == null)
                {
                    Thread.Sleep(10 * 1000);
                    continue;
                }
                _gotconfig = true;
            }

            while (!_stopped)
            {
#if false
                Aliyun.MNS.IQueue queue = this._queue;
                if (queue == null)
                {
                    Thread.Sleep(10 * 1000);

                    // do nothing if the queue wasn't initialized
                    continue;
                }
#endif
                try
                {
                    var msgStr = "";
#if false
                    var msg = queue.ReceiveMessage();
#endif

                    if (!string.IsNullOrWhiteSpace(msgStr) && msgStr.IndexOf("Type") > 0)// && msg.Message != null && msg.Message.Body != null)
                    {
                        //MessageItem item = msg.Message.Body.FromJsonString<MessageItem>();
                        if (item != null)
                        {
                            if (item.Type == MessageType.UserRechargeOrder)
                            {
                                // enqueue the withdraw order
                                string orderId = item.Body;
                                if (!string.IsNullOrWhiteSpace(orderId))
                                {
                                    Utility.OutputDebugString("Receiving withdraw order: {0}", orderId);
                                    WithdrawProcessorQueue.Instance.EnqueueTask(item.Body);
                                }
                                queue.DeleteMessage(cqMessage);
                            }
                            else
                            {
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                Thread.Sleep(50);
            }
        }

    }
}
