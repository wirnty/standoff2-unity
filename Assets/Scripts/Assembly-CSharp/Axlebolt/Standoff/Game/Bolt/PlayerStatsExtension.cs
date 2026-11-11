using System;
using System.Collections.Generic;
using System.Linq;
using Axlebolt.Bolt;
using Axlebolt.Standoff.Inventory;

namespace Axlebolt.Standoff.Game.Bolt
{
	public static class PlayerStatsExtension
	{
		public const string Kills = "kills";

		public const string Deaths = "deaths";

		public const string Assists = "assists";

		public const string Shots = "shots";

		public const string Hits = "hits";

		public const string Headshots = "headshots";

		public const string Damage = "damage";

		public const string GamesPlayed = "games_played";

		public const string GunKills = "kills";

		public const string GunShots = "shots";

		public const string GunHits = "hits";

		public const string GunHeadshots = "headshots";

		public const string GunDamage = "damage";

		private static readonly HashSet<string> SupportedGameModes;

		static PlayerStatsExtension()
		{
			SupportedGameModes = new HashSet<string>();
			SupportedGameModes.Add("DeathMatch");
		}

		private static string WrapMode(string apiName, string gameMode)
		{
			return gameMode.ToLower() + "_" + apiName;
		}

		private static string WrapModeGun(string apiName, string gameMode, WeaponId weaponId)
		{
			return "gun_" + gameMode.ToLower() + "_" + weaponId.ToString().ToLower() + "_" + apiName;
		}

		public static int GetKills(this PlayerStats stats)
		{
			return ((IEnumerable<string>)SupportedGameModes).Sum((Func<string, int>)stats.GetKills);
		}

		public static int GetDeaths(this PlayerStats stats)
		{
			return ((IEnumerable<string>)SupportedGameModes).Sum((Func<string, int>)stats.GetDeaths);
		}

		public static int GetAssists(this PlayerStats stats)
		{
			return ((IEnumerable<string>)SupportedGameModes).Sum((Func<string, int>)stats.GetAssists);
		}

		public static int GetShots(this PlayerStats stats)
		{
			return ((IEnumerable<string>)SupportedGameModes).Sum((Func<string, int>)stats.GetShots);
		}

		public static int GetHits(this PlayerStats stats)
		{
			return ((IEnumerable<string>)SupportedGameModes).Sum((Func<string, int>)stats.GetHits);
		}

		public static int GetHeadshots(this PlayerStats stats)
		{
			return ((IEnumerable<string>)SupportedGameModes).Sum((Func<string, int>)stats.GetHeadshots);
		}

		public static int GetGamesPlayed(this PlayerStats stats)
		{
			return ((IEnumerable<string>)SupportedGameModes).Sum((Func<string, int>)stats.GetGamesPlayed);
		}

		public static int GetKills(this PlayerStats stats, string gameMode)
		{
			return stats.GetIntStat(WrapMode("kills", gameMode));
		}

		public static void IncKills(this PlayerStats stats, string gameMode)
		{
			stats.IncrementStat(WrapMode("kills", gameMode));
		}

		public static int GetDeaths(this PlayerStats stats, string gameMode)
		{
			return stats.GetIntStat(WrapMode("deaths", gameMode));
		}

		public static void IncDeaths(this PlayerStats stats, string gameMode)
		{
			stats.IncrementStat(WrapMode("deaths", gameMode));
		}

		public static int GetAssists(this PlayerStats stats, string gameMode)
		{
			return stats.GetIntStat(WrapMode("assists", gameMode));
		}

		public static void IncAssists(this PlayerStats stats, string gameMode)
		{
			stats.IncrementStat(WrapMode("assists", gameMode));
		}

		public static int GetShots(this PlayerStats stats, string gameMode)
		{
			return stats.GetIntStat(WrapMode("shots", gameMode));
		}

		public static void IncShots(this PlayerStats stats, string gameMode)
		{
			stats.IncrementStat(WrapMode("shots", gameMode));
		}

		public static int GetHits(this PlayerStats stats, string gameMode)
		{
			return stats.GetIntStat(WrapMode("hits", gameMode));
		}

		public static void IncHits(this PlayerStats stats, string gameMode)
		{
			stats.IncrementStat(WrapMode("hits", gameMode));
		}

		public static int GetHeadshots(this PlayerStats stats, string gameMode)
		{
			return stats.GetIntStat(WrapMode("headshots", gameMode));
		}

		public static void IncHeadshots(this PlayerStats stats, string gameMode)
		{
			stats.IncrementStat(WrapMode("headshots", gameMode));
		}

		public static void AddDamage(this PlayerStats stats, string gameMode, int damage)
		{
			string apiName = WrapMode("damage", gameMode);
			stats.SetIntStat(apiName, stats.GetIntStat(apiName) + damage);
		}

		public static int GetGamesPlayed(this PlayerStats stats, string gameMode)
		{
			return stats.GetIntStat(WrapMode("games_played", gameMode));
		}

		public static void IncGamesPlayed(this PlayerStats stats, string gameMode)
		{
			stats.IncrementStat(WrapMode("games_played", gameMode));
		}

		public static float GetAccuracyPercentage(this PlayerStats stats)
		{
			return (stats.GetShots() <= 0) ? 0f : ((float)stats.GetHits() / (float)stats.GetShots() * 100f);
		}

		public static float GetHeadshotsPercentage(this PlayerStats stats)
		{
			return (stats.GetKills() <= 0) ? 0f : ((float)stats.GetHeadshots() / (float)stats.GetKills() * 100f);
		}

		public static float GetKd(this PlayerStats stats)
		{
			return (stats.GetDeaths() <= 0) ? 0f : ((float)stats.GetKills() / (float)stats.GetDeaths());
		}

		public static void IncGunKills(this PlayerStats stats, string gameMode, WeaponId weaponId)
		{
			stats.IncrementStat(WrapModeGun("kills", gameMode, weaponId));
		}

		public static int GetGunKills(this PlayerStats stats, string gameMode, WeaponId weaponId)
		{
			return stats.GetIntStat(WrapModeGun("kills", gameMode, weaponId));
		}

		public static int GetGunKills(this PlayerStats stats, WeaponId weaponId)
		{
			return SupportedGameModes.Sum((string gameMode) => stats.GetIntStat(WrapModeGun("kills", gameMode, weaponId)));
		}

		public static void IncGunShots(this PlayerStats stats, string gameMode, WeaponId weaponId)
		{
			stats.IncrementStat(WrapModeGun("shots", gameMode, weaponId));
		}

		public static int GetGunShots(this PlayerStats stats, string gameMode, WeaponId weaponId)
		{
			return stats.GetIntStat(WrapModeGun("shots", gameMode, weaponId));
		}

		public static int GetGunShots(this PlayerStats stats, WeaponId weaponId)
		{
			return SupportedGameModes.Sum((string gameMode) => stats.GetIntStat(WrapModeGun("shots", gameMode, weaponId)));
		}

		public static void IncGunHits(this PlayerStats stats, string gameMode, WeaponId weaponId)
		{
			stats.IncrementStat(WrapModeGun("hits", gameMode, weaponId));
		}

		public static int GetGunHits(this PlayerStats stats, string gameMode, WeaponId weaponId)
		{
			return stats.GetIntStat(WrapModeGun("hits", gameMode, weaponId));
		}

		public static int GetGunHits(this PlayerStats stats, WeaponId weaponId)
		{
			return SupportedGameModes.Sum((string gameMode) => stats.GetIntStat(WrapModeGun("hits", gameMode, weaponId)));
		}

		public static void IncGunHeadshots(this PlayerStats stats, string gameMode, WeaponId weaponId)
		{
			stats.IncrementStat(WrapModeGun("headshots", gameMode, weaponId));
		}

		public static int GetGunHeadshots(this PlayerStats stats, string gameMode, WeaponId weaponId)
		{
			return stats.GetIntStat(WrapModeGun("headshots", gameMode, weaponId));
		}

		public static int GetGunHeadshots(this PlayerStats stats, WeaponId weaponId)
		{
			return SupportedGameModes.Sum((string gameMode) => stats.GetIntStat(WrapModeGun("headshots", gameMode, weaponId)));
		}

		public static float GetGunAccuracyPercentage(this PlayerStats stats, WeaponId weaponId)
		{
			int gunShots = stats.GetGunShots(weaponId);
			return (gunShots <= 0) ? 0f : ((float)stats.GetGunHits(weaponId) / (float)gunShots * 100f);
		}

		public static void AddGunDamage(this PlayerStats stats, string gameMode, WeaponId weaponId, int damage)
		{
			string apiName = WrapModeGun("damage", gameMode, weaponId);
			stats.SetIntStat(apiName, stats.GetIntStat(apiName) + damage);
		}
	}
}
