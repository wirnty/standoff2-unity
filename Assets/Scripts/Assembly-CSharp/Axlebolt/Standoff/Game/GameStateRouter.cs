using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Axlebolt.Standoff.Game
{
	public class GameStateRouter
	{
		public delegate byte Router();

		private readonly byte _firstGameStateId;

		private readonly Dictionary<byte, Router> _routers;

		private GameStateRouter(byte gameStateId, [NotNull] Router router)
		{
			if (router == null)
			{
				throw new ArgumentNullException("router");
			}
			_firstGameStateId = gameStateId;
			Dictionary<byte, Router> dictionary = new Dictionary<byte, Router>();
			dictionary[gameStateId] = router;
			_routers = dictionary;
		}

		public static GameStateRouter Create(byte gameStateId, Router router)
		{
			return new GameStateRouter(gameStateId, router);
		}

		public GameStateRouter Route(byte gameStateId, Router router)
		{
			_routers[gameStateId] = router;
			return this;
		}

		internal byte RouteNext(byte currentStateId)
		{
			return (currentStateId != 0) ? _routers[currentStateId]() : _firstGameStateId;
		}

		public void CheckRoutingExists(byte gameStateId)
		{
			if (!_routers.ContainsKey(gameStateId))
			{
				throw new InvalidOperationException(string.Format("GameState ({0}) not registered in routing", gameStateId));
			}
		}
	}
}
