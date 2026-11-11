using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Effects;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player;
using Photon;
using UnityEngine;

namespace Axlebolt.Standoff.Test
{
	public class LevelTesting : PunBehaviour
	{
		[SerializeField]
		private Camera _mainCamera;

		[SerializeField]
		private Camera _fpsCamera;

		[PlayerArms]
		[SerializeField]
		[Space]
		[Header("Counter Terrorist")]
		private string _ctArms;

		[SerializeField]
		private PlayerCharacters _ctCharacters;

		[PlayerArms]
		[SerializeField]
		[Space]
		[Header("Terrorist")]
		private string _trArms;

		[SerializeField]
		private PlayerCharacters _trCharacters;

		[SerializeField]
		[Space]
		private WeaponId _weapon;

		[SerializeField]
		private Team _currentTeam;

		[SerializeField]
		private ViewMode _viewMode = ViewMode.FPS;

		private SpawnPointManager _spawnPointManager;

		private void Awake()
		{
			PhotonNetworkExtension.MessageListeners.Add(this);
			Singleton<WeaponManager>.Instance.Init();
			Singleton<PlayerManager>.Instance.Init(_ctArms, _ctCharacters, _trArms, _trCharacters);
			EffectsModule.Init();
			PhotonNetwork.PrefabPool = new PhotonPrefabPool();
			_spawnPointManager = new SpawnPointManager();
		}

		private void Start()
		{
			PhotonNetwork.sendRate = 15;
			PhotonNetwork.sendRateOnSerialize = 15;
			PhotonNetwork.ConnectUsingSettings("0.0.1");
		}

		public override void OnConnectedToMaster()
		{
			UnityEngine.MonoBehaviour.print("OnConnectedToMaster");
			PhotonNetwork.JoinLobby(TypedLobby.Default);
		}

		public override void OnJoinedLobby()
		{
			UnityEngine.MonoBehaviour.print("OnJoinedLobby");
			PhotonNetwork.JoinOrCreateRoom("testRoom3", new RoomOptions
			{
				MaxPlayers = 12,
				IsOpen = true
			}, TypedLobby.Default);
		}

		public override void OnJoinedRoom()
		{
			UnityEngine.MonoBehaviour.print("OnJoinedRoom");
			SpawnPoint spawnPoint = _spawnPointManager.GetSpawnPoint(_currentTeam);
			Singleton<PlayerManager>.Instance.LocalInstantiateEvent += OnSpawned;
			Singleton<PlayerManager>.Instance.Instantiate(_currentTeam, spawnPoint.Position, spawnPoint.Rotation);
		}

		private void OnSpawned(PlayerController playerController)
		{
			Controls.Instance.PlayerController = playerController;
			playerController.SetMainCamera(_mainCamera);
			playerController.SetFpsCamera(_fpsCamera);
			playerController.InventoryController.SetWeapon(Singleton<WeaponManager>.Instance.Get((byte)_weapon));
			if (_viewMode == ViewMode.FPS)
			{
				playerController.SetFPSView();
			}
		}

		public void EstablishConnection()
		{
			if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated || PhotonNetwork.connectionStateDetailed == ClientState.Disconnected)
			{
				PhotonNetwork.ConnectUsingSettings("0.0.1");
			}
		}

		public void OnDestroy()
		{
			PhotonNetworkExtension.MessageListeners.Remove(this);
		}
	}
}
