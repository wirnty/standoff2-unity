// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// AsyncTools
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public static class AsyncTools
{
	public abstract class Awaiter : INotifyCompletion
	{
		public abstract bool IsCompleted { get; }

		public abstract void OnCompleted(Action action);

		public Awaiter GetAwaiter()
		{
			return this;
		}

		public void GetResult()
		{
		}
	}

	private class DelayAwaiter : Awaiter
	{
		private readonly SynchronizationContext context;

		private readonly float seconds;

		public override bool IsCompleted
		{
			[CompilerGenerated]
			get
			{
				return seconds <= 0f;
			}
		}

		public DelayAwaiter(float seconds)
		{
			context = SynchronizationContext.Current;
			this.seconds = seconds;
		}

		public override void OnCompleted(Action action)
		{
			Task.Delay((int)(seconds * 1000f)).ContinueWith(delegate
			{
				if (context != null)
				{
					context.Post(delegate
					{
						action();
					}, null);
				}
				else
				{
					action();
				}
			});
		}
	}

	private class ContextActivationAwaiter : Awaiter
	{
		private readonly UnitySynchronizationContext context;

		private Action continuation;

		public override bool IsCompleted
		{
			[CompilerGenerated]
			get
			{
				return false;
			}
		}

		public ContextActivationAwaiter(UnitySynchronizationContext context)
		{
			this.context = context;
		}

		public override void OnCompleted(Action action)
		{
			continuation = action;
			context.Activated += ContextActivationEventHandler;
		}

		private void ContextActivationEventHandler(object sender, EventArgs eventArgs)
		{
			context.Activated -= ContextActivationEventHandler;
			context.Post(delegate
			{
				continuation();
			}, null);
		}
	}

	private class SynchronizationContextAwaiter : Awaiter
	{
		private readonly UnitySynchronizationContext context;

		public override bool IsCompleted
		{
			[CompilerGenerated]
			get
			{
				return context == null || context == SynchronizationContext.Current;
			}
		}

		public SynchronizationContextAwaiter(UnitySynchronizationContext context)
		{
			this.context = context;
		}

		public override void OnCompleted(Action action)
		{
			context.Post(delegate
			{
				action();
			}, null);
		}
	}

	private class ThreadPoolContextAwaiter : Awaiter
	{
		public override bool IsCompleted
		{
			[CompilerGenerated]
			get
			{
				return !IsMainThread();
			}
		}

		public override void OnCompleted(Action action)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				action();
			}, null);
		}
	}

	private class AsyncOperationAwaiter : Awaiter
	{
		[CompilerGenerated]
		private sealed class _OnCompleted_c__AnonStorey4
		{
			[StructLayout(LayoutKind.Auto)]
			public struct _OnCompleted_c__async3 : IAsyncStateMachine
			{
				internal _OnCompleted_c__AnonStorey4 f__ref_4;

		internal AsyncTaskMethodBuilder _builder;

				internal int _PC;

				private Awaiter _awaiter0;

				public void MoveNext()
		{
			uint num = (uint)_PC;
					_PC = -1;
			try
			{
				switch (num)
				{
					default:
						return;
					case 1u:
							_awaiter0.GetResult();
						goto case 0u;
					case 0u:
						if (!f__ref_4._this.asyncOp.isDone)
							{
								_awaiter0 = 0.GetAwaiter();
							if (!_awaiter0.IsCompleted)
								{
									_PC = 1;
									_builder.AwaitOnCompleted(ref _awaiter0, ref this);
								return;
							}
							goto case 1u;
						}
							f__ref_4.action();
						break;
				}
			}
			catch (Exception exception)
			{
						_PC = -1;
						_builder.SetException(exception);
				return;
			}
					_PC = -1;
					_builder.SetResult();
		}

		[DebuggerHidden]
		public void SetStateMachine(IAsyncStateMachine stateMachine)
		{
					_builder.SetStateMachine(stateMachine);
		}
	}

	internal Action action;

	internal AsyncOperationAwaiter _this;

			[AsyncStateMachine(typeof(_OnCompleted_c__async3))]
	internal Task m__0()
	{
				_OnCompleted_c__async3 stateMachine = default(_OnCompleted_c__async3);
		stateMachine.f__ref_4 = this;
		stateMachine._builder = AsyncTaskMethodBuilder.Create();
		ref AsyncTaskMethodBuilder _builder = ref stateMachine._builder;
				_builder.Start(ref stateMachine);
		return _builder.Task;
	}
}

private readonly AsyncOperation asyncOp;

public override bool IsCompleted
{
	[CompilerGenerated]
	get
	{
		return asyncOp.isDone;
	}
}

public AsyncOperationAwaiter(AsyncOperation asyncOp)
{
	this.asyncOp = asyncOp;
}

public override void OnCompleted(Action action)
{
			_OnCompleted_c__AnonStorey4 CS___8__locals0 = new _OnCompleted_c__AnonStorey4();
	CS___8__locals0.action = action;
	CS___8__locals0._this = this;
	Task.Factory.StartNew(() =>
	{
				_OnCompleted_c__AnonStorey4._OnCompleted_c__async3 stateMachine = default(_OnCompleted_c__AnonStorey4._OnCompleted_c__async3);
		stateMachine.f__ref_4 = CS___8__locals0;
		stateMachine._builder = AsyncTaskMethodBuilder.Create();
		ref AsyncTaskMethodBuilder _builder = ref stateMachine._builder;
				_builder.Start(ref stateMachine);
		return _builder.Task;
	}, CancellationToken.None, TaskCreationOptions.None, UnityScheduler.UpdateScheduler);
}
	}

	private static Awaiter updateAwaiter;

private static Awaiter fixedAwaiter;

private static Awaiter lateUpdateAwaiter;

private static Awaiter editorUpdateAwaiter;

private static Awaiter threadPoolAwaiter = new ThreadPoolContextAwaiter();

public static void WhereAmI(string text)
{
	if (IsMainThread())
	{
		UnitySynchronizationContext obj = SynchronizationContext.Current as UnitySynchronizationContext;
		string arg = ((obj != null) ? obj.Name : null) ?? "No context";
		UnityEngine.Debug.Log(string.Format("{0}: main thread, {1}, frame: {2}", text, arg, Time.frameCount));
	}
	else
	{
		UnityEngine.Debug.Log(string.Format("{0}: background thread, id: {1}", text, Thread.CurrentThread.ManagedThreadId));
	}
}

public static bool IsMainThread()
{
	return Thread.CurrentThread.ManagedThreadId == UnityScheduler.MainThreadId;
}

public static Awaiter ToThreadPool()
{
	return threadPoolAwaiter;
}

[Obsolete("Use ToUpdate(), ToLateUpdate() or ToFixedUpdate() instead.")]
public static Awaiter ToMainThread()
{
	return ToUpdate();
}

public static Awaiter ToEditorUpdate()
{
	return editorUpdateAwaiter ?? (editorUpdateAwaiter = new SynchronizationContextAwaiter(UnityScheduler.EditorUpdateScheduler.Context));
}

public static Awaiter ToUpdate()
{
	return updateAwaiter ?? (updateAwaiter = new SynchronizationContextAwaiter(UnityScheduler.UpdateScheduler.Context));
}

public static Awaiter ToLateUpdate()
{
	return lateUpdateAwaiter ?? (lateUpdateAwaiter = new SynchronizationContextAwaiter(UnityScheduler.LateUpdateScheduler.Context));
}

public static Awaiter ToFixedUpdate()
{
	return fixedAwaiter ?? (fixedAwaiter = new SynchronizationContextAwaiter(UnityScheduler.FixedUpdateScheduler.Context));
}

public static Task<byte[]> DownloadAsBytesAsync(string address, CancellationToken cancellationToken = default(CancellationToken))
{
	return Task.Factory.StartNew(delegate
	{
		using (WebClient webClient = new WebClient())
		{
			return webClient.DownloadData(address);
		}
	}, cancellationToken);
}

public static Task<string> DownloadAsStringAsync(string address, CancellationToken cancellationToken = default(CancellationToken))
{
	return Task.Factory.StartNew(delegate
	{
		using (WebClient webClient = new WebClient())
		{
			return webClient.DownloadString(address);
		}
	}, cancellationToken);
}

public static Awaiter GetAwaiter(this float seconds)
{
	UnitySynchronizationContext unitySynchronizationContext = SynchronizationContext.Current as UnitySynchronizationContext;
	if (seconds <= 0f && unitySynchronizationContext != null)
	{
		return new ContextActivationAwaiter(unitySynchronizationContext);
	}
	return new DelayAwaiter(seconds);
}

public static Awaiter GetAwaiter(this int seconds)
{
	return ((float)seconds).GetAwaiter();
}

public static TaskAwaiter GetAwaiter(this IEnumerable<Task> tasks)
{
	return Task.WhenAll(tasks).GetAwaiter();
}

public static TaskAwaiter<int> GetAwaiter(this Process process)
{
	TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
	process.EnableRaisingEvents = true;
	process.Exited += delegate
	{
		tcs.TrySetResult(process.ExitCode);
	};
	if (process.HasExited)
	{
		tcs.TrySetResult(process.ExitCode);
	}
	return tcs.Task.GetAwaiter();
}

public static Awaiter GetAwaiter(this AsyncOperation asyncOp)
{
	return new AsyncOperationAwaiter(asyncOp);
}
}
