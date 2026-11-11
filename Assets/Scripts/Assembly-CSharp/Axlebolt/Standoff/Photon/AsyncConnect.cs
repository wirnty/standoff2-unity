using System.Threading.Tasks;
using Photon;

namespace Axlebolt.Standoff.Photon
{
	public class AsyncConnect : AsyncOperation<AsyncConnect>
	{
		public async Task ConnectUsingSettings(string gameVersion)
		{
			if (PhotonNetwork.ConnectUsingSettings(gameVersion))
			{
				await WaitForResult();
				Done();
			}
			else
			{
				Result = new AsyncResult(new PhotonStateException("ConnectUsingSettings", PhotonNetwork.networkingPeer.PeerState));
				Done();
			}
		}

		public async Task Connect(string masterServerAddress, int port, string appId, string gameVersion)
		{
			AsyncOperation<AsyncConnect>.Log.Debug($"Connect to {masterServerAddress}");
			UnityEngine.Debug.Log(appId);
			if (PhotonNetwork.ConnectUsingSettings(gameVersion))/*("92.38.154.54", 5055, "0893fa01-0b4c-40b9-ac88-bcf5ef0614fb", "0.10.21a")*//*(masterServerAddress, port, appId, gameVersion)*/
			{
				await WaitForResult();
				Done();
			}
			else
			{
				Result = new AsyncResult(new PhotonStateException("Connect", PhotonNetwork.networkingPeer.PeerState));
				Done();
			}
		}

		public override void OnFailedToConnectToPhoton(DisconnectCause cause)
		{
			if (AsyncOperation<AsyncConnect>.Log.DebugEnabled)
			{
				AsyncOperation<AsyncConnect>.Log.Debug($"OnFailedToConnectToPhoton({cause})");
			}
			Result = new AsyncResult(new PhotonConnectionFailedException(cause));
		}

		public override void OnConnectedToMaster()
		{
			if (AsyncOperation<AsyncConnect>.Log.DebugEnabled)
			{
				AsyncOperation<AsyncConnect>.Log.Debug("OnConnectedToMaster");
			}
			Result = new AsyncResult();
		}

		public override void OnJoinedLobby()
		{
			if (AsyncOperation<AsyncConnect>.Log.DebugEnabled)
			{
				AsyncOperation<AsyncConnect>.Log.Debug("OnJoinedLobby");
			}
			Result = new AsyncResult();
		}
	}
}
