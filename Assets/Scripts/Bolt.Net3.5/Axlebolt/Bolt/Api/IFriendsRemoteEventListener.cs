using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[EventListener("FriendsRemoteEventListener")]
	public interface IFriendsRemoteEventListener
	{
		[Event("onFriendOnlineStateChanged")]
		void OnFriendOnlineStateChanged(string friendId, FriendsMessage.OnlineStatus prevStatus, FriendsMessage.OnlineStatus newStatus);

		[Event("onFriendNameChanged")]
		void OnFriendNameChanged(string friendId, string newName);

		[Event("onNewFriendshipRequest")]
		void OnNewFriendshipRequest(string friendId);

		[Event("onFriendAdded")]
		void OnFriendAdded(string friendId);

		[Event("onFriendRemoved")]
		void OnFriendRemoved(string friendId);

		[Event("onFriendAvatarChanged")]
		void OnFriendAvatarChanged(string friendId, string avatarId);

		[Event("onMsgFromFriend")]
		void OnMsgFromFriend(string friendId, string textMessage);
	}
}
