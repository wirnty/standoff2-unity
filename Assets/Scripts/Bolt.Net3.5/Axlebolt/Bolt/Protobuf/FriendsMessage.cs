using ProtoBuf;

namespace Axlebolt.Bolt.Protobuf
{
	public class FriendsMessage
	{
		[ProtoContract]
		public enum OnlineStatus
		{
			[ProtoEnum(Value = 0)]
			StateOffline = 0,
			[ProtoEnum(Value = 1)]
			StateOnline = 1,
			[ProtoEnum(Value = 2)]
			StateBusy = 2,
			[ProtoEnum(Value = 3)]
			StateAway = 3,
			[ProtoEnum(Value = 4)]
			StateSnooze = 4,
			[ProtoEnum(Value = 5)]
			StateLookingToTrade = 5,
			[ProtoEnum(Value = 6)]
			StateLookingToPlay = 6
		}

		[ProtoContract]
		public enum RelationshipStatus
		{
			[ProtoEnum(Value = 0)]
			None = 0,
			[ProtoEnum(Value = 1)]
			Blocked = 1,
			[ProtoEnum(Value = 2)]
			RequestRecipient = 2,
			[ProtoEnum(Value = 3)]
			Friend = 3,
			[ProtoEnum(Value = 4)]
			RequestInitiator = 4,
			[ProtoEnum(Value = 5)]
			Ignored = 5
		}

		[ProtoContract]
		public enum MessageStatus
		{
			[ProtoEnum(Value = 0)]
			Sent = 0,
			[ProtoEnum(Value = 1)]
			Received = 1,
			[ProtoEnum(Value = 2)]
			Read = 2,
			[ProtoEnum(Value = 3)]
			SentFailed = 3
		}

		[ProtoContract]
		public class PlayerFriend
		{
			[ProtoMember(1)]
			public PlayerMessage.Player Player { get; set; }

			[ProtoMember(2)]
			public RelationshipStatus RelationshipStatus { get; set; }

			[ProtoMember(3)]
			public long LastRelationshipUpdate { get; set; }

			[ProtoMember(4)]
			public OnlineStatus OnlineStatus { get; set; }
		}

		[ProtoContract]
		public class FriendMessage
		{
			[ProtoMember(1)]
			public string Id { get; set; }

			[ProtoMember(2)]
			public string PlayerFrom { get; set; }

			[ProtoMember(3)]
			public string PlayerTo { get; set; }

			[ProtoMember(4)]
			public long Timestamp { get; set; }

			[ProtoMember(5)]
			public string Msg { get; set; }

			[ProtoMember(6)]
			public bool IsRead { get; set; }
		}
	}
}
