using ProtoBuf;

namespace Axlebolt.Bolt.Protobuf
{
	public class PlayerMessage
	{
		[ProtoContract]
		public enum AuthType
		{
			[ProtoEnum(Value = 0)]
			Test = 0,
			[ProtoEnum(Value = 1)]
			Guest = 1,
			[ProtoEnum(Value = 2)]
			GooglePlay = 2,
			[ProtoEnum(Value = 3)]
			GameCenter = 3,
			[ProtoEnum(Value = 4)]
			Facebook = 4
		}

		[ProtoContract]
		public class Player
		{
			[ProtoMember(1)]
			public string Id { get; set; }

			[ProtoMember(2)]
			public string Uid { get; set; }

			[ProtoMember(3)]
			public string Name { get; set; }

			[ProtoMember(4)]
			public string AvatarId { get; set; }

			[ProtoMember(5)]
			public int TimeInGame { get; set; }
		}
	}
}
