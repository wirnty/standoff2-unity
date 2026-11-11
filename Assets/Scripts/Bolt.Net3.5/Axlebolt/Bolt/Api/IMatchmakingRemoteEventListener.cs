using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[EventListener("MatchmakingRemoteEventListener")]
	public interface IMatchmakingRemoteEventListener
	{
		[Event("onNewPlayerJoinedLobby")]
		void OnNewPlayerJoinedLobby(PlayerMessage.Player newPlayer);

		[Event("onPlayerLeftLobby")]
		void OnPlayerLeftLobby(string playerId);

		[Event("onPlayerInvitedToLobby")]
		void OnPlayerInvitedToLobby(PlayerMessage.Player newPlayer);

		[Event("onReceivedInviteToLobby")]
		void OnReceivedInviteToLobby(PlayerMessage.Player lobbyOwner);

		[Event("onRevokeInviteToLobby")]
		void OnRevokeInviteToLobby(string playerId);

		[Event("onReceivedRevokeInviteToLobby")]
		void OnReceivedRevokeInviteToLobby(string lobbyOwnerId);

		[Event("onLobbyOwnerChanged")]
		void OnLobbyOwnerChanged(string lobbyOwnerId);

		[Event("onLobbyNameChanged")]
		void OnLobbyNameChanged(string name);

		[Event("onLobbyTypeChanged")]
		void OnLobbyTypeChanged(MatchmakingMessage.LobbyType lobbyType);

		[Event("onLobbyMaxMembersChanged")]
		void OnLobbyMaxMembersChanged(byte maxMembers);

		[Event("onLobbyJoinableChanged")]
		void OnLobbyJoinableChanged(bool? joinable);

		[Event("onLobbyDataChanged")]
		void OnLobbyDataChanged(CommonMessage.KeyValue[] data);

		[Event("onLobbyGameServerChanged")]
		void OnLobbyGameServerChanged(MatchmakingMessage.GameServer gameServer);

		[Event("onLobbyChatMessage")]
		void OnLobbyChatMessage(string playerId, byte[] message);
	}
}
