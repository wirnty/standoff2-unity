using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Game.UI.NextLevel;
using Axlebolt.Standoff.Level;
using UnityEngine;

namespace Axlebolt.Standoff.Game.State
{
	public class NextLevelVotingResultState : TimerGameState
	{
		private GameController _gameController;

		private NextLevelVotingView _votingView;

		public override byte Id
		{
			[CompilerGenerated]
			get
			{
				return 203;
			}
		}

		public NextLevelVotingResultState(double duration)
			: base(duration)
		{
		}

		public override void Setup(GameController gameController)
		{
			_gameController = gameController;
			_votingView = gameController.GameView.GetView<NextLevelVotingView>();
		}

		public override void Init()
		{
			string[] levels = PhotonNetwork.room.GetVotingLevels();
			int[] array = new int[levels.Length];
			for (int i = 0; i < levels.Length; i++)
			{
				array[i] = PhotonNetwork.playerList.Count((PhotonPlayer player) => levels[i].Equals(player.GetNextLevelVote()));
			}
			PhotonNetwork.room.SetFinalLevelVotes(array);
			string[] winnerLevels = GetWinnerLevels();
			if (winnerLevels.Length == 0)
			{
				PhotonNetwork.room.SetNextLevel(PhotonNetwork.room.GetLevelName());
				return;
			}
			string nextLevel = winnerLevels[Random.Range(0, winnerLevels.Length)];
			PhotonNetwork.room.SetNextLevel(nextLevel);
		}

		private string[] GetWinnerLevels()
		{
			string[] votingLevels = PhotonNetwork.room.GetVotingLevels();
			int[] finalLevelVotes = PhotonNetwork.room.GetFinalLevelVotes();
			int num = finalLevelVotes.OrderByDescending((int x) => x).ElementAt(0);
			List<string> list = new List<string>();
			for (int i = 0; i < finalLevelVotes.Length; i++)
			{
				if (finalLevelVotes[i] == num)
				{
					list.Add(votingLevels[i]);
				}
			}
			return list.ToArray();
		}

		public override void Enter()
		{
			_gameController.DestroyPlayer();
			string[] votingLevels = PhotonNetwork.room.GetVotingLevels();
			int[] finalLevelVotes = PhotonNetwork.room.GetFinalLevelVotes();
			string nextLevel = PhotonNetwork.room.GetNextLevel();
			LevelDefinition[] array = LevelUtility.GetLevels(votingLevels).ToArray();
			LevelDefinition levelDefinition = array.First((LevelDefinition level) => level.name.Equals(nextLevel));
			_votingView.Init(array);
			_votingView.Frozen(finalLevelVotes);
			_votingView.HighLightWinner(nextLevel);
			_votingView.Text = levelDefinition.DisplayName;
		}

		public override void Update()
		{
			_votingView.TimeLeft = base.TimeLeft;
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
