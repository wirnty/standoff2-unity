using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Game.State
{
	public class GameFinishedState : IGameState
	{
		public byte Id
		{
			[CompilerGenerated]
			get
			{
				return byte.MaxValue;
			}
		}

		public void Setup([NotNull] GameController gameController)
		{
		}

		public void Init()
		{
		}

		public void Enter()
		{
		}

		public YieldInstruction Instruction()
		{
			throw new NotImplementedException();
		}

		public void Update()
		{
			throw new NotImplementedException();
		}

		public bool CheckExit()
		{
			throw new NotImplementedException();
		}

		public void Exit()
		{
			throw new NotImplementedException();
		}

		public void Complete()
		{
			throw new NotImplementedException();
		}

		public bool CanSpawnPlayer()
		{
			throw new NotImplementedException();
		}
	}
}
