using System.Collections.Generic;
using ProtoBuf;

namespace Axlebolt.Bolt.Protobuf
{
	public class ClanMessage
	{
		[ProtoContract]
		public class Clan
		{
			[ProtoMember(1)]
			public string Name { get; set; }

			[ProtoMember(2)]
			public string Tag { get; set; }

			[ProtoMember(3)]
			public ClanType ClanType { get; set; }
		}

		[ProtoContract]
		public enum ClanType
		{
			[ProtoEnum(Value = 0)]
			Closed = 0,
			[ProtoEnum(Value = 1)]
			InviteOnly = 1,
			[ProtoEnum(Value = 2)]
			AnyoneCanJoin = 2
		}

		[ProtoContract]
		public class ClanMember
		{
			[ProtoMember(1)]
			public PlayerMessage.Player Player { get; set; }

			[ProtoMember(2)]
			public bool Online { get; set; }

			[ProtoMember(3)]
			public ClanMemberRole Role { get; set; }
		}

		[ProtoContract]
		public class ClanMemberRole
		{
			[ProtoMember(1)]
			public string Name { get; set; }

			[ProtoMember(2)]
			public int Level { get; set; }

			[ProtoMember(3)]
			public List<ClanMemberRolePermission> Permissions { get; set; }
		}

		[ProtoContract]
		public enum ClanMemberRolePermission
		{
			[ProtoEnum(Value = 0)]
			ChangeClanSettings = 0,
			[ProtoEnum(Value = 1)]
			AcceptMember = 1,
			[ProtoEnum(Value = 2)]
			InviteMember = 2,
			[ProtoEnum(Value = 3)]
			KickMemberLess = 3,
			[ProtoEnum(Value = 4)]
			KickMemberEqual = 4,
			[ProtoEnum(Value = 5)]
			AssignRoleLess = 5,
			[ProtoEnum(Value = 6)]
			AssignRoleEqual = 6,
			[ProtoEnum(Value = 7)]
			CreateClanBattle = 7,
			[ProtoEnum(Value = 8)]
			JoinClanBattle = 8
		}
	}
}
