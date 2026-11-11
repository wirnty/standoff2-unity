using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Axlebolt.Standoff.Matchmaking
{
	public class MatchmakingFilter
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly string _003CPlayerId_003Ek__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly string _003CGameModeName_003Ek__BackingField;

		public string PlayerId
		{
			[CompilerGenerated]
			get
			{
				return _003CPlayerId_003Ek__BackingField;
			}
		}

		public string GameModeName
		{
			[CompilerGenerated]
			get
			{
				return _003CGameModeName_003Ek__BackingField;
			}
		}

		public string LevelName { get; set; }

		public bool AllowEmptyRoom { get; set; }

		public string[] ExpectedPlayers { get; set; }

		public MatchmakingFilter([NotNull] string gameModeName)
			: this(Guid.NewGuid().ToString(), gameModeName)
		{
		}

		public MatchmakingFilter([NotNull] string playerId, [NotNull] string gameModeName)
		{
			if (playerId == null)
			{
				throw new ArgumentNullException("playerId");
			}
			if (gameModeName == null)
			{
				throw new ArgumentNullException("gameModeName");
			}
			_003CPlayerId_003Ek__BackingField = playerId;
			_003CGameModeName_003Ek__BackingField = gameModeName;
		}
	}
}
