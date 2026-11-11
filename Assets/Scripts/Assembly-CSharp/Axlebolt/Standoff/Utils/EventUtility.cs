using System;
using System.Diagnostics;
using Axlebolt.Standoff.Core;

namespace Axlebolt.Standoff.Utils
{
	public class EventUtility
	{
		private static readonly Log Log = Log.Create(typeof(EventUtility));

		public static void SafeInvoke<T>(MulticastDelegate multicastDelegate, object sender, T args) where T : EventArgs
		{
			if ((object)multicastDelegate == null)
			{
				return;
			}
			Delegate[] invocationList = multicastDelegate.GetInvocationList();
			foreach (Delegate @delegate in invocationList)
			{
				EventHandler<T> eventHandler = (EventHandler<T>)@delegate;
				try
				{
					eventHandler(sender, args);
				}
				catch (Exception message)
				{
					Log.Error(message);
				}
			}
		}

		[Conditional("ENABLE_PROFILER")]
		private static void BeginSample<T>(EventHandler<T> handler) where T : EventArgs
		{
		}

		[Conditional("ENABLE_PROFILER")]
		private static void EndSample()
		{
		}
	}
}
