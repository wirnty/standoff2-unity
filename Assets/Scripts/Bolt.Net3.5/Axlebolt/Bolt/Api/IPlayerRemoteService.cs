using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("PlayerRemoteService")]
	public interface IPlayerRemoteService
	{
		[Rpc("getPlayer")]
		PlayerMessage.Player GetCurrentPlayer();

		[Rpc("setPlayerName")]
		void SetPlayerName(string newName);

		[Rpc("setPlayerAvatar")]
		string SetPlayerAvatar(byte[] avatar);
	}
}
