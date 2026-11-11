using ProtoBuf;

namespace Axlebolt.Bolt.Protobuf
{
	public class SettingsMessage
	{
		[ProtoContract]
		public class PlayerSetting
		{
			[ProtoMember(1)]
			public string Key { get; set; }

			[ProtoMember(2)]
			public int IntValue { get; set; }

			[ProtoMember(3)]
			public float FloatValue { get; set; }

			[ProtoMember(4)]
			public bool BoolValue { get; set; }

			[ProtoMember(5)]
			public string StringValue { get; set; }
		}
	}
}
