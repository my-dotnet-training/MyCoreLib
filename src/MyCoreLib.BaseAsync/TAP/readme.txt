基于任务的异步模式

Task.ExecuteWithThreadLocal
if (capturedContext == null)
{
	Execute();
}
else
{
	if (IsSelfReplicatingRoot || IsChildReplica)
	{
		CapturedContext = CopyExecutionContext(capturedContext);
	}
	ContextCallback callback = ExecutionContextCallback;
	ExecutionContext.Run(capturedContext, callback, this, true);
}

Task.Factory.FromAsync
底层是AMP方式
TaskFactory.FromAsyncCoreLogic

Task.Factory.StartNew
底层是Thread方式
ExecutionContext.Run

ExecutionContext

SynchronizationContext 队列
线程池
ThreadPoolGlobals.workQueue.Enqueue

https://msdn.microsoft.com/zh-cn/library/dd537609(v=vs.110).aspx
