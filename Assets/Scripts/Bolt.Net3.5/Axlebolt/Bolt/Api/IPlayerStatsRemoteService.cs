using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("PlayerStatsRemoteService")]
	public interface IPlayerStatsRemoteService
	{
		[Rpc("storeStats")]
		void StoreStats(PlayerStatsMessage.StorePlayerStat[] storeStats, PlayerStatsMessage.StoreAchievement[] storeAchievements);

		[Rpc("resetAllStats")]
		void ResetStats();

		[Rpc("getPlayerStats")]
		PlayerStatsMessage.Stats GetPlayerStats(string playerId);

		[Rpc("getGlobalStats")]
		PlayerStatsMessage.PlayerStat[] GetGlobalStats(int? historicDays);

		[Rpc("getNumberOfCurrentPlayers")]
		int? GetNumberOfCurrentPlayers();

		[Rpc("getStats")]
		PlayerStatsMessage.Stats GetStats();
	}
}
