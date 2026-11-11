using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Axlebolt.Standoff.Game
{
	public class GamePlayer
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly PhotonPlayer _003CPhotonPlayer_003Ek__BackingField;

		private readonly HashSet<HitLogs> _hits;

		public PhotonPlayer PhotonPlayer
		{
			[CompilerGenerated]
			get
			{
				return _003CPhotonPlayer_003Ek__BackingField;
			}
		}

		public GamePlayer(PhotonPlayer photonPlayer)
		{
			_003CPhotonPlayer_003Ek__BackingField = photonPlayer;
			_hits = new HashSet<HitLogs>();
		}

		public void ClearHits()
		{
			_hits.Clear();
		}

		public void AddHit(PhotonPlayer shooter, int damage)
		{
			GetHits(shooter).Damages.Add(damage);
		}

		[NotNull]
		public HitLogs GetHits(PhotonPlayer filter)
		{
			HitLogs hitLogs = _hits.Where((HitLogs hit) => hit.PlayerId == filter.ID).ElementAtOrDefault(0);
			if (hitLogs == null)
			{
				hitLogs = new HitLogs(filter);
				_hits.Add(hitLogs);
			}
			return hitLogs;
		}

		[CanBeNull]
		public HitLogs GetAssistHits(PhotonPlayer killer, int minTotalDamage)
		{
			HitLogs hitLogs = (from hit in _hits
				where hit.PlayerId != killer.ID
				orderby hit.Damages.Sum() descending
				select hit).ElementAtOrDefault(0);
			if (hitLogs != null && hitLogs.Damages.Sum() < minTotalDamage)
			{
				hitLogs = null;
			}
			return hitLogs;
		}
	}
}
