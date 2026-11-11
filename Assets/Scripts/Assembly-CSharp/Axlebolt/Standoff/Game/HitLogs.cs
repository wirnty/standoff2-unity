using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Axlebolt.Standoff.Game
{
	public class HitLogs
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly int _003CPlayerId_003Ek__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly List<int> _003CDamages_003Ek__BackingField;

		public int PlayerId
		{
			[CompilerGenerated]
			get
			{
				return _003CPlayerId_003Ek__BackingField;
			}
		}

		public PhotonPlayer Player
		{
			[CompilerGenerated]
			get
			{
				return PhotonPlayer.Find(PlayerId);
			}
		}

		public List<int> Damages
		{
			[CompilerGenerated]
			get
			{
				return _003CDamages_003Ek__BackingField;
			}
		}

		public HitLogs([NotNull] PhotonPlayer player)
		{
			if (player == null)
			{
				throw new ArgumentNullException("player");
			}
			_003CPlayerId_003Ek__BackingField = player.ID;
			_003CDamages_003Ek__BackingField = new List<int>();
		}
	}
}
