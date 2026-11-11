using ExitGames.Client.Photon;

namespace Axlebolt.Standoff.Photon
{
	public class PhotonStateException : PhotonException
	{
		public string Operation { get; }

		public PeerStateValue ConnectionState { get; }

		public PhotonStateException(string operation, PeerStateValue connectionState)
		{
			Operation = operation;
			ConnectionState = connectionState;
		}
	}
}
