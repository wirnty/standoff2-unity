using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.RpcSupport
{
	public class RpcException : Exception
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly int _003CCode_003Ek__BackingField;

		public int Code
		{
			[CompilerGenerated]
			get
			{
				return _003CCode_003Ek__BackingField;
			}
		}

		public long Id { get; internal set; }

		public override string Message
		{
			[CompilerGenerated]
			get
			{
				return string.Format("RpcException (Id: {0}, Code: {1})", Id, Code);
			}
		}

		public RpcException(int code)
		{
			_003CCode_003Ek__BackingField = code;
		}
	}
}
