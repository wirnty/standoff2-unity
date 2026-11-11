using Axlebolt.Bolt;
using Axlebolt.Standoff.UI;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Profile
{
	public class PlayerStatsView : View
	{
		[SerializeField]
		private PlayerGeneralStatsView _playerGeneralStatsView;

		[SerializeField]
		private WeaponsStatsView _weaponsStatsView;

		public void SetPlayer(Axlebolt.Bolt.Player player, PlayerStats stats)
		{
			_playerGeneralStatsView.SetPlayer(player, stats);
			_weaponsStatsView.SetPlayer(stats);
		}
	}
}
