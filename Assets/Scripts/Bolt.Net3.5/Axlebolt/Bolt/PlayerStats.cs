using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt
{
	public class PlayerStats
	{
		private readonly Dictionary<string, PlayerStatsMessage.PlayerStat> _stats;

		private readonly Dictionary<string, PlayerStatsMessage.StorePlayerStat> _changedStats;

		public int Count
		{
			[CompilerGenerated]
			get
			{
				return _stats.Count;
			}
		}

		public PlayerStats(PlayerStatsMessage.Stats stats)
		{
			if (stats == null)
			{
				throw new ArgumentNullException("stats");
			}
			_stats = new Dictionary<string, PlayerStatsMessage.PlayerStat>();
			_changedStats = new Dictionary<string, PlayerStatsMessage.StorePlayerStat>();
			if (stats.Stat == null)
			{
				return;
			}
			foreach (PlayerStatsMessage.PlayerStat item in stats.Stat)
			{
				_stats[item.Name] = item;
			}
		}

		internal void SetStats(PlayerStatsMessage.Stats stats)
		{
			if (stats == null)
			{
				throw new ArgumentNullException("stats");
			}
			_stats.Clear();
			if (stats.Stat != null)
			{
				foreach (PlayerStatsMessage.PlayerStat item in stats.Stat)
				{
					_stats[item.Name] = item;
				}
			}
			foreach (KeyValuePair<string, PlayerStatsMessage.StorePlayerStat> changedStat in _changedStats)
			{
				if (_stats[changedStat.Key].Type == PlayerStatsMessage.StatDefType.Int)
				{
					_stats[changedStat.Key].IntValue = changedStat.Value.StoreInt;
				}
				else
				{
					_stats[changedStat.Key].FloatValue = changedStat.Value.StoreFloat;
				}
			}
		}

		public float GetFloatStat(string apiName)
		{
			return FindStat(apiName, PlayerStatsMessage.StatDefType.Float, PlayerStatsMessage.StatDefType.Avgrate).FloatValue;
		}

		public int GetIntStat(string apiName)
		{
			return FindStat(apiName, default(PlayerStatsMessage.StatDefType)).IntValue;
		}

		public void SetFloatStat(string apiName, float value)
		{
			FindStat(apiName, PlayerStatsMessage.StatDefType.Float).FloatValue = value;
			_changedStats[apiName] = new PlayerStatsMessage.StorePlayerStat
			{
				Name = apiName,
				StoreFloat = value
			};
		}

		public void SetIntStat(string apiName, int value)
		{
			FindStat(apiName, default(PlayerStatsMessage.StatDefType)).IntValue = value;
			_changedStats[apiName] = new PlayerStatsMessage.StorePlayerStat
			{
				Name = apiName,
				StoreInt = value
			};
		}

		public void IncrementStat(string apiName)
		{
			SetIntStat(apiName, GetIntStat(apiName) + 1);
		}

		public void UpdateAvgRateStat(string apiName, float countThisSession, double sessionLength)
		{
			PlayerStatsMessage.PlayerStat playerStat = FindStat(apiName, PlayerStatsMessage.StatDefType.Avgrate);
			float floatValue = playerStat.FloatValue;
			float window = playerStat.Window;
			floatValue = (playerStat.FloatValue = (float)((double)floatValue - (double)floatValue * sessionLength / (double)window + (double)countThisSession * sessionLength / (double)window));
			_changedStats[apiName] = new PlayerStatsMessage.StorePlayerStat
			{
				Name = apiName,
				StoreFloat = floatValue
			};
		}

		private PlayerStatsMessage.PlayerStat FindStat(string apiName, params PlayerStatsMessage.StatDefType[] types)
		{
			if (!_stats.ContainsKey(apiName))
			{
				return new PlayerStatsMessage.PlayerStat() { Name = apiName, Type = PlayerStatsMessage.StatDefType.Int };
			}
			PlayerStatsMessage.PlayerStat stat = _stats[apiName];
			if (types.Any((PlayerStatsMessage.StatDefType type) => stat.Type == type))
			{
				return stat;
			}
			string[] value = types.Select((PlayerStatsMessage.StatDefType type) => type.ToString()).ToArray();
			throw new BoltPlayerStatsException(string.Format("Stat {0} is not {1}", apiName, string.Join(",", value)));
		}

		internal PlayerStatsMessage.StorePlayerStat[] GetAndRemoveChanges()
		{
			PlayerStatsMessage.StorePlayerStat[] result = _changedStats.Values.ToArray();
			_changedStats.Clear();
			return result;
		}
	}
}
