using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Game.UI;
using I2.Loc;

namespace Axlebolt.Standoff.Game.State
{
	public class WarmUpState : TimerGameState
	{
		private GameStateView _gameStateView;

		public override byte Id
		{
			[CompilerGenerated]
			get
			{
				return 11;
			}
		}

		public WarmUpState(double duration)
			: base(duration)
		{
		}

		public override void Setup(GameController gameController)
		{
			_gameStateView = gameController.GameView.GetView<GameStateView>();
		}

		public override void Enter()
		{
			_gameStateView.Show();
		}

		public override void Update()
		{
			_gameStateView.Text = ScriptLocalization.GameState.WarmUp + " " + StringUtils.FormatTwoDigit(base.TimeLeft);
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
			return true;
		}
	}
}
