using UnityEngine;

namespace Axlebolt.Standoff.Game.State
{
	public interface IGameState
	{
		byte Id { get; }

		void Setup(GameController gameController);

		void Init();

		void Enter();

		YieldInstruction Instruction();

		void Update();

		bool CheckExit();

		void Exit();

		void Complete();

		bool CanSpawnPlayer();
	}
}
