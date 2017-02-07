
namespace MyCoreLib.BaseWeb.Tasks
{
    using MyCoreLib.Common;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class TaskProcessorQueueBase<T> : IDisposable
    {
        private ConcurrentQueue<T> _processingQueue;
        private volatile bool _stopped;
        private AutoResetEvent _autoResetEvent;
        private List<Task> _taskList;
        private int _numRunning;
        private Timer _startupTimer;

        /// <summary>
        /// Get the number of concurrent tasks.
        /// </summary>
        protected int NumConcurrentTasks { get; private set; }

        /// <summary>
        /// Get the initial delay in milliseconds to create background tasks.
        /// </summary>
        protected int InitialDelayMS { get; private set; }

        protected string Name { get; private set; }

        /// <summary>
        /// Create a new task queue
        /// </summary>
        protected TaskProcessorQueueBase(int concurrency, string name, int initialDelayMS = 2000)
        {
            if (concurrency <= 0) throw new ArgumentOutOfRangeException("concurrency");
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            if (initialDelayMS < 0) throw new ArgumentOutOfRangeException("initialDelayMS");

            this.InitialDelayMS = initialDelayMS;
            this.NumConcurrentTasks = concurrency;
            this.Name = name;
            this._processingQueue = new ConcurrentQueue<T>();
            this._autoResetEvent = new AutoResetEvent(false);
            this._taskList = new List<Task>();

            TimerCallback callback = delegate
            {
                // Create some background tasks for order processing
                // We will use at most 32 background tasks, or 4 at the minimal.
                Utility.OutputDebugString("[{0}] Creating {1} background tasks for task processing in app domain {2}.", name, concurrency,
                    ""/*AppDomain.CurrentDomain.FriendlyName*/);
                for (int i = 0; i < concurrency; i++)
                {
                    Task t = Task.Run((Action)Run);
                    this._taskList.Add(t);
                }
            };

            // Disabled until Start is called.
            _startupTimer = new Timer(callback, null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// The sub class could have some customized code before starting to run.
        /// </summary>
        public virtual void Start()
        {
            // Only run the startup code once
            _startupTimer.Change(this.InitialDelayMS, Timeout.Infinite);
        }

        public void Stop()
        {
            _stopped = true;
            _autoResetEvent.Set();
        }

        public virtual void EnqueueTask(T task)
        {
            if (task == null)
                throw new ArgumentNullException("task");

            _processingQueue.Enqueue(task);

            // Notify a thread to process the task.
            _autoResetEvent.Set();
        }

        private void Run()
        {
            int current = Interlocked.Increment(ref _numRunning);
            Utility.OutputDebugString("[{0}] {1} tasks were started for task processing.", Name, current);
            try
            {
                RunCore();
            }
            finally
            {
                Interlocked.Decrement(ref _numRunning);
            }
        }

        private void RunCore()
        {
            do
            {
                _autoResetEvent.WaitOne();

                while (!_stopped)
                {
                    // Get a task from the processing queue
                    T task;
                    if (!_processingQueue.TryDequeue(out task))
                    {
                        // No more tasks to process.
                        break;
                    }

                    // Start to process the task now.
                    ProcessTask(task);
                };
            } while (!_stopped);
        }

        /// <summary>
        /// The main method to process a single task. 
        /// Please note that the sub class will have to handle all exceptions.
        /// </summary>
        protected abstract void ProcessTask(T task);

        public virtual void Dispose()
        {
            // Mark as stopped
            this._stopped = true;

            for (int i = 0; i < _taskList.Count; i++)
                _autoResetEvent.Set();

            Timer oldTimer = Interlocked.Exchange(ref _startupTimer, null);
            if (oldTimer != null)
                oldTimer.Dispose();
        }
    }
}
