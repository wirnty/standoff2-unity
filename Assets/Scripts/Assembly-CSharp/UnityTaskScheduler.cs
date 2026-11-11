using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

public class UnityTaskScheduler : TaskScheduler
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	[CompilerGenerated]
	private readonly string _003CName_003Ek__BackingField;

	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	[CompilerGenerated]
	private readonly UnitySynchronizationContext _003CContext_003Ek__BackingField;

	private readonly LinkedList<Task> queue = new LinkedList<Task>();

	public string Name
	{
		[CompilerGenerated]
		get
		{
			return _003CName_003Ek__BackingField;
		}
	}

	public UnitySynchronizationContext Context
	{
		[CompilerGenerated]
		get
		{
			return _003CContext_003Ek__BackingField;
		}
	}

	public UnityTaskScheduler(string name)
	{
		_003CName_003Ek__BackingField = name;
		_003CContext_003Ek__BackingField = new UnitySynchronizationContext(name);
	}

	public void Activate()
	{
		SynchronizationContext.SetSynchronizationContext(Context);
		Context.Activate();
		ExecutePendingTasks();
		Context.ExecutePendingContinuations();
	}

	protected override IEnumerable<Task> GetScheduledTasks()
	{
		lock (queue)
		{
			return queue.ToArray();
		}
	}

	protected override void QueueTask(Task task)
	{
		lock (queue)
		{
			queue.AddLast(task);
		}
	}

	protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
	{
		if (Context != SynchronizationContext.Current)
		{
			return false;
		}
		if (taskWasPreviouslyQueued)
		{
			lock (queue)
			{
				queue.Remove(task);
			}
		}
		return TryExecuteTask(task);
	}

	private void ExecutePendingTasks()
	{
		Task value;
		do
		{
			lock (queue)
			{
				if (queue.Count == 0)
				{
					return;
				}
				value = queue.First.Value;
				queue.RemoveFirst();
			}
		}
		while (value == null || TryExecuteTask(value));
		throw new InvalidOperationException();
	}
}
