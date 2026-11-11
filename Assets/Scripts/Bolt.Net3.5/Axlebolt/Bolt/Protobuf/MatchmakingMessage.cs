using System.Collections.Generic;
using ProtoBuf;

namespace Axlebolt.Bolt.Protobuf
{
	public class MatchmakingMessage
	{
		[ProtoContract]
		public enum LobbyType
		{
			[ProtoEnum(Value = 0)]
			Private = 0,
			[ProtoEnum(Value = 1)]
			FriendsOnly = 1,
			[ProtoEnum(Value = 2)]
			Public = 2,
			[ProtoEnum(Value = 3)]
			Invisible = 3
		}

		[ProtoContract]
		public enum LobbyDistanceFilter
		{
			[ProtoEnum(Value = 0)]
			Close = 0,
			[ProtoEnum(Value = 1)]
			Default = 1,
			[ProtoEnum(Value = 2)]
			Far = 2,
			[ProtoEnum(Value = 3)]
			Worldwide = 3
		}

		[ProtoContract]
		public class GameServer
		{
			[ProtoMember(1)]
			public string Id { get; set; }

			[ProtoMember(2)]
			public string Ip { get; set; }

			[ProtoMember(3)]
			public int Port { get; set; }
		}

		[ProtoContract]
		public class GameServerDetails
		{
			[ProtoMember(1)]
			public string Id { get; set; }

			[ProtoMember(2)]
			public GameServer GameServer { get; set; }

			[ProtoMember(3)]
			public string Map { get; set; }

			[ProtoMember(4)]
			public int CurrentPlayers { get; set; }

			[ProtoMember(5)]
			public int MaxPlayers { get; set; }

			[ProtoMember(6)]
			public int BotPlayers { get; set; }

			[ProtoMember(7)]
			public bool RequirePassword { get; set; }

			[ProtoMember(8)]
			public string Version { get; set; }

			[ProtoMember(9)]
			public bool SuccessfulResponse { get; set; }

			[ProtoMember(10)]
			public bool DoNotRefresh { get; set; }
		}

		[ProtoContract]
		public class Lobby
		{
			[ProtoMember(1)]
			public string Id { get; set; }

			[ProtoMember(2)]
			public string OwnerPlayerId { get; set; }

			[ProtoMember(3)]
			public string Name { get; set; }

			[ProtoMember(4)]
			public LobbyType LobbyType { get; set; }

			[ProtoMember(5)]
			public bool Joinable { get; set; }

			[ProtoMember(6)]
			public int CurrentMembers { get; set; }

			[ProtoMember(7)]
			public int MaxMembers { get; set; }

			[ProtoMember(8)]
			public Dictionary<string, string> Data { get; set; }

			[ProtoMember(9)]
			public GameServer GameServer { get; set; }
		}
	}
}
