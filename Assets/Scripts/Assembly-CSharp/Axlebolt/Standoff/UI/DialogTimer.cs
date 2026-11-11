using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.UI
{
	public class DialogTimer
	{
		public interface ITimer
		{
		}

		public class Default : ITimer
		{
		}

		public class CountDown : ITimer
		{
			[CompilerGenerated]
			[DebuggerBrowsable(DebuggerBrowsableState.Never)]
			private readonly float _003CDuration_003Ek__BackingField;

			public float Duration
			{
				[CompilerGenerated]
				get
				{
					return _003CDuration_003Ek__BackingField;
				}
			}

			public CountDown(float duration)
			{
				_003CDuration_003Ek__BackingField = duration;
			}
		}
	}
}
