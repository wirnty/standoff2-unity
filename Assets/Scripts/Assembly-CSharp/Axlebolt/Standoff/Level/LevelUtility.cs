using System;
using System.Linq;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using UnityEngine;

namespace Axlebolt.Standoff.Level
{
	public class LevelUtility
	{
		private static readonly Log Log = Log.Create(typeof(LevelUtility));

		public static LevelDefinition[] GetLevels(string gameMode)
		{
			return (from level in GetLevels()
				where level.GameModes != null && level.GameModes.Contains(gameMode)
				select level).ToArray();
		}

		public static LevelDefinition GetRandomLevel(string gameMode)
		{
			LevelDefinition[] levels = GetLevels(gameMode);
			return levels[UnityEngine.Random.Range(0, levels.Length)];
		}

		public static LevelDefinition[] GetLevels(string[] levelNames)
		{
			return (from level in GetLevels()
				where levelNames.Contains(level.name)
				select level).ToArray();
		}

		public static LevelDefinition GetLevel(string levelName)
		{
			LevelDefinition levelDefinition = ResourcesUtility.Load<LevelDefinition>("Levels/" + levelName);
			if (IsValidLevel(levelDefinition))
			{
				return levelDefinition;
			}
			throw new ArgumentException(string.Format("{0} is not valid", levelName));
		}

		public static LevelDefinition[] GetLevels()
		{
			return Resources.LoadAll<LevelDefinition>("Levels").Where(IsValidLevel).ToArray();
		}

		public static bool IsValidLevel(LevelDefinition levelDefinition)
		{
			if (string.IsNullOrEmpty(levelDefinition.SceneName))
			{
				Log.Error(string.Format("Invalid {0}, SceneName is empty", levelDefinition.Name));
				return false;
			}
			if (levelDefinition.CtCharacters == null || levelDefinition.CtCharacters.Skins.Length == 0)
			{
				Log.Error(string.Format("Invalid {0}, CtCharacters is empty", levelDefinition.Name));
				return false;
			}
			if (levelDefinition.TrCharacters == null || levelDefinition.TrCharacters.Skins.Length == 0)
			{
				Log.Error(string.Format("Invalid {0}, TrCharacters is empty", levelDefinition.Name));
				return false;
			}
			return true;
		}
	}
}
