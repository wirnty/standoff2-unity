using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Axlebolt.Standoff.UI
{
	public class DialogButton
	{
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

		public DialogButton([NotNull] string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			_003CName_003Ek__BackingField = name;
		}
	}
}
