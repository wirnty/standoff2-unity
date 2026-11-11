using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.UI.Statistics;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI
{
	public class StatisticsView : View, IPlayerPropSensitiveView
	{
		private static readonly string[] PlayerProps = new string[6] { "team", "isDeath", "score", "kills", "death", "ping" };

		[SerializeField]
		[NotNull]
		private StatisticsHeaderView _header;

		[NotNull]
		[SerializeField]
		private StatisticsScoreView _ctScore;

		[SerializeField]
		[NotNull]
		private StatisticsHighlightView _ctHighlightView;

		[SerializeField]
		[NotNull]
		private StatisticsPlayersView _ctPlayerStatistics;

		[SerializeField]
		[NotNull]
		private StatisticsScoreView _trScore;

		[SerializeField]
		[NotNull]
		private StatisticsPlayersView _trPlayerStatistics;

		[NotNull]
		[SerializeField]
		private StatisticsHighlightView _trHighlightView;

		private bool _gameRunning;

		public string GameDescription
		{
			set
			{
				_header.Description = value;
			}
		}

		public bool ScoreEnabled
		{
			set
			{
				StatisticsScoreView ctScore = _ctScore;
				bool active = value;
				_trScore.Active = active;
				ctScore.Active = active;
			}
		}

		public string CtScore
		{
			set
			{
				_ctScore.Score = value;
			}
		}

		public string TrScore
		{
			set
			{
				_trScore.Score = value;
			}
		}

		public string CtHighlightText
		{
			set
			{
				_ctHighlightView.Text = value;
				_ctHighlightView.Show();
			}
		}

		public string TrHighlighText
		{
			set
			{
				_trHighlightView.Text = value;
				_trHighlightView.Show();
			}
		}

		public double TimeLeft
		{
			set
			{
				_header.Time = value;
			}
		}

		public string[] SensitivePlayerProperties
		{
			[CompilerGenerated]
			get
			{
				return PlayerProps;
			}
		}

		private void Awake()
		{
			_ctHighlightView.Hide();
			_trHighlightView.Hide();
		}

		public void Init(string gameMode, string levelName)
		{
			_gameRunning = true;
			_header.GameMode = gameMode;
			_header.Level = levelName;
		}

		public void GameFinished(PhotonPlayer[] finalPlayers)
		{
			_gameRunning = false;
			Refresh(finalPlayers.OrderByScore());
		}

		public override void Show()
		{
			if (!base.IsVisible)
			{
				Refresh();
				base.Show();
			}
		}

		public void Refresh()
		{
			if (_gameRunning)
			{
				Refresh(PhotonNetwork.playerList.OrderByScore());
			}
		}

		public void Refresh(PhotonPlayer[] orderedPlayers)
		{
			PhotonPlayer[] byTeam = orderedPlayers.GetByTeam(Team.Ct);
			PhotonPlayer[] byTeam2 = orderedPlayers.GetByTeam(Team.Tr);
			_ctPlayerStatistics.Refresh(byTeam);
			_trPlayerStatistics.Refresh(byTeam2);
		}
	}
}
