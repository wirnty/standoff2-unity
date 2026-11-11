using System;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI.Statistics
{
	public class StatisticsPlayersView : View
	{
		[SerializeField]
		private Color _normalColor;

		[SerializeField]
		private Color _currentColor;

		[SerializeField]
		private Color _deathColor;

		[SerializeField]
		private Color _normalTextColor;

		[SerializeField]
		private Color _currentTextColor;

		[SerializeField]
		private Color _deathTextColor;

		[NotNull]
		[SerializeField]
		private StatisticsPlayerView _playerViewPrefab;

		private ViewPool<StatisticsPlayerView> _playerViewPool;

		private void Awake()
		{
			_playerViewPool = new ViewPool<StatisticsPlayerView>(_playerViewPrefab, 5);
		}

		public void Refresh(PhotonPlayer[] players)
		{
			int num = Math.Min(players.Length, 5);
			StatisticsPlayerView[] items = _playerViewPool.GetItems(num);
			for (int i = 0; i < num; i++)
			{
				PhotonPlayer photonPlayer = players[i];
				bool flag = photonPlayer.Equals(PhotonNetwork.player);
				bool isTeammate = photonPlayer.GetTeam() == PhotonNetwork.player.GetTeam();
				Color color = GetColor(flag, isTeammate, photonPlayer);
				Color textColor = GetTextColor(flag, isTeammate, photonPlayer);
				items[i].Refresh(photonPlayer, color, textColor, flag);
			}
		}

		private Color GetColor(bool isCurrentPlayer, bool isTeammate, PhotonPlayer player)
		{
			if (isCurrentPlayer)
			{
				return _currentColor;
			}
			if (!isTeammate)
			{
				return _normalColor;
			}
			return (!player.IsDead()) ? _normalColor : _deathColor;
		}

		private Color GetTextColor(bool isCurrentPlayer, bool isTeammate, PhotonPlayer player)
		{
			if (isCurrentPlayer)
			{
				return _currentTextColor;
			}
			if (!isTeammate)
			{
				return _normalTextColor;
			}
			return (!player.IsDead()) ? _normalTextColor : _deathTextColor;
		}
	}
}
