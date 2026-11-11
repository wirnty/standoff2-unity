using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Axlebolt.Bolt.Api;
using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt
{
	public class BoltPlayerStats : BoltService<BoltPlayerStats>
	{
		private readonly IPlayerStatsRemoteService _statsRemoteService;

		public int StatCount
		{
			[CompilerGenerated]
			get
			{
				return Stats.Count;
			}
		}

		public PlayerStats Stats { get; private set; }

		public BoltPlayerStats()
		{
			_statsRemoteService = BoltApi.Instance.Create<IPlayerStatsRemoteService>();
		}

		public float GetFloatStat(string apiName)
		{
			return Stats.GetFloatStat(apiName);
		}

		public int GetIntStat(string apiName)
		{
			return Stats.GetIntStat(apiName);
		}

		public void SetFloatStat(string apiName, float value)
		{
			Stats.SetFloatStat(apiName, value);
		}

		public void SetIntStat(string apiName, int value)
		{
			Stats.SetIntStat(apiName, value);
		}

		public void IncrementStat(string apiName)
		{
			Stats.IncrementStat(apiName);
		}

		public void UpdateAvgRateStat(string apiName, float countThisSession, double sessionLength)
		{
			Stats.UpdateAvgRateStat(apiName, countThisSession, sessionLength);
		}

		public Task StoreStats()
		{
			PlayerStatsMessage.StorePlayerStat[] changes = Stats.GetAndRemoveChanges();
			PlayerStatsMessage.StorePlayerStat[] array = changes;
			foreach (PlayerStatsMessage.StorePlayerStat storePlayerStat in array)
			{
				Logger.Debug(string.Format("{0} {1}", storePlayerStat.Name, storePlayerStat.StoreInt));
			}
			return BoltApi.Instance.Async(delegate
			{
				_statsRemoteService.StoreStats(changes, new PlayerStatsMessage.StoreAchievement[0]);
			});
		}

		public Task ResetStats()
		{
			return BoltApi.Instance.Async(delegate
			{
				_statsRemoteService.ResetStats();
			});
		}

		internal override void Load()
		{
			Stats = new PlayerStats(new PlayerStatsMessage.Stats());
		}
	}
}
