using ProtoBuf;

namespace Axlebolt.Bolt.Protobuf
{
	public class CommonMessage
	{
		[ProtoContract]
		public enum Comparison
		{
			[ProtoEnum(Value = 0)]
			EqualToOrLessThan = 0,
			[ProtoEnum(Value = 1)]
			LessThan = 1,
			[ProtoEnum(Value = 2)]
			Equal = 2,
			[ProtoEnum(Value = 3)]
			GreaterThan = 3,
			[ProtoEnum(Value = 4)]
			EqualToOrGreaterThan = 4,
			[ProtoEnum(Value = 5)]
			NotEqual = 5
		}

		[ProtoContract]
		public class Filter
		{
			[ProtoMember(1)]
			public string Name { get; set; }

			[ProtoMember(2)]
			public int IntValue { get; set; }

			[ProtoMember(3)]
			public float FloatValue { get; set; }

			[ProtoMember(4)]
			public string StringValue { get; set; }

			[ProtoMember(5)]
			public Comparison Comparison { get; set; }
		}

		[ProtoContract]
		public class AvatarBinary
		{
			[ProtoMember(1)]
			public string Id { get; set; }

			[ProtoMember(2)]
			public byte[] Avatar { get; set; }
		}

		[ProtoContract]
		public class KeyValue
		{
			[ProtoMember(1)]
			public string Key { get; set; }

			[ProtoMember(2)]
			public string Value { get; set; }
		}
	}
}
