using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.UI;
using Axlebolt.Standoff.Game.UI.GameStats;

namespace Axlebolt.Standoff.Game.State
{
	public class RunningState : TimerGameState
	{
		private GameController _gameController;

		private StatisticsView _statisticsView;

		private PersonalGameStatsView _personalGameStatsView;

		public override byte Id
		{
			[CompilerGenerated]
			get
			{
				return 30;
			}
		}

		public RunningState(double duration)
			: base(duration)
		{
		}

		public override void Setup(GameController gameController)
		{
			_gameController = gameController;
			_statisticsView = gameController.GameView.GetView<StatisticsView>();
			_personalGameStatsView = gameController.GameView.GetView<PersonalGameStatsView>();
		}

		public override void Enter()
		{
			if (_gameController.GameType == GameType.Personal)
			{
				_personalGameStatsView.TimeVisible = true;
				_personalGameStatsView.ScoreVisible = true;
				_personalGameStatsView.Refresh();
			}
			_gameController.SpawnPlayer();
			_statisticsView.VisibleChanged += RefreshTimer;
		}

		public override void Update()
		{
			RefreshTimer(_statisticsView.IsVisible);
			if (_gameController.GameType == GameType.Personal)
			{
				_personalGameStatsView.Time = base.TimeLeft;
			}
		}

		private void RefreshTimer(bool visible)
		{
			if (visible)
			{
				_statisticsView.TimeLeft = base.TimeLeft;
			}
		}

		public override void Exit()
		{
			_statisticsView.VisibleChanged -= RefreshTimer;
			_personalGameStatsView.Hide();
		}

		public override void Complete()
		{
			PhotonPlayer[] playerList = PhotonNetwork.playerList;
			PhotonNetwork.room.SetFinalPlayers(playerList);
			ProcessWinners(playerList);
		}

		internal virtual void ProcessWinners(PhotonPlayer[] players)
		{
			if (_gameController.GameType == GameType.Personal)
			{
				PhotonPlayer[] array = players.OrderByScore();
				PhotonPlayer firstPlace = array[0];
				PhotonPlayer secondPlace = ((array.Length <= 1) ? null : array[1]);
				PhotonPlayer thirdPlace = ((array.Length <= 2) ? null : array[2]);
				WinPlayers winPlayers = new WinPlayers(firstPlace, secondPlace, thirdPlace);
				PhotonNetwork.room.SetWinPlayers(winPlayers);
			}
			else
			{
				int ctScore = PhotonNetwork.room.GetCtScore();
				int trScore = PhotonNetwork.room.GetTrScore();
				PhotonRoomExtension.SetWinTeam(winTeam: (ctScore == trScore) ? WinTeam.Draw() : ((ctScore <= trScore) ? WinTeam.Winner(Team.Tr) : WinTeam.Winner(Team.Ct)), room: PhotonNetwork.room);
			}
		}

		public override bool CanSpawnPlayer()
		{
			return true;
		}
	}
}
