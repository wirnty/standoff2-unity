using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Core;

namespace Axlebolt.Standoff.Game
{
	public class WinTeam
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly bool _003CIsDraw_003Ek__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly bool _003CIsGiveUp_003Ek__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly Team _003CWinnerTeam_003Ek__BackingField;

		public bool IsDraw
		{
			[CompilerGenerated]
			get
			{
				return _003CIsDraw_003Ek__BackingField;
			}
		}

		public bool IsGiveUp
		{
			[CompilerGenerated]
			get
			{
				return _003CIsGiveUp_003Ek__BackingField;
			}
		}

		public Team WinnerTeam
		{
			[CompilerGenerated]
			get
			{
				return _003CWinnerTeam_003Ek__BackingField;
			}
		}

		public WinTeam(bool isDraw, bool isGiveUp, Team winnerTeam)
		{
			_003CIsDraw_003Ek__BackingField = isDraw;
			_003CIsGiveUp_003Ek__BackingField = isGiveUp;
			_003CWinnerTeam_003Ek__BackingField = winnerTeam;
		}

		public static WinTeam Draw()
		{
			return new WinTeam(true, false, Team.None);
		}

		public static WinTeam GiveUp(Team winTeam)
		{
			return new WinTeam(false, true, winTeam);
		}

		public static WinTeam Winner(Team winTeam)
		{
			return new WinTeam(false, false, winTeam);
		}
	}
}
