using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyCoreLib.BaseAsync.TAP
{
    public static class TaskManager
    {
        static CancellationToken cancellationToken;
        /// <summary>
        /// cancel all task
        /// </summary>
        public static CancellationTokenSource TaskCancellationToken = new CancellationTokenSource();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public static void CancelTasks(string message = "")
        {
            //if (!string.IsNullOrEmpty(message))
            //    TaskManager.TaskCancellationToken.Token.Register(() =>
            //    {
            //        MessageBox.Show(message);
            //    }, true);
            //TaskCancellationToken.Cancel();

            if (cancellationToken.IsCancellationRequested)
                //TaskCancellationToken.Cancel();
                Task.FromCanceled(cancellationToken);

        }

        /// <summary>
        /// 
        /// </summary>
        public static void AddCancel(Action callback = null)
        {
            cancellationToken = TaskCancellationToken.Token;
            //TaskManager.TaskCancellationToken.Token.ThrowIfCancellationRequested();
            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException(cancellationToken);

            if (callback != null)
                cancellationToken.Register(() =>
                {
                    callback();
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskStart"></param>
        /// <param name="taskDone"></param>
        /// <param name="handleException"></param>
        /// <param name="scheduler"></param>
        public static Task StartNew(Action taskStart, Action taskDone = null, Action<AggregateException> handleException = null, TaskScheduler scheduler = null)
        {
            //if (TaskCancellationToken.IsCancellationRequested)
            //    TaskCancellationToken = new CancellationTokenSource();
            Task task = Task.Factory.StartNew(taskStart, cancellationToken);

            if (taskDone != null)
                task.ContinueWith(t => taskDone, CancellationToken.None,
                  TaskContinuationOptions.OnlyOnRanToCompletion,
                  scheduler);

            if (handleException != null)
                task.ContinueWith(t => handleException(t.Exception), CancellationToken.None,
                  TaskContinuationOptions.OnlyOnFaulted, scheduler);

            return task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskStart"></param>
        /// <param name="taskDone"></param>
        /// <param name="handleException"></param>
        /// <param name="scheduler"></param>
        public static Task StartNew(Action taskStart, TaskCreationOptions taskOption, Action taskDone = null, Action<AggregateException> handleException = null, TaskScheduler scheduler = null)
        {
            //if (TaskCancellationToken.IsCancellationRequested)
            //    TaskCancellationToken = new CancellationTokenSource();
            Task task = Task.Factory.StartNew(taskStart, cancellationToken, taskOption, scheduler);

            if (taskDone != null)
                task.ContinueWith(t => taskDone, CancellationToken.None,
                  TaskContinuationOptions.OnlyOnRanToCompletion,
                  scheduler);

            if (handleException != null)
                task.ContinueWith(t => handleException(t.Exception), CancellationToken.None,
                  TaskContinuationOptions.OnlyOnFaulted, scheduler);

            return task;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="taskStart"></param>
        /// <param name="taskDone"></param>
        /// <param name="handleException"></param>
        /// <param name="scheduler"></param>
        public static Task<T> StartNew<T>(Func<T> taskStart, Action<T> taskDone = null, Action<AggregateException> handleException = null, TaskScheduler scheduler = null)
        {
            //if (TaskManager.TaskCancellationToken.IsCancellationRequested)
            //    TaskManager.TaskCancellationToken = new CancellationTokenSource();
            Task<T> task = Task<T>.Factory.StartNew(taskStart, cancellationToken);
           
            if (taskDone != null)
                task.ContinueWith(t => taskDone(t.Result), CancellationToken.None,
                  TaskContinuationOptions.OnlyOnRanToCompletion,
                  scheduler);

            //if (handleException != null)
            //    task.ContinueWith(t => handleException(t.Exception), CancellationToken.None,
            //      TaskContinuationOptions.OnlyOnFaulted, scheduler);
            task.ContinueWith(t =>
            {
                if (handleException != null)
                    handleException(t.Exception);
                else
                    t.Exception.OutputException(null);
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, scheduler);

            return task;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="taskStart"></param>
        /// <param name="taskDone"></param>
        /// <param name="handleException"></param>
        /// <param name="scheduler"></param>
        public static Task<T> StartNew<T>(Func<T> taskStart, TaskCreationOptions taskOption, Action<T> taskDone = null, Action<AggregateException> handleException = null, TaskScheduler scheduler = null)
        {
            //if (TaskManager.TaskCancellationToken.IsCancellationRequested)
            //    TaskManager.TaskCancellationToken = new CancellationTokenSource();
            Task<T> task = Task<T>.Factory.StartNew(taskStart, cancellationToken, taskOption, scheduler);

            if (taskDone != null)
                task.ContinueWith(t => taskDone(t.Result), CancellationToken.None,
                  TaskContinuationOptions.OnlyOnRanToCompletion,
                  scheduler);

            if (handleException != null)
                task.ContinueWith(t => handleException(t.Exception), CancellationToken.None,
                  TaskContinuationOptions.OnlyOnFaulted, scheduler);

            return task;
        }

        public static Task<T> FromAsync<T>(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, T> endMethod, object[] state, Action<T> taskDone = null, Action<AggregateException> handleException = null, TaskScheduler scheduler = null)
        {
            //if (TaskCancellationToken.IsCancellationRequested)
            //    TaskCancellationToken = new CancellationTokenSource();
            Task<T> task = Task<T>.Factory.FromAsync(beginMethod, endMethod, state);

            if (taskDone != null)
                task.ContinueWith(t => taskDone(t.Result), CancellationToken.None,
                  TaskContinuationOptions.OnlyOnRanToCompletion,
                  scheduler);

            if (handleException != null)
                task.ContinueWith(t => handleException(t.Exception), CancellationToken.None,
                  TaskContinuationOptions.OnlyOnFaulted, scheduler);
            return task;
        }
        public static Task<T> FromAsync<T>(Func<AsyncCallback, object, IAsyncResult> beginMethod, Func<IAsyncResult, T> endMethod, object[] state, TaskCreationOptions taskOption, Action<T> taskDone = null, Action<AggregateException> handleException = null, TaskScheduler scheduler = null)
        {
            //if (TaskCancellationToken.IsCancellationRequested)
            //    TaskCancellationToken = new CancellationTokenSource();
            Task<T> task = Task<T>.Factory.FromAsync(beginMethod, endMethod, state, taskOption);

            if (taskDone != null)
                task.ContinueWith(t => taskDone(t.Result), CancellationToken.None,
                  TaskContinuationOptions.OnlyOnRanToCompletion,
                  scheduler);

            if (handleException != null)
                task.ContinueWith(t => handleException(t.Exception), CancellationToken.None,
                  TaskContinuationOptions.OnlyOnFaulted, scheduler);
            return task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="sender"></param>
        public static void OutputException(this AggregateException exception, object sender)
        {
            //foreach (Exception innerException in exception.InnerExceptions)
            //{
            //    NotificationManager.SendOutput(
            //       sender,
            //       Icons.Error,
            //       new OutputEventArgs.OutputCategory(Global.GetString(sender.GetType(), "Exception")),
            //      innerException.Message);
            //    ConfirmationForm.Show(Confirmations.Generic, ConfirmationButtons.Ok,
            //        SystemIcons.Error,
            //        Global.GetString(typeof(TaskManager), "Error"),
            //        innerException.Message,
            //        null,
            //        innerException.Message);
            //}

            foreach (Exception innerException in exception.InnerExceptions)
                Console.WriteLine(innerException);
        }
    }
}
