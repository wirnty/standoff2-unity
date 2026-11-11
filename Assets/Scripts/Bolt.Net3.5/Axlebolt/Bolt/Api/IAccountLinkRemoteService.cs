using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("AccountLinkRemoteService")]
	public interface IAccountLinkRemoteService
	{
		[Rpc("getPlayerByTicket")]
		PlayerMessage.Player GetPlayerByTicket(string ticket);

		[Rpc("createLinkTicket")]
		string CreateLinkTicket();

		[Rpc("linkAccount")]
		void LinkAccount(string ticket);

		[Rpc("unlinkAccount")]
		void UnlinkAccount(string playerId, PlayerMessage.AuthType authType);
	}
}
