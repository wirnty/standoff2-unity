using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game.State
{
	public abstract class TimerGameState : IGameState
	{
		private readonly double _duration;

		public abstract byte Id { get; }

		protected double TimeLeft
		{
			[CompilerGenerated]
			get
			{
				return GameStateUtility.GetTimeLeft(_duration);
			}
		}

		protected TimerGameState(double duration)
		{
			_duration = duration;
		}

		public abstract void Setup(GameController gameController);

		public virtual void Init()
		{
			PhotonNetwork.room.SetTime(PhotonNetwork.time);
		}

		public abstract void Enter();

		public virtual YieldInstruction Instruction()
		{
			return new WaitForSeconds(0.2f);
		}

		public abstract void Update();

		public virtual bool CheckExit()
		{
			return GameStateUtility.GetTimePassed() > _duration;
		}

		public abstract void Exit();

		public abstract void Complete();

		public abstract bool CanSpawnPlayer();
	}
}
