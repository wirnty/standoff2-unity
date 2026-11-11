using System;
using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.UI;
using I2.Loc;

namespace Axlebolt.Standoff.Game.State
{
	public class StatisticsState : TimerGameState
	{
		private GameController _gameController;

		private StatisticsView _statisticsView;

		public override byte Id
		{
			[CompilerGenerated]
			get
			{
				return 201;
			}
		}

		public StatisticsState(double duration)
			: base(duration)
		{
		}

		public override void Setup(GameController gameController)
		{
			_gameController = gameController;
			_statisticsView = _gameController.GameView.GetView<StatisticsView>();
		}

		public override void Enter()
		{
			_gameController.DestroyPlayer();
			PhotonPlayer[] finalPlayers = PhotonNetwork.room.GetFinalPlayers();
			WinPlayers winPlayers = PhotonNetwork.room.GetWinPlayers();
			_statisticsView.GameFinished(finalPlayers);
			if (_gameController.GameType == GameType.Personal)
			{
				_statisticsView.CtScore = "0";
				_statisticsView.TrScore = "0";
				_statisticsView.ScoreEnabled = false;
				if (winPlayers.FirstPlace.GetTeam() == Team.Ct)
				{
					_statisticsView.CtHighlightText = ScriptLocalization.Statistics.Winner;
				}
				else
				{
					_statisticsView.TrHighlighText = ScriptLocalization.Statistics.Winner;
				}
				_statisticsView.Show();
				return;
			}
			throw new NotSupportedException();
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
			_gameController.ExitGame();
		}

		public override void Complete()
		{
		}

		public override bool CanSpawnPlayer()
		{
			return false;
		}
	}
}
