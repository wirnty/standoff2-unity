using System;
using System.Collections;
using System.Collections.Generic;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.State;
using JetBrains.Annotations;

namespace Axlebolt.Standoff.Game
{
	public class GameStateMachine
	{
		public delegate byte Router();

		public delegate void StateHandler();

		private static readonly Log Log = Log.Create(typeof(GameStateMachine));

		private readonly GameStateRouter _router;

		private readonly Dictionary<byte, IGameState> _gameStates = new Dictionary<byte, IGameState>();

		public IGameState GameState { get; private set; }

		public event StateHandler GameFinished;

		public event StateHandler GameInit;

		public GameStateMachine([NotNull] GameStateRouter router)
		{
			if (router == null)
			{
				throw new ArgumentNullException("router");
			}
			_router = router;
			_gameStates[byte.MaxValue] = new GameFinishedState();
		}

		public void Register(IGameState gameState)
		{
			_router.CheckRoutingExists(gameState.Id);
			_gameStates[gameState.Id] = gameState;
		}

		public void Setup(GameController gameController)
		{
			foreach (IGameState value in _gameStates.Values)
			{
				value.Setup(gameController);
			}
		}

		public IEnumerator GameLoop()
		{
			while (!InitIfNot())
			{
				yield return null;
			}
			while (true)
			{
				IGameState state = GameState;
				if (Update())
				{
					break;
				}
				if (state == GameState)
				{
					yield return GameState.Instruction();
				}
				else
				{
					yield return null;
				}
			}
		}

		private bool Update()
		{
			if (!PhotonNetwork.isMasterClient)
			{
				byte gameStateId = PhotonNetwork.room.GetGameStateId();
				if (GameState.Id != gameStateId)
				{
					GoTo(gameStateId);
				}
			}
			if (GameState.Id == byte.MaxValue)
			{
				OnGameFinished();
				return true;
			}
			GameState.Update();
			ExitIfCan();
			return false;
		}

		private bool InitIfNot()
		{
			if (GameState != null)
			{
				return true;
			}
			byte gameStateId = PhotonNetwork.room.GetGameStateId();
			if (gameStateId == 0)
			{
				if (!PhotonNetwork.isMasterClient)
				{
					return false;
				}
				RouteFirst();
			}
			else
			{
				GoTo(gameStateId);
			}
			OnGameInit();
			return true;
		}

		private void RouteFirst()
		{
			byte b = _router.RouteNext(0);
			if (b == 0)
			{
				Log.Error("First GameState is None?");
			}
			else
			{
				GoTo(b);
			}
		}

		private void RouteNext()
		{
			if (!PhotonNetwork.isMasterClient)
			{
				Log.Error("RouteNext available only on MasterClient");
				return;
			}
			byte toGameStateId = _router.RouteNext(GameState.Id);
			GoTo(toGameStateId);
		}

		private void GoTo(byte toGameStateId)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("Get state by Id({0})", toGameStateId));
			}
			IGameState value;
			if (!_gameStates.TryGetValue(toGameStateId, out value))
			{
				Log.Error(string.Format("Can't change game state to {0}, is not found", toGameStateId));
			}
			else
			{
				GoTo(value);
			}
		}

		private void GoTo([NotNull] IGameState toGameState)
		{
			if (toGameState == null)
			{
				throw new ArgumentNullException("toGameState");
			}
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("GoTo {0}", toGameState));
			}
			ExitState();
			GameState = toGameState;
			if (PhotonNetwork.isMasterClient)
			{
				PhotonNetwork.room.SetGameStateId(GameState.Id);
				GameState.Init();
			}
			GameState.Enter();
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("State changed to {0}", GameState.GetType().Name));
			}
		}

		private void ExitState()
		{
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("ExitState {0}", GameState));
			}
			IGameState gameState = GameState;
			if (gameState != null)
			{
				gameState.Exit();
			}
			if (PhotonNetwork.isMasterClient)
			{
				IGameState gameState2 = GameState;
				if (gameState2 != null)
				{
					gameState2.Complete();
				}
			}
		}

		private void ExitIfCan()
		{
			if (PhotonNetwork.isMasterClient && GameState.CheckExit())
			{
				RouteNext();
			}
		}

		private void OnGameFinished()
		{
			if (this.GameFinished != null)
			{
				this.GameFinished();
			}
		}

		private void OnGameInit()
		{
			if (this.GameInit != null)
			{
				this.GameInit();
			}
		}
	}
}
