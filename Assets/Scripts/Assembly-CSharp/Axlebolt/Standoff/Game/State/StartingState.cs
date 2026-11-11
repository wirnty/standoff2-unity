using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Game.UI;
using I2.Loc;

namespace Axlebolt.Standoff.Game.State
{
	public class StartingState : TimerGameState
	{
		private GameController _gameController;

		private GameStateView _gameStateView;

		public override byte Id
		{
			[CompilerGenerated]
			get
			{
				return 21;
			}
		}

		public StartingState(double duration)
			: base(duration)
		{
		}

		public override void Setup(GameController gameController)
		{
			_gameController = gameController;
			_gameStateView = gameController.GameView.GetView<GameStateView>();
		}

		public override void Enter()
		{
			_gameStateView.Show();
			if (_gameController.PlayerController != null)
			{
				_gameController.PlayerController.FreezeMovement();
			}
		}

		public override void Update()
		{
			_gameStateView.Text = ScriptLocalization.GameState.StartingGame + " " + StringUtils.FormatWithDots(base.TimeLeft);
		}

		public override void Exit()
		{
			_gameStateView.Hide();
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
