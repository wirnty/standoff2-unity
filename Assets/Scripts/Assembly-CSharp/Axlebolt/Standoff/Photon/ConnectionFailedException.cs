using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Photon
{
	public class ConnectionFailedException : PhotonException
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly DisconnectCause _003CDisconnectCause_003Ek__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly bool _003CHasCause_003Ek__BackingField;

		public DisconnectCause DisconnectCause
		{
			[CompilerGenerated]
			get
			{
				return _003CDisconnectCause_003Ek__BackingField;
			}
		}

		public bool HasCause
		{
			[CompilerGenerated]
			get
			{
				return _003CHasCause_003Ek__BackingField;
			}
		}

		public ConnectionFailedException()
		{
			_003CHasCause_003Ek__BackingField = false;
		}

		public ConnectionFailedException(DisconnectCause disconnectCause)
		{
			_003CDisconnectCause_003Ek__BackingField = disconnectCause;
			_003CHasCause_003Ek__BackingField = true;
		}
	}
}
