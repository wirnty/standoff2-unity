using System;
using System.Linq;
using Axlebolt.Standoff.Core;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public class SpawnPointManager
	{
		public enum Type
		{
			RandomSpawnArea = 0,
			TeamRandomSpawnArea = 1
		}

		private readonly Type _type;

		private readonly SpawnArea[] _areas;

		private readonly SpawnArea[] _trAreas;

		private readonly SpawnArea[] _ctAreas;

		public SpawnPointManager(Type type = Type.RandomSpawnArea)
		{
			_type = type;
			SpawnArea[] array = UnityEngine.Object.FindObjectsOfType<SpawnArea>();
			if (_type == Type.RandomSpawnArea)
			{
				_areas = array;
				if (_areas.Length == 0)
				{
					throw new InvalidOperationException("SpawnArea's not found");
				}
				return;
			}
			_trAreas = array.Where((SpawnArea area) => area.Team == Team.Tr).ToArray();
			_ctAreas = array.Where((SpawnArea area) => area.Team == Team.Ct).ToArray();
			if (_trAreas.Length != 0 && _ctAreas.Length != 0)
			{
				return;
			}
			throw new InvalidOperationException("SpawnArea's not found");
		}

		public SpawnPoint GetSpawnPoint(Team team)
		{
			if (team == Team.None)
			{
				throw new ArgumentException("team");
			}
			if (_type == Type.RandomSpawnArea)
			{
				return RandomPoint(_areas);
			}
			return RandomPoint((team != Team.Ct) ? _trAreas : _ctAreas);
		}

		private static SpawnPoint RandomPoint(SpawnArea[] areas)
		{
			int num = UnityEngine.Random.Range(0, areas.Length);
			return areas[num].RandomPoint();
		}
	}
}
