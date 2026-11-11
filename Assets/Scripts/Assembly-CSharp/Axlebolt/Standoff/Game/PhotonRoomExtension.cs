using System;
using System.Collections;
using System.Linq;
using Axlebolt.Standoff.Core;
using ExitGames.Client.Photon;

namespace Axlebolt.Standoff.Game
{
	public static class PhotonRoomExtension
	{
		public const string TimeProp = "Time";

		public const string GameModeProp = "C0";

		public const string LevelNameProp = "C1";

		public const string GameStateProp = "C2";

		public const string CtScoreProp = "CtScore";

		public const string TrScoreProp = "TrScore";

		public const string NextLevelsProp = "NextLevels";

		public const string FinalNextLevelVotesProp = "FinalNextLevelVotes";

		public const string NextLevelProp = "NextLevel";

		public const string FinalPlayers = "FinalPlayers";

		public const string WinPlayers = "WinPlayers";

		public const string WinTeam = "WinTeam";

		public static readonly string[] CleanProperties = new string[10] { "Time", "C2", "CtScore", "TrScore", "NextLevels", "FinalNextLevelVotes", "NextLevel", "FinalPlayers", "WinPlayers", "WinTeam" };

		public static void SetTime(this Room room, double time)
		{
			room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Time", time } });
		}

		public static double GetTime(this Room room)
		{
			return (double)room.CustomProperties["Time"];
		}

		public static void SetGameStateId(this Room room, byte gameState)
		{
			room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "C2", gameState } });
		}

		public static byte GetGameStateId(this Room room)
		{
			object result;
			if (room.CustomProperties.TryGetNonNullValue("C2", out result))
			{
				return (byte)result;
			}
			return 0;
		}

		public static string GetLevelName(this Room room)
		{
			object result;
			if (room.CustomProperties.TryGetNonNullValue("C1", out result))
			{
				return (string)result;
			}
			throw new InvalidOperationException("LevelName is null");
		}

		public static string GetGameModeName(this Room room)
		{
			object result;
			if (room.CustomProperties.TryGetNonNullValue("C0", out result))
			{
				return (string)result;
			}
			throw new InvalidOperationException("GameMode is null");
		}

		public static void AddCtScore(this Room room)
		{
			int ctScore = room.GetCtScore();
			room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { 
			{
				"CtScore",
				ctScore + 1
			} });
		}

		public static int GetCtScore(this Room room)
		{
			object result;
			if (room.CustomProperties.TryGetNonNullValue("CtScore", out result))
			{
				return (int)result;
			}
			return 0;
		}

		public static void AddTrScore(this Room room)
		{
			int trScore = room.GetTrScore();
			room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { 
			{
				"TrScore",
				trScore + 1
			} });
		}

		public static int GetTrScore(this Room room)
		{
			object result;
			if (room.CustomProperties.TryGetNonNullValue("TrScore", out result))
			{
				return (int)result;
			}
			return 0;
		}

		public static void SetVotingLevels(this Room room, string[] levelNames)
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			for (int i = 0; i < levelNames.Length; i++)
			{
				hashtable[i] = levelNames[i];
			}
			room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "NextLevels", hashtable } });
		}

		public static string[] GetVotingLevels(this Room room)
		{
			object result;
			if (room.CustomProperties.TryGetNonNullValue("NextLevels", out result))
			{
				ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)result;
				string[] array = new string[hashtable.Count];
				for (int i = 0; i < hashtable.Count; i++)
				{
					array[i] = (string)hashtable[i];
				}
				return array;
			}
			throw new InvalidOperationException("Can't retrive vote levels, is empty");
		}

		public static void SetFinalLevelVotes(this Room room, int[] votes)
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			for (int i = 0; i < votes.Length; i++)
			{
				hashtable[i] = votes[i];
			}
			room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "FinalNextLevelVotes", hashtable } });
		}

		public static int[] GetFinalLevelVotes(this Room room)
		{
			object result;
			if (room.CustomProperties.TryGetNonNullValue("FinalNextLevelVotes", out result))
			{
				ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)result;
				int[] array = new int[hashtable.Count];
				foreach (DictionaryEntry item in hashtable)
				{
					array[(int)item.Key] = (int)item.Value;
				}
				return array;
			}
			throw new InvalidOperationException("Can't retrive final next level votes, is empty");
		}

		public static void SetNextLevel(this Room room, string nextLevel)
		{
			room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "NextLevel", nextLevel } });
		}

		public static string GetNextLevel(this Room room)
		{
			object result;
			if (room.CustomProperties.TryGetNonNullValue("NextLevel", out result))
			{
				return (string)result;
			}
			throw new InvalidOperationException("Can't retrive next level, is empty");
		}

		public static string GetNextLevelNullable(this Room room)
		{
			object result;
			if (room.CustomProperties.TryGetNonNullValue("NextLevel", out result))
			{
				return (string)result;
			}
			return null;
		}

		public static void SetFinalPlayers(this Room room, PhotonPlayer[] players)
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			foreach (PhotonPlayer photonPlayer in players)
			{
				ExitGames.Client.Photon.Hashtable allProperties = photonPlayer.AllProperties;
				string[] localProperties = PhotonGameExtension.LocalProperties;
				foreach (string key in localProperties)
				{
					allProperties.Remove(key);
				}
				hashtable[photonPlayer.ID] = allProperties;
			}
			room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "FinalPlayers", hashtable } });
		}

		public static PhotonPlayer[] GetFinalPlayers(this Room room)
		{
			object result;
			if (room.CustomProperties.TryGetNonNullValue("FinalPlayers", out result))
			{
				ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)result;
				PhotonPlayer[] array = new PhotonPlayer[hashtable.Count];
				int num = 0;
				foreach (DictionaryEntry item in hashtable)
				{
					int actorID = (int)item.Key;
					ExitGames.Client.Photon.Hashtable properties = (ExitGames.Client.Photon.Hashtable)item.Value;
					array[num++] = new PhotonPlayer(false, actorID, properties);
				}
				return array;
			}
			throw new InvalidOperationException("Can't retrive final players, is empty");
		}

		public static void SetWinPlayers(this Room room, WinPlayers winPlayers)
		{
			int[] value = (from player in winPlayers.GetAsArray()
				select player.ID).ToArray();
			room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "WinPlayers", value } });
		}

		public static WinPlayers GetWinPlayers(this Room room)
		{
			object value;
			if (room.CustomProperties.TryGetValue("WinPlayers", out value))
			{
				PhotonPlayer[] finalPlayers = room.GetFinalPlayers();
				int[] source = (int[])value;
				PhotonPlayer[] players = source.Select((int id) => finalPlayers.First((PhotonPlayer p) => p.ID == id)).ToArray();
				return new WinPlayers(players);
			}
			throw new InvalidOperationException("Can't retrive WinPlayers, is empty");
		}

		public static void SetWinTeam(this Room room, WinTeam winTeam)
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable["isDraw"] = winTeam.IsDraw;
			hashtable["isGiveUp"] = winTeam.IsGiveUp;
			hashtable["winnerTeam"] = winTeam.WinnerTeam;
			ExitGames.Client.Photon.Hashtable value = hashtable;
			room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "WinTeam", value } });
		}

		public static WinTeam GetWinTeam(this Room room)
		{
			object value;
			if (room.CustomProperties.TryGetValue("WinTeam", out value))
			{
				ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)value;
				return new WinTeam((bool)hashtable["isDraw"], (bool)hashtable["isGiveUp"], (Team)hashtable["winnerTeam"]);
			}
			throw new InvalidOperationException("Can't retrive WinTeam, is empty");
		}
	}
}
