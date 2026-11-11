namespace Axlebolt.Standoff.Photon
{
	public class PhotonConnectionFailedException : PhotonException
	{
		public string ServerAddress { get; }

		public ServerConnection ServerConnection { get; }

		public DisconnectCause DisconnectCause { get; }

		public PhotonConnectionFailedException(DisconnectCause disconnectCause)
		{
			ServerAddress = PhotonNetwork.networkingPeer.ServerAddress;
			ServerConnection = PhotonNetwork.networkingPeer.Server;
			DisconnectCause = disconnectCause;
		}
	}
}
