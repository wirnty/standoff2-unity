using System;
using System.Collections.Generic;
using System.Linq;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player.Networking;
using Axlebolt.Standoff.Player.Ragdoll;
using Axlebolt.Standoff.Settings;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class PlayerManager : Singleton<PlayerManager>
	{
		public delegate void EventHandler(PlayerController playerController);

		private static readonly Log Log = Log.Create(typeof(PlayerManager));

		private const int PoolSize = 5;

		private PlayerHitboxConfig _playerHitboxConfig;

		private PlayerPool _ctPool;

		private PlayerPool _trPool;

		private PlayerController _currentPlayer;

		private PlayerSnapshot _lastSnapshot;

		private readonly Dictionary<int, PlayerController> _playerById = new Dictionary<int, PlayerController>();

		internal PlayerController PlayerController { get; private set; }

		public event EventHandler RemoteInstantiateEvent;

		public event EventHandler LocalInstantiateEvent;

		public event EventHandler LocalDestroyEvent;

		public void Init([NotNull] string ctArms, [NotNull] PlayerCharacters ctCharacters, [NotNull] string trArms, [NotNull] PlayerCharacters trCharacters)
		{
			if (ctArms == null)
			{
				throw new ArgumentNullException("ctArms");
			}
			if (ctCharacters == null)
			{
				throw new ArgumentNullException("ctCharacters");
			}
			if (trArms == null)
			{
				throw new ArgumentNullException("trArms");
			}
			if (trCharacters == null)
			{
				throw new ArgumentNullException("trCharacters");
			}
			PlayerController = PlayerUtility.LoadPlayerPrefab();
			_playerHitboxConfig = PlayerUtility.LoadPlayerHitboxConfig();
			_ctPool = new PlayerPool(PlayerController, _playerHitboxConfig, ctArms, ctCharacters, 5);
			_trPool = new PlayerPool(PlayerController, _playerHitboxConfig, trArms, trCharacters, 5);
			QualityManager.Instance.ModelDetailChanged += OnModelDetailChanged;
			QualityManager.Instance.ShaderDetailChanged += OnShaderDetailChanged;
			Singleton<RagdollManager>.Instance.Initialize(ctCharacters, trCharacters);
		}

		private void OnModelDetailChanged()
		{
			_ctPool.RefreshLods();
			_trPool.RefreshLods();
		}

		private void OnShaderDetailChanged()
		{
			_ctPool.UpdateMaterials();
			_trPool.UpdateMaterials();
		}

		public void Instantiate(Team team, Vector3 position, Quaternion rotation, int health = 100, int armor = 0, bool hasHelmet = false)
		{
			PhotonNetwork.player.SetHealth(0);
			PhotonNetwork.player.SetArmor(0);
			PhotonNetwork.player.SetHelmet(false);
			string freeId = GetFreeId(team);
			GameObject gameObject = PhotonNetwork.Instantiate(freeId, position, rotation, 0);
			PlayerController component = gameObject.GetComponent<PlayerController>();
			component.Instantiate(health, armor, hasHelmet);
		}

		internal void OnRemoteInstantiateEvent(PlayerController playerController)
		{
			_playerById[playerController.PhotonView.ownerId] = playerController;
			if (this.RemoteInstantiateEvent != null)
			{
				this.RemoteInstantiateEvent(playerController);
			}
		}

		internal void OnLocalInstantiateEvent(PlayerController playerController, bool reconnected)
		{
			if (!playerController.PhotonView.isMine)
			{
				Log.Error("Incorrent local instantiate event, actual remote player");
				return;
			}
			_currentPlayer = playerController;
			if (reconnected && _lastSnapshot != null)
			{
				Log.Debug("Set last snapshot to reconnected player");
				_currentPlayer.SetSnapsot(FixSnapshot(_lastSnapshot));
				_currentPlayer.InventoryController.ReswitchCurrentWeapon();
			}
			_lastSnapshot = null;
			if (this.LocalInstantiateEvent != null)
			{
				this.LocalInstantiateEvent(playerController);
			}
		}

		private PlayerSnapshot FixSnapshot(PlayerSnapshot snapshot)
		{
			return (PlayerSnapshot)_currentPlayer.Interpolator.Interpolate(snapshot, snapshot, 0f);
		}

		internal void OnLocalDestroyEvent(PlayerController playercontroller)
		{
			_currentPlayer = null;
			_lastSnapshot = playercontroller.GetSnapshot();
			if (this.LocalDestroyEvent != null)
			{
				this.LocalDestroyEvent(playercontroller);
			}
		}

		internal void OnRemoteDestroyEvent(PlayerController playerController, int id)
		{
			_playerById.Remove(id);
		}

		public PlayerController GetController(int playerId)
		{
			PlayerController value;
			return (!_playerById.TryGetValue(playerId, out value)) ? null : value;
		}

		internal void CacheSnapshot([NotNull] PlayerSnapshot playerSnapshot)
		{
			if (playerSnapshot == null)
			{
				throw new ArgumentNullException("playerSnapshot");
			}
			if (_currentPlayer == null)
			{
				Log.Error("Current player is null, is's impossible! ");
			}
			else
			{
				_lastSnapshot = playerSnapshot;
			}
		}

		public GameObject GetFromPool(string id, Vector3 position, Quaternion rotation)
		{
			Team team;
			string character;
			PlayerUtility.ParseId(id, out team, out character);
			PlayerController fromPool = GetPool(team).GetFromPool(character);
			fromPool.name = id;
			fromPool.gameObject.SetActive(true);
			fromPool.transform.position = position;
			fromPool.transform.rotation = rotation;
			fromPool.Initialize(Singleton<WeaponManager>.Instance.GetPrimaryWeapon());
			return fromPool.gameObject;
		}

		public void ReturnToPool(GameObject go)
		{
			PlayerController component = go.GetComponent<PlayerController>();
			if (component == null)
			{
				Log.Error("Can't return to pool {0}, Player not found", go);
				return;
			}
			string id = go.name;
			Team team;
			string character;
			PlayerUtility.ParseId(id, out team, out character);
			go.SetActive(false);
			component.OnPhotonDestroy();
			GetPool(team).ReturnToPool(component);
		}

		private PlayerPool GetPool(Team team)
		{
			PlayerPool result;
			switch (team)
			{
			case Team.None:
				throw new InvalidOperationException("Can't get Player for None team");
			case Team.Ct:
				result = _ctPool;
				break;
			default:
				result = _trPool;
				break;
			}
			return result;
		}

		public List<string> GetIds()
		{
			List<string> list = _trPool.Characters.Select((string skin) => PlayerUtility.CreateId(Team.Tr, skin)).ToList();
			list.AddRange(_ctPool.Characters.Select((string skin) => PlayerUtility.CreateId(Team.Ct, skin)).ToList());
			return list;
		}

		public string GetFreeId(Team team)
		{
			string freeCharacter = GetPool(team).GetFreeCharacter();
			return PlayerUtility.CreateId(team, freeCharacter);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			QualityManager.Instance.ModelDetailChanged -= OnModelDetailChanged;
			QualityManager.Instance.ShaderDetailChanged -= OnShaderDetailChanged;
		}
	}
}
