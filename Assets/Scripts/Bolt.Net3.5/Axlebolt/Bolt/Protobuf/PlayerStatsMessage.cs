using System.Collections.Generic;
using ProtoBuf;

namespace Axlebolt.Bolt.Protobuf
{
	public class PlayerStatsMessage
	{
		[ProtoContract]
		public class Stats
		{
			[ProtoMember(1)]
			public List<PlayerStat> Stat { get; set; }

			[ProtoMember(2)]
			public List<Achievement> Achievement { get; set; }
		}

		[ProtoContract]
		public enum StatDefType
		{
			[ProtoEnum(Value = 0)]
			Int = 0,
			[ProtoEnum(Value = 1)]
			Float = 1,
			[ProtoEnum(Value = 2)]
			Avgrate = 2
		}

		[ProtoContract]
		public class PlayerStat
		{
			[ProtoMember(1)]
			public string Name { get; set; }

			[ProtoMember(2)]
			public int IntValue { get; set; }

			[ProtoMember(3)]
			public float FloatValue { get; set; }

			[ProtoMember(4)]
			public float Window { get; set; }

			[ProtoMember(5)]
			public StatDefType Type { get; set; }
		}

		[ProtoContract]
		public class Achievement
		{
			[ProtoMember(1)]
			public string Name { get; set; }

			[ProtoMember(2)]
			public string DisplayName { get; set; }

			[ProtoMember(3)]
			public string DisplayDescription { get; set; }

			[ProtoMember(4)]
			public long UnlockTime { get; set; }

			[ProtoMember(5)]
			public bool Achieved { get; set; }

			[ProtoMember(6)]
			public byte[] Icon { get; set; }

			[ProtoMember(7)]
			public bool Hidden { get; set; }
		}

		[ProtoContract]
		public class StorePlayerStat
		{
			[ProtoMember(1)]
			public string Name { get; set; }

			[ProtoMember(2)]
			public int StoreInt { get; set; }

			[ProtoMember(3)]
			public float StoreFloat { get; set; }
		}

		[ProtoContract]
		public class StoreAchievement
		{
			[ProtoMember(1)]
			public string Name { get; set; }

			[ProtoMember(2)]
			public bool Achieved { get; set; }
		}
	}
}
