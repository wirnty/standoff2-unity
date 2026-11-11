using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("MatchmakingRemoteService")]
	public interface IMatchmakingRemoteService
	{
		[Rpc("getLobby")]
		MatchmakingMessage.Lobby GetLobby(string lobbyId);

		[Rpc("requestInternetServerList")]
		MatchmakingMessage.GameServer[] RequestInternetServerList(string map, byte? freePlayerSlots, byte? maxPlayers, bool? withPassword);

		[Rpc("getGameServerDetails")]
		MatchmakingMessage.GameServerDetails GetGameServerDetails(string gameServerId);

		[Rpc("getGameServerPlayers")]
		PlayerMessage.Player[] GetGameServerPlayers(string gameServerId);

		[Rpc("requestLobbyList")]
		MatchmakingMessage.Lobby[] RequestLobbyList(MatchmakingMessage.LobbyDistanceFilter distanceFilter, CommonMessage.Filter[] filters);

		[Rpc("createLobby")]
		MatchmakingMessage.Lobby CreateLobby(string name, MatchmakingMessage.LobbyType lobbyType, int maxMembers);

		[Rpc("joinLobby")]
		MatchmakingMessage.Lobby JoinLobby(string lobbyId);

		[Rpc("leaveLobby")]
		void LeaveLobby(string lobbyId);

		[Rpc("invitePlayerToLobby")]
		void InvitePlayerToLobby(string lobbyId, string playerId);

		[Rpc("revokePlayerInvitationToLobby")]
		void RevokePlayerInvitationToLobby(string lobbyId, string playerId);

		[Rpc("getLobbyMembers")]
		PlayerMessage.Player[] GetLobbyMembers(string lobbyId);

		[Rpc("setLobbyOwner")]
		void SetLobbyOwner(string lobbyId, string playerId);

		[Rpc("getLobbyOwner")]
		PlayerMessage.Player GetLobbyOwner(string lobbyId);

		[Rpc("setLobbyName")]
		void SetLobbyName(string lobbyId, string name);

		[Rpc("setLobbyType")]
		void SetLobbyType(string lobbyId, MatchmakingMessage.LobbyType lobbyType);

		[Rpc("setLobbyMaxMembers")]
		void SetLobbyMaxMembers(string lobbyId, int maxMembers);

		[Rpc("setLobbyJoinable")]
		void SetLobbyJoinable(string lobbyId, bool? joinable);

		[Rpc("setLobbyData")]
		void SetLobbyData(string lobbyId, string key, string value);

		[Rpc("deleteLobbyData")]
		void DeleteLobbyData(string lobbyId, string key);

		[Rpc("sendLobbyChatMsg")]
		void SendLobbyChatMsg(string lobbyId, byte[] message);

		[Rpc("setLobbyGameServer")]
		void SetLobbyGameServer(string lobbyId, string gameServerId);

		[Rpc("getLobbyGameServer")]
		MatchmakingMessage.GameServerDetails GetLobbyGameServer(string lobbyId);
	}
}
