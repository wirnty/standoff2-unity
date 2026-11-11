using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("ClanRemoteService")]
	public interface IClanRemoteService
	{
		[Rpc("assignRoleToMember")]
		void AssignRoleToMember(string memberId, ClanMessage.ClanMemberRole newRole);

		[Rpc("getClan")]
		ClanMessage.Clan GetClan(string clanId);

		[Rpc("requestToJoinClan")]
		void RequestToJoinClan(string clanId);

		[Rpc("inviteToClan")]
		void InviteToClan(string playerId);

		[Rpc("createClan")]
		void CreateClan(string clanName);

		[Rpc("findClan")]
		ClanMessage.Clan[] FindClan(string clanName);

		[Rpc("sendMsgToClan")]
		void SendMsgToClan(string message);
	}
}
