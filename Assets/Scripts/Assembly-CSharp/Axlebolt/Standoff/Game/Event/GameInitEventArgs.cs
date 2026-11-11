using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Axlebolt.Standoff.Game.Event
{
	public class GameInitEventArgs
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly string _003CGameMode_003Ek__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly int _003CMaxPlayers_003Ek__BackingField;

		public string GameMode
		{
			[CompilerGenerated]
			get
			{
				return _003CGameMode_003Ek__BackingField;
			}
		}

		public int MaxPlayers
		{
			[CompilerGenerated]
			get
			{
				return _003CMaxPlayers_003Ek__BackingField;
			}
		}

		public GameInitEventArgs([NotNull] string gameMode, int maxPlayers)
		{
			if (gameMode == null)
			{
				throw new ArgumentNullException("gameMode");
			}
			if (maxPlayers <= 0)
			{
				throw new ArgumentOutOfRangeException("maxPlayers");
			}
			_003CGameMode_003Ek__BackingField = gameMode;
			_003CMaxPlayers_003Ek__BackingField = maxPlayers;
		}
	}
}
