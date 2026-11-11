using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("HandshakeRemoteService")]
	public interface IHandshakeRemoteService
	{
		[Rpc("handshake")]
		void Handshake(string ticket);

		[Rpc("logout")]
		void Logout();
	}
}
