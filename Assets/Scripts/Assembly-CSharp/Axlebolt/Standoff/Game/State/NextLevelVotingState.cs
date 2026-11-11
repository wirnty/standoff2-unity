using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Game.UI.NextLevel;
using Axlebolt.Standoff.Level;
using I2.Loc;
using UnityEngine;

namespace Axlebolt.Standoff.Game.State
{
	public class NextLevelVotingState : TimerGameState
	{
		private GameController _gameController;

		private NextLevelVotingView _nextLevelVotingView;

		public override byte Id
		{
			[CompilerGenerated]
			get
			{
				return 202;
			}
		}

		public NextLevelVotingState(double duration)
			: base(duration)
		{
		}

		public override void Setup(GameController gameController)
		{
			_gameController = gameController;
			_nextLevelVotingView = _gameController.GameView.GetView<NextLevelVotingView>();
		}

		public override void Init()
		{
			base.Init();
			PhotonNetwork.room.SetVotingLevels(GetNextLevels());
		}

		private string[] GetNextLevels()
		{
			string gameModeName = PhotonNetwork.room.GetGameModeName();
			string levelName = PhotonNetwork.room.GetLevelName();
			List<string> list = (from level in LevelUtility.GetLevels(gameModeName)
				where !level.name.Equals(levelName)
				select level.name).ToList();
			while (list.Count > 5)
			{
				list.RemoveAt(Random.Range(0, list.Count));
			}
			return list.OrderBy((string level) => level).ToArray();
		}

		public override void Enter()
		{
			_gameController.DestroyPlayer();
			string[] votingLevels = PhotonNetwork.room.GetVotingLevels();
			LevelDefinition[] levels = LevelUtility.GetLevels(votingLevels).ToArray();
			_nextLevelVotingView.Init(levels);
			_nextLevelVotingView.Show();
			_nextLevelVotingView.Text = ScriptLocalization.NextLevelVote.VoteEndsIn;
		}

		public override void Update()
		{
			_nextLevelVotingView.TimeLeft = base.TimeLeft;
		}

		public override void Exit()
		{
			_nextLevelVotingView.Hide();
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
