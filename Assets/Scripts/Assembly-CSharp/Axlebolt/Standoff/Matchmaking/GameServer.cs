using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Matchmaking
{
	public class GameServer
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly string _003CIp_003Ek__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly string _003CLocation_003Ek__BackingField;

		public string Ip
		{
			[CompilerGenerated]
			get
			{
				return _003CIp_003Ek__BackingField;
			}
		}

		public string Location
		{
			[CompilerGenerated]
			get
			{
				return _003CLocation_003Ek__BackingField;
			}
		}

		public GameServer(string ip, string location)
		{
			_003CIp_003Ek__BackingField = ip;
			_003CLocation_003Ek__BackingField = location;
		}
	}
}
