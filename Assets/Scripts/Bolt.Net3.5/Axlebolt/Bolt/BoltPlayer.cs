using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Axlebolt.Bolt.Api;
using Axlebolt.Bolt.Protobuf;

namespace Axlebolt.Bolt
{
	public class BoltPlayer : BoltService<BoltPlayer>
	{

		private const string AvatarCache = "Avatar";

		private readonly IPlayerRemoteService _playerRemoteService;

		private readonly IFriendsRemoteService _friendsRemoteService;

		private readonly FileCache _fileCache;

		public string Uid
		{
			[CompilerGenerated]
			get
			{
				return Player.Uid;
			}
		}

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return Player.Name;
			}
		}

		public byte[] Avatar
		{
			[CompilerGenerated]
			get
			{
				return Player.Avatar;
			}
		}

		public Player Player { get; private set; }

		internal BoltPlayer()
		{
			_fileCache = BoltApi.FileCache;
		}

		public Task SetName(string name)
		{
			return BoltApi.Instance.Async(delegate
			{
				Player.Name = name;
			});
		}

		public Task SetAvatar(byte[] avatar)
		{
			return BoltApi.Instance.Async(delegate
			{
				Player.Avatar = avatar;
				_fileCache.Set(Guid.NewGuid().ToString(), avatar);
			});
		}

		internal override void Load()
		{
			PlayerMessage.Player currentPlayer = new PlayerMessage.Player { AvatarId = "", Id = "asdasda123", Name = "Guest", TimeInGame = 100000, Uid = "Ryzen" };
			byte[] avatar = GetAvatar(currentPlayer);
			Player = new Player(currentPlayer.Uid, currentPlayer.Name, avatar, currentPlayer.TimeInGame);
		}

		private byte[] GetAvatar(PlayerMessage.Player playerMessage)
		{
			byte[] array;
			if (string.IsNullOrEmpty(playerMessage.AvatarId))
			{
				array = null;
			}
			else
			{
				array = _fileCache.Get(playerMessage.AvatarId);
				if (array != null)
				{
					return array;
				}
				array = _friendsRemoteService.GetAvatar(playerMessage.AvatarId).Avatar;
				_fileCache.Set("Avatar", array);
			}
			return array;
		}
	}
}
