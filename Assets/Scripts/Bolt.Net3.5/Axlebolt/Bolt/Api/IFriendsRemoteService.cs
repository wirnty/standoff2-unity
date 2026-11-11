using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("FriendsRemoteService")]
	public interface IFriendsRemoteService
	{
		[Rpc("getPlayerFriends")]
		FriendsMessage.PlayerFriend[] GetPlayerFriends();

		[Rpc("getOnlineStatus")]
		FriendsMessage.OnlineStatus GetOnlineStatus(string playerId);

		[Rpc("getAvatar")]
		CommonMessage.AvatarBinary GetAvatar(string id);

		[Rpc("sendFriendRequest")]
		void SendFriendRequest(string friendId);

		[Rpc("revokeFriendRequest")]
		void RevokeFriendRequest(string friendId);

		[Rpc("acceptFriendRequest")]
		void AcceptFriendRequest(string friendId);

		[Rpc("ignoreFriendRequest")]
		void IgnoreFriendRequest(string friendId);

		[Rpc("blockFriend")]
		void BlockFriend(string friendId);

		[Rpc("unblockFriend")]
		void UnblockFriend(string friendId);

		[Rpc("removeFriend")]
		void RemoveFriend(string friendId);

		[Rpc("sendMsg")]
		void SendMsg(string msg, string playerId);

		[Rpc("getAllMsg")]
		FriendsMessage.FriendMessage[] GetAllMsg(string[] playerIds);

		[Rpc("getNewMsg")]
		FriendsMessage.FriendMessage[] GetNewMsg(string[] playerIds);

		[Rpc("getNewMsgPlayersCount")]
		int? GetNewMsgPlayersCount();

		[Rpc("readNewMsgs")]
		void ReadNewMsgs(string[] msgIds);

		[Rpc("getPlayerById")]
		PlayerMessage.Player GetPlayer(string playerId);
	}
}
