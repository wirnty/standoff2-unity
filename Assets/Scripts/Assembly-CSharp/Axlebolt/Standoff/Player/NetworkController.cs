using Axlebolt.Standoff.Player.Networking;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player
{
	[RequireComponent(typeof(PlayerController))]
	[RequireComponent(typeof(CharacterPlayer))]
	public class NetworkController : MonoBehaviour, IPunObservable
	{
		private CharacterPlayer _characterPlayer;

		private PlayerController _playerController;

		private PhotonView _photonView;

		public bool SynchronizationEnabled
		{
			get
			{
				return _photonView.synchronization == ViewSynchronization.Unreliable;
			}
			set
			{
				_photonView.synchronization = (value ? ViewSynchronization.Unreliable : ViewSynchronization.Off);
			}
		}

		private void Awake()
		{
			_characterPlayer = this.GetRequireComponent<CharacterPlayer>();
			_playerController = this.GetRequireComponent<PlayerController>();
			_photonView = this.GetRequireComponent<PhotonView>();
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting)
			{
				PlayerSnapshot snapshot = _playerController.GetSnapshot();
				NetworkWriter networkWriter = new NetworkWriter();
				snapshot.Serialize(networkWriter);
				byte[] obj = networkWriter.AsArray();
				stream.SendNext(obj);
			}
			else
			{
				byte[] buffer = (byte[])stream.ReceiveNext();
				NetworkReader reader = new NetworkReader(buffer);
				PlayerSnapshot playerSnapshot = new PlayerSnapshot();
				playerSnapshot.Deserialize(reader);
				_characterPlayer.AddSnapshot(playerSnapshot);
			}
		}
	}
}
