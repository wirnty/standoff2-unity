using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Axlebolt.Bolt
{
	public class Player
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly int _003CTimeInGame_003Ek__BackingField;

		public string Uid { get; internal set; }

		public string Name { get; internal set; }

		public byte[] Avatar { get; internal set; }

		public int TimeInGame
		{
			[CompilerGenerated]
			get
			{
				return _003CTimeInGame_003Ek__BackingField;
			}
		}

		public Player(string uid, string name, byte[] avatar, int timeInGame)
		{
			Uid = uid;
			Name = name;
			Avatar = avatar;
			_003CTimeInGame_003Ek__BackingField = timeInGame;
		}
	}
}
