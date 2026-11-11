using System;
using System.Runtime.CompilerServices;
using Axlebolt.Networking;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player.Aim;
using Axlebolt.Standoff.Player.Inventory;
using Axlebolt.Standoff.Player.Mecanim;
using Axlebolt.Standoff.Player.Movement;
using Axlebolt.Standoff.Player.Networking;
using Axlebolt.Standoff.Player.Ragdoll;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Axlebolt.Standoff.Player
{
	[RequireComponent(typeof(MecanimController))]
	[RequireComponent(typeof(MovementController))]
	[RequireComponent(typeof(ArmsAnimationController))]
	[RequireComponent(typeof(AimController))]
	[RequireComponent(typeof(PhotonView))]
	[RequireComponent(typeof(InventoryController))]
	public class PlayerController : MonoBehaviour
	{
		public delegate void CharacterSkinChangedHander(BipedMap bipedMap);

		private static readonly Log Log = Log.Create(typeof(PlayerController));

		[SerializeField]
		[FormerlySerializedAs("_mainCamera")]
		private GameObject _mainCameraHolder;

		[SerializeField]
		[FormerlySerializedAs("_fpsCamera")]
		private GameObject _fpsCameraHolder;

		[SerializeField]
		private BipedMap _characterBiped;

		[SerializeField]
		private BipedMap _armsBiped;

		private PhotonView _photonView;

		private int _photonPlayerId;

		private bool _isSynchronized;

		private bool _isCharacterVisible;

		private CharacterPlayer _characterPlayer;

		private Camera _mainCamera;

		private Camera _fpsCamera;

		public float LocalTime { get; private set; }

		public bool IsInitialized { get; private set; }

		public AimController AimController { get; private set; }

		public InventoryController InventoryController { get; private set; }

		public MecanimController MecanimController { get; private set; }

		public MovementController MovementController { get; private set; }

		public NetworkController NetworkController { get; private set; }

		public PlayerRagdollController CharacterRagdollController { get; private set; }

		public ArmsAnimationController ArmsAnimController { get; private set; }

		public PlayerHitController HitController { get; private set; }

		public Interpolator Interpolator
		{
			[CompilerGenerated]
			get
			{
				return _characterPlayer.GetInterpolator();
			}
		}

		public BipedMap BipedMap { get; private set; }

		public GameObject CharacterGO
		{
			[CompilerGenerated]
			get
			{
				return BipedMap.gameObject;
			}
		}

		public GameObject FpsCameraHolder
		{
			[CompilerGenerated]
			get
			{
				return _fpsCameraHolder;
			}
		}

		public GameObject MainCameraHolder
		{
			[CompilerGenerated]
			get
			{
				return _mainCameraHolder;
			}
		}

		public BipedMap CharacterBiped
		{
			[CompilerGenerated]
			get
			{
				return _characterBiped;
			}
		}

		public bool IsSynchronized
		{
			[CompilerGenerated]
			get
			{
				return _isSynchronized;
			}
		}

		public bool IsCharacterVisible
		{
			[CompilerGenerated]
			get
			{
				return _isCharacterVisible;
			}
		}

		public ViewMode ViewMode { get; private set; }

		public PhotonView PhotonView
		{
			[CompilerGenerated]
			get
			{
				return _photonView;
			}
		}

		public int PlayerId
		{
			[CompilerGenerated]
			get
			{
				return PhotonView.ownerId;
			}
		}

		public PhotonPlayer Player
		{
			[CompilerGenerated]
			get
			{
				return PhotonView.owner;
			}
		}

		public event CharacterSkinChangedHander CharacterSkinSetEvent = delegate
		{
		};

		public void PreInitialize(BipedMap characterBiped, BipedMap armsBiped)
		{
			PreInitialize(characterBiped, armsBiped, null);
		}

		public void PreInitialize([NotNull] BipedMap characterBiped, [NotNull] BipedMap armsBiped, PlayerHitboxConfig hitboxConfig)
		{
			if (characterBiped == null)
			{
				throw new ArgumentNullException("characterBiped");
			}
			if (armsBiped == null)
			{
				throw new ArgumentNullException("armsBiped");
			}
			IsInitialized = true;
			_characterBiped = characterBiped;
			_armsBiped = armsBiped;
			SetInitView(true);
			_photonView = this.GetRequireComponent<PhotonView>();
			MovementController = this.GetRequireComponent<MovementController>();
			MovementController.PreInitialize();
			AimController = this.GetRequireComponent<AimController>();
			AimController.PreInitialize();
			MecanimController = this.GetRequireComponent<MecanimController>();
			MecanimController.PreInitialize();
			ArmsAnimController = this.GetRequireComponent<ArmsAnimationController>();
			ArmsAnimController.PreInitialize();
			InventoryController = this.GetRequireComponent<InventoryController>();
			InventoryController.PreInitialize();
			CharacterRagdollController = base.gameObject.AddComponent<PlayerRagdollController>();
			CharacterRagdollController.PreInitialize();
			NetworkController = GetComponent<NetworkController>();
			_characterPlayer = base.gameObject.GetRequireComponent<CharacterPlayer>();
			_characterPlayer.PreInitialize();
			if (hitboxConfig != null)
			{
				HitController = base.gameObject.AddComponent<PlayerHitController>();
				HitController.PreInitialize(hitboxConfig);
			}
			base.enabled = true;
		}

		public void Initialize(WeaponController primaryWeapon)
		{
			MovementController.Initialize();
			AimController.Initialize();
			MecanimController.Initialize();
			ArmsAnimController.Initialize();
			InventoryController.Initialize();
			SetInitView(false);
			InventoryController.SetWeapon(primaryWeapon);
			_characterPlayer.Initialize();
			SetCharacterVisible(false);
		}

		private void Clear()
		{
			if (_mainCamera != null && _mainCameraHolder.GetComponent<CameraScopeZoomer>() != null)
			{
				CameraScopeZoomer component = _mainCamera.GetComponent<CameraScopeZoomer>();
				component.PlayerController = null;
				_mainCamera.GetComponent<CameraScopeZoomer>().SetNotZoomed();
			}
			if (_mainCamera != null)
			{
				_mainCamera.transform.SetParent(null);
				_mainCamera = null;
			}
			if (_fpsCamera != null)
			{
				_fpsCamera.transform.SetParent(null);
				_fpsCamera = null;
			}
			_characterPlayer.Clear();
			InventoryController.Clear();
		}

		private void SetInitView(bool preInit)
		{
			if (preInit)
			{
				_characterBiped.transform.SetParent(base.transform);
				_armsBiped.transform.SetParent(base.transform);
				BipedMap = _characterBiped;
			}
			if (Singleton<Trash>.Instance.Contains(_characterBiped.transform, base.transform))
			{
				Singleton<Trash>.Instance.Return(_characterBiped.transform, base.transform);
			}
			_characterBiped.gameObject.SetActive(true);
			if (!Singleton<Trash>.Instance.Contains(_armsBiped.transform, base.transform))
			{
				Singleton<Trash>.Instance.Drop(_armsBiped.transform);
			}
			if (!preInit)
			{
				ViewMode = ViewMode.NotStated;
				BipedMap = null;
				SetTPSView();
			}
		}

		public void SetFPSView()
		{
			if (ViewMode != ViewMode.FPS)
			{
				ViewMode = ViewMode.FPS;
				SetCharacterSkin(_armsBiped);
				AimController.SetFPSView();
			}
		}

		public void SetTPSView()
		{
			if (ViewMode != ViewMode.TPS)
			{
				ViewMode = ViewMode.TPS;
				SetCharacterSkin(_characterBiped);
				AimController.SetTPSView();
			}
		}

		public void SetCharacterSkin(BipedMap newBipedMap)
		{
			if (Singleton<Trash>.Instance.Contains(newBipedMap.transform, base.transform))
			{
				Singleton<Trash>.Instance.Return(newBipedMap.transform, base.transform);
			}
			newBipedMap.transform.SetParent(base.transform);
			newBipedMap.transform.localPosition = Vector3.zero;
			newBipedMap.transform.localRotation = Quaternion.identity;
			if (!newBipedMap.gameObject.activeSelf)
			{
				newBipedMap.gameObject.SetActive(true);
			}
			this.CharacterSkinSetEvent(newBipedMap);
			if (BipedMap != null && !Singleton<Trash>.Instance.Contains(BipedMap.transform, base.transform))
			{
				Singleton<Trash>.Instance.Drop(BipedMap.transform);
			}
			BipedMap = newBipedMap;
			if (ViewMode == ViewMode.FPS)
			{
				_armsBiped = newBipedMap;
			}
			if (ViewMode == ViewMode.TPS)
			{
				_characterBiped = newBipedMap;
			}
			SetCharacterVisible(_isCharacterVisible);
		}

		public void SetMainCamera([NotNull] Camera mainCamera)
		{
			if (mainCamera == null)
			{
				throw new ArgumentNullException("mainCamera");
			}
			mainCamera.transform.SetParent(_mainCameraHolder.transform);
			mainCamera.transform.localPosition = Vector3.zero;
			mainCamera.transform.localRotation = Quaternion.identity;
			CameraScopeZoomer cameraScopeZoomer = mainCamera.GetComponent<CameraScopeZoomer>() ?? mainCamera.gameObject.AddComponent<CameraScopeZoomer>();
			cameraScopeZoomer.PlayerController = this;
			cameraScopeZoomer.SetNotZoomed();
			_mainCamera = mainCamera;
		}

		public void SetFpsCamera([NotNull] Camera fpsCamera)
		{
			if (fpsCamera == null)
			{
				throw new ArgumentNullException("fpsCamera");
			}
			fpsCamera.transform.SetParent(_fpsCameraHolder.transform);
			fpsCamera.transform.localPosition = Vector3.zero;
			fpsCamera.transform.localRotation = Quaternion.identity;
			_fpsCamera = fpsCamera;
		}

		public void SetInputs(PlayerInputs inputs, float deltaTime)
		{
			_isSynchronized = false;
			LocalTime += deltaTime;
			MovementControllerCmd movementControllerCmd = new MovementControllerCmd();
			movementControllerCmd.horizontal = inputs.Horizontal;
			movementControllerCmd.vertical = inputs.Vertical;
			movementControllerCmd.isToJump = inputs.IsToJump;
			movementControllerCmd.isCrouching = inputs.IsCrouching;
			MovementControllerCmd commands = movementControllerCmd;
			AimControlCmd aimControlCmd = new AimControlCmd();
			aimControlCmd.aimType = AimType.DeltaAngle;
			aimControlCmd.deltaAimAngles = inputs.DeltaAimAngles;
			AimControlCmd commands2 = aimControlCmd;
			InventoryControllerCmd inventoryControllerCmd = new InventoryControllerCmd();
			inventoryControllerCmd.slotIndex = inputs.SwitchToWeapon;
			inventoryControllerCmd.toFire = inputs.IsToFire;
			inventoryControllerCmd.toReload = inputs.IsToReload;
			inventoryControllerCmd.toAim = inputs.IsToAim;
			InventoryControllerCmd commands3 = inventoryControllerCmd;
			AimController.ExecuteCommands(commands2, deltaTime, LocalTime);
			MovementController.ExecuteCommands(commands, deltaTime, LocalTime);
			MecanimController.ExecuteCommands(new MecanimControllerCmd(), deltaTime, LocalTime);
			InventoryController.ExecuteCommands(commands3, deltaTime, LocalTime);
		}

		public void SetSnapsot(PlayerSnapshot snapshot)
		{
			_isSynchronized = true;
			MecanimController.SetSnapshot(snapshot.mecanimSnapshot);
			AimController.SetSnapshot(snapshot.aimingSnapshot);
			MovementController.SetSnapshot(snapshot.movementSnapshot);
			InventoryController.SetSnapshot(snapshot.inventorySnapshot);
		}

		public PlayerSnapshot GetSnapshot()
		{
			PlayerSnapshot playerSnapshot = new PlayerSnapshot();
			playerSnapshot.time = LocalTime;
			playerSnapshot.mecanimSnapshot = MecanimController.GetSnapshot();
			playerSnapshot.movementSnapshot = MovementController.GetSnapshot();
			playerSnapshot.aimingSnapshot = AimController.GetSnapshot();
			playerSnapshot.inventorySnapshot = InventoryController.GetSnapshot();
			return playerSnapshot;
		}

		private void OnPhotonInstantiate()
		{
			_photonPlayerId = PhotonView.ownerId;
			_characterPlayer.enabled = !PhotonView.isMine;
			if (!PhotonView.isMine)
			{
				Singleton<PlayerManager>.Instance.OnRemoteInstantiateEvent(this);
			}
			else if (GetHealth() > 0)
			{
				OnLocalInstantiate(true);
			}
			else
			{
				NetworkController.SynchronizationEnabled = false;
			}
		}

		internal void Instantiate(int health, int armor, bool hasHelmet)
		{
			if (!PhotonView.isMine)
			{
				Log.Error("Can't Instantiate remote player");
				return;
			}
			if (GetHealth() > 0)
			{
				Log.Error("Can't instantiate player, already instantiated!");
				return;
			}
			PhotonView.RPC("InstantiateViaServer", PhotonTargets.AllBufferedViaServer, health, armor, hasHelmet);
		}

		[PunRPC]
		private void InstantiateViaServer(int health, int armor, bool hasHelmet)
		{
			PhotonPlayer owner = PhotonView.owner;
			owner.SetHealth(health);
			owner.SetArmor(armor);
			owner.SetHelmet(hasHelmet);
			if (PhotonView.isMine)
			{
				OnLocalInstantiate(false);
			}
		}

		private void OnLocalInstantiate(bool reconnected)
		{
			Singleton<PlayerManager>.Instance.OnLocalInstantiateEvent(this, reconnected);
			NetworkController.SynchronizationEnabled = true;
			SetCharacterVisible(true);
		}

		public int GetHealth()
		{
			return _photonView.owner.GetHealth();
		}

		public int GetArmor()
		{
			return _photonView.owner.GetArmor();
		}

		public bool HasHelmet()
		{
			return _photonView.owner.HasHelmet();
		}

		public bool IsDeath()
		{
			return _photonView.owner.IsDead();
		}

		public void Die()
		{
			PhotonView.RPC("DieViaServer", PhotonTargets.AllViaServer);
		}

		[PunRPC]
		private void DieViaServer()
		{
			PhotonPlayer owner = PhotonView.owner;
			if (owner.IsLocal)
			{
				DestroyPlayer();
			}
			Singleton<HitManager>.Instance.Suicide(owner);
		}

		public void DestroyPlayer()
		{
			if (!PhotonView.isMine)
			{
				Log.Error("Can't destory other player");
			}
			else
			{
				PhotonNetwork.Destroy(base.gameObject);
			}
		}

		internal void OnPhotonDestroy()
		{
			try
			{
				if (!_characterPlayer.enabled)
				{
					Singleton<PlayerManager>.Instance.OnLocalDestroyEvent(this);
				}
				else
				{
					Singleton<PlayerManager>.Instance.OnRemoteDestroyEvent(this, _photonPlayerId);
				}
			}
			finally
			{
				Clear();
			}
		}

		public void FreezeMovement()
		{
		}

		public void SetCharacterVisible(bool isEnabled)
		{
			_isCharacterVisible = isEnabled;
			HitController.Trigger.enabled = isEnabled;
			InventoryController.SetWeaponsVisible(isEnabled);
			SkinnedMeshLodGroup requireComponent = BipedMap.GetRequireComponent<SkinnedMeshLodGroup>();
			if (isEnabled)
			{
				requireComponent.EnableSkinnedMeshRenderer();
			}
			else
			{
				requireComponent.DisableSkinnedMeshRenderer();
			}
		}
	}
}
