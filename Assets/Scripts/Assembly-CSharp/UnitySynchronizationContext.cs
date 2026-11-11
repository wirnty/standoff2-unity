using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

public class UnitySynchronizationContext : SynchronizationContext
{
	private readonly BlockingCollection<KeyValuePair<SendOrPostCallback, object>> queue = new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private readonly string _003CName_003Ek__BackingField;

	public string Name
	{
		[CompilerGenerated]
		get
		{
			return _003CName_003Ek__BackingField;
		}
	}

	public event EventHandler Activated;

	public UnitySynchronizationContext(string name)
	{
		_003CName_003Ek__BackingField = name;
	}

	public void Activate()
	{
		if (this.Activated != null)
		{
			this.Activated(this, EventArgs.Empty);
		}
		this.Activated = null;
	}

	public override void Post(SendOrPostCallback d, object state)
	{
		queue.Add(new KeyValuePair<SendOrPostCallback, object>(d, state));
	}

	public void ExecutePendingContinuations()
	{
		KeyValuePair<SendOrPostCallback, object> item;
		while (queue.TryTake(out item))
		{
			item.Key(item.Value);
		}
	}
}
