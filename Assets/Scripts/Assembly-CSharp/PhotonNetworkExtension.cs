using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

public class PhotonNetworkExtension
{
	public static HashSet<IPunCallbacks> MessageListeners = new HashSet<IPunCallbacks>();

	public static void SendMonoMessageFast(PhotonNetworkingMessage methodString, params object[] parameters)
	{
		HashSet<IPunCallbacks> hashSet = new HashSet<IPunCallbacks>(MessageListeners);
		object callParameter = ((parameters == null || parameters.Length != 1) ? parameters : parameters[0]);
		switch (methodString)
		{
		case PhotonNetworkingMessage.OnConnectedToMaster:
		{
			foreach (IPunCallbacks messageListener in hashSet)
			{
				Safe(delegate
				{
					messageListener.OnConnectedToMaster();
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnConnectedToPhoton:
		{
			foreach (IPunCallbacks messageListener2 in hashSet)
			{
				Safe(delegate
				{
					messageListener2.OnConnectedToPhoton();
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnConnectionFail:
		{
			foreach (IPunCallbacks messageListener3 in hashSet)
			{
				Safe(delegate
				{
					messageListener3.OnConnectionFail((DisconnectCause)callParameter);
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnCreatedRoom:
		{
			foreach (IPunCallbacks messageListener29 in hashSet)
			{
				Safe(delegate
				{
					messageListener29.OnCreatedRoom();
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnCustomAuthenticationFailed:
		{
			foreach (IPunCallbacks messageListener28 in hashSet)
			{
				Safe(delegate
				{
					messageListener28.OnCustomAuthenticationFailed(callParameter as string);
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnCustomAuthenticationResponse:
		{
			foreach (IPunCallbacks messageListener27 in hashSet)
			{
				Safe(delegate
				{
					messageListener27.OnCustomAuthenticationResponse((Dictionary<string, object>)callParameter);
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnDisconnectedFromPhoton:
		{
			foreach (IPunCallbacks messageListener26 in hashSet)
			{
				Safe(delegate
				{
					messageListener26.OnDisconnectedFromPhoton();
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnFailedToConnectToPhoton:
		{
			foreach (IPunCallbacks messageListener25 in hashSet)
			{
				Safe(delegate
				{
					messageListener25.OnFailedToConnectToPhoton((DisconnectCause)callParameter);
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnJoinedLobby:
		{
			foreach (IPunCallbacks messageListener24 in hashSet)
			{
				Safe(delegate
				{
					messageListener24.OnJoinedLobby();
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnJoinedRoom:
		{
			foreach (IPunCallbacks messageListener23 in hashSet)
			{
				Safe(delegate
				{
					messageListener23.OnJoinedRoom();
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnLeftLobby:
		{
			foreach (IPunCallbacks messageListener22 in hashSet)
			{
				Safe(delegate
				{
					messageListener22.OnLeftLobby();
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnLeftRoom:
		{
			foreach (IPunCallbacks messageListener21 in hashSet)
			{
				Safe(delegate
				{
					messageListener21.OnLeftRoom();
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnLobbyStatisticsUpdate:
		{
			foreach (IPunCallbacks messageListener20 in hashSet)
			{
				Safe(delegate
				{
					messageListener20.OnLobbyStatisticsUpdate();
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnMasterClientSwitched:
		{
			foreach (IPunCallbacks messageListener19 in hashSet)
			{
				Safe(delegate
				{
					messageListener19.OnMasterClientSwitched((PhotonPlayer)callParameter);
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnOwnershipRequest:
		{
			foreach (IPunCallbacks messageListener18 in hashSet)
			{
				Safe(delegate
				{
					messageListener18.OnOwnershipRequest((object[])callParameter);
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnPhotonCreateRoomFailed:
		{
			foreach (IPunCallbacks messageListener17 in hashSet)
			{
				Safe(delegate
				{
					messageListener17.OnPhotonCreateRoomFailed((object[])callParameter);
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged:
		{
			foreach (IPunCallbacks messageListener16 in hashSet)
			{
				Safe(delegate
				{
					messageListener16.OnPhotonCustomRoomPropertiesChanged((Hashtable)callParameter);
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnPhotonInstantiate:
		{
			foreach (IPunCallbacks messageListener15 in hashSet)
			{
				Safe(delegate
				{
					messageListener15.OnPhotonInstantiate((PhotonMessageInfo)callParameter);
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnPhotonJoinRoomFailed:
		{
			foreach (IPunCallbacks messageListener14 in hashSet)
			{
				Safe(delegate
				{
					messageListener14.OnPhotonJoinRoomFailed((object[])callParameter);
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnPhotonMaxCccuReached:
		{
			foreach (IPunCallbacks messageListener13 in hashSet)
			{
				Safe(delegate
				{
					messageListener13.OnPhotonMaxCccuReached();
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnPhotonPlayerConnected:
		{
			foreach (IPunCallbacks messageListener12 in hashSet)
			{
				Safe(delegate
				{
					messageListener12.OnPhotonPlayerConnected((PhotonPlayer)callParameter);
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnPhotonPlayerDisconnected:
		{
			foreach (IPunCallbacks messageListener11 in hashSet)
			{
				Safe(delegate
				{
					messageListener11.OnPhotonPlayerDisconnected((PhotonPlayer)callParameter);
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged:
		{
			foreach (IPunCallbacks messageListener10 in hashSet)
			{
				Safe(delegate
				{
					messageListener10.OnPhotonPlayerPropertiesChanged((object[])callParameter);
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnPhotonRandomJoinFailed:
		{
			foreach (IPunCallbacks messageListener9 in hashSet)
			{
				Safe(delegate
				{
					messageListener9.OnPhotonRandomJoinFailed((object[])callParameter);
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnPhotonSerializeView:
			Debug.LogError("PhotonNetworkingMessage.OnPhotonSerializeView unsupported");
			break;
		case PhotonNetworkingMessage.OnReceivedRoomListUpdate:
		{
			foreach (IPunCallbacks messageListener8 in hashSet)
			{
				Safe(delegate
				{
					messageListener8.OnReceivedRoomListUpdate();
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnUpdatedFriendList:
		{
			foreach (IPunCallbacks messageListener7 in hashSet)
			{
				Safe(delegate
				{
					messageListener7.OnUpdatedFriendList();
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnWebRpcResponse:
		{
			foreach (IPunCallbacks messageListener6 in hashSet)
			{
				Safe(delegate
				{
					messageListener6.OnWebRpcResponse((OperationResponse)callParameter);
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnPhotonPlayerActivityChanged:
		{
			foreach (IPunCallbacks messageListener5 in hashSet)
			{
				Safe(delegate
				{
					messageListener5.OnPhotonPlayerActivityChanged((PhotonPlayer)callParameter);
				});
			}
			break;
		}
		case PhotonNetworkingMessage.OnOwnershipTransfered:
		{
			foreach (IPunCallbacks messageListener4 in hashSet)
			{
				Safe(delegate
				{
					messageListener4.OnOwnershipTransfered((object[])callParameter);
				});
			}
			break;
		}
		default:
			Debug.LogError(string.Format("Unsupported {0}", methodString));
			break;
		}
	}

	private static void Safe(Action action)
	{
		try
		{
			action();
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}
}
