using Axlebolt.Bolt;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Profile
{
	public class ProfileController : MonoBehaviour
	{
		[SerializeField]
		private PlayerStatsView _playerStatsView;

		public void Refresh()
		{
			_playerStatsView.SetPlayer(BoltService<BoltPlayer>.Instance.Player, BoltService<BoltPlayerStats>.Instance.Stats);
		}
	}
}
