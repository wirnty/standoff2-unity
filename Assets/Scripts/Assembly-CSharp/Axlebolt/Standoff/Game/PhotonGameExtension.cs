using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Player;
using ExitGames.Client.Photon;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public static class PhotonGameExtension
	{
		private class PlayerComparer : IComparer<PhotonPlayer>
		{
			public int Compare(PhotonPlayer x, PhotonPlayer y)
			{
				return (x.GetScore() != y.GetScore()) ? x.GetScore().CompareTo(y.GetScore()) : y.GetDeath().CompareTo(x.GetDeath());
			}
		}

		public const string TeamPlayerProp = "team";

		public const string PingProp = "ping";

		public const string KillsProp = "kills";

		public const string AssistsProp = "assists";

		public const string DeathProp = "death";

		public const string ScoreProp = "score";

		public const string NextLevelVoteProp = "NextLevelVoteView";

		public const string AvatarProp = "avatar";

		public const string HitsProp = "hits";

		private const float UpdateInterval = 5f;

		public static readonly string[] Properties = new string[7] { "team", "kills", "assists", "death", "score", "NextLevelVoteView", "ping" };

		public static readonly string[] LocalProperties = new string[1] { "hits" };

		public static IEnumerator UpdatePing()
		{
			while (true)
			{
				if (!PhotonNetwork.connectedAndReady)
				{
					yield return new WaitForSeconds(5f);
				}
				PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { 
				{
					"ping",
					PhotonNetwork.GetPing()
				} });
				yield return new WaitForSeconds(5f);
			}
		}

		public static int GetPing(this PhotonPlayer player)
		{
			object value;
			if (player.CustomProperties.TryGetValue("ping", out value))
			{
				return (int)value;
			}
			return 0;
		}

		public static void SetTeam(this PhotonPlayer player, Team team)
		{
			Team team2 = player.GetTeam();
			if (team2 != team)
			{
				player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { 
				{
					"team",
					(byte)team
				} });
			}
		}

		public static Team GetTeam(this PhotonPlayer player)
		{
			object result;
			if (player.CustomProperties.TryGetNonNullValue("team", out result))
			{
				return (Team)result;
			}
			return Team.None;
		}

		public static void AddScore(this PhotonPlayer player, int scoreToAddToCurrent)
		{
			int score = player.GetScore();
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { 
			{
				"score",
				score + scoreToAddToCurrent
			} });
		}

		public static int GetScore(this PhotonPlayer player)
		{
			object result;
			if (player.CustomProperties.TryGetNonNullValue("score", out result))
			{
				return (int)result;
			}
			return 0;
		}

		public static void AddKill(this PhotonPlayer player)
		{
			int kills = player.GetKills();
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { 
			{
				"kills",
				kills + 1
			} });
		}

		public static int GetKills(this PhotonPlayer player)
		{
			object result;
			if (player.CustomProperties.TryGetNonNullValue("kills", out result))
			{
				return (int)result;
			}
			return 0;
		}

		public static void AddAssist(this PhotonPlayer player)
		{
			int assists = player.GetAssists();
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { 
			{
				"assists",
				assists + 1
			} });
		}

		public static int GetAssists(this PhotonPlayer player)
		{
			object result;
			if (player.CustomProperties.TryGetNonNullValue("assists", out result))
			{
				return (int)result;
			}
			return 0;
		}

		public static void AddDeath(this PhotonPlayer player)
		{
			int death = player.GetDeath();
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { 
			{
				"death",
				death + 1
			} });
		}

		public static int GetDeath(this PhotonPlayer player)
		{
			return player.GetProperty("death", 0);
		}

		public static void SetNextLevelVote(this PhotonPlayer player, string levelName)
		{
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "NextLevelVoteView", levelName } });
		}

		public static string GetNextLevelVote(this PhotonPlayer player)
		{
			object result;
			if (player.CustomProperties.TryGetNonNullValue("NextLevelVoteView", out result))
			{
				return (string)result;
			}
			return null;
		}

		public static void SetAvatar(this PhotonPlayer player, byte[] avatar)
		{
			player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "avatar", avatar } });
		}

		public static byte[] GetAvatar(this PhotonPlayer player)
		{
			object result;
			if (player.CustomProperties.TryGetNonNullValue("avatar", out result))
			{
				return (byte[])result;
			}
			return null;
		}

		public static PhotonPlayer[] GetByTeam(this PhotonPlayer[] players, Team team, bool excludeCurrentPlayer = false)
		{
			return players.Where((PhotonPlayer player) => (!excludeCurrentPlayer || !object.Equals(player, PhotonNetwork.player)) && player.GetTeam() == team).ToArray();
		}

		public static int GetCtPlayersCount(this PhotonPlayer[] players)
		{
			return players.Count((PhotonPlayer player) => player.GetTeam() == Team.Ct);
		}

		public static int GetTrPlayersCount(this PhotonPlayer[] players)
		{
			return players.Count((PhotonPlayer player) => player.GetTeam() == Team.Tr);
		}

		public static PhotonPlayer[] OrderByScore(this PhotonPlayer[] players)
		{
			return players.OrderByDescending((PhotonPlayer player) => player, new PlayerComparer()).ToArray();
		}
	}
}
