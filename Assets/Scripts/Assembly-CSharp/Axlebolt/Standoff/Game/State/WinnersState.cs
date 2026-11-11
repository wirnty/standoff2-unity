using System;
using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Game.UI.Winners;

namespace Axlebolt.Standoff.Game.State
{
	public class WinnersState : TimerGameState
	{
		private GameController _gameController;

		private WinnersView _winnersView;

		public override byte Id
		{
			[CompilerGenerated]
			get
			{
				return 200;
			}
		}

		public WinnersState(double duration)
			: base(duration)
		{
		}

		public override void Setup(GameController gameController)
		{
			_gameController = gameController;
			_winnersView = gameController.GameView.GetView<WinnersView>();
		}

		public override void Enter()
		{
			_gameController.DestroyPlayer();
			if (_gameController.GameType == GameType.Personal)
			{
				WinPlayers winPlayers = PhotonNetwork.room.GetWinPlayers();
				_winnersView.Show(winPlayers);
				return;
			}
			throw new NotSupportedException();
		}

		public override void Update()
		{
		}

		public override void Exit()
		{
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
