using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Player.Ragdoll.Test
{
	public class RagdollTestController : MonoBehaviour
	{
		[SerializeField]
		[Header("Counter Terrorist")]
		[PlayerArms]
		private string ctArms;

		[SerializeField]
		private PlayerCharacters ctCharacters;

		[Space]
		[Header("Terrorist")]
		[SerializeField]
		[PlayerArms]
		private string trArms;

		[SerializeField]
		private PlayerCharacters trCharacters;

		public InputField inputBoneID;

		public InputField inputForce;

		public Transform director;

		private SpawnPointManager _spawnPointManager;

		private Controls playerControls;

		public static RagdollTestController Instance;

		[HideInInspector]
		public float hitImpulse;

		[HideInInspector]
		public BipedMap.Bip targetBone;

		private PlayerController player;

		private void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			PhotonNetwork.offlineMode = true;
			Singleton<WeaponManager>.Instance.Init();
			Singleton<PlayerManager>.Instance.Init(ctArms, ctCharacters, trArms, trCharacters);
			PhotonNetwork.PrefabPool = new PhotonPrefabPool();
			_spawnPointManager = new SpawnPointManager();
			GameObject gameObject = new GameObject();
			gameObject.name = "CONTROLS";
			playerControls = Controls.Instance;
			SpawnPlayer();
			InitTargetCharacter();
		}

		public void InitTargetCharacter()
		{
			Singleton<RagdollManager>.Instance.Initialize(ctCharacters, trCharacters);
		}

		public PlayerController SpawnPlayer()
		{
			player = Singleton<PlayerManager>.Instance.GetFromPool(Singleton<PlayerManager>.Instance.GetFreeId(Team.Ct), _spawnPointManager.GetSpawnPoint(Team.Tr).Position, Quaternion.identity).GetComponent<PlayerController>();
			player.InventoryController.SetWeapon(Singleton<WeaponManager>.Instance.Get(51));
			playerControls.PlayerController = player;
			player.GetComponent<CharacterPlayer>().enabled = false;
			player.SetCharacterVisible(true);
			return player;
		}

		public PlayerController InitPlayer()
		{
			if (player != null)
			{
				Singleton<PlayerManager>.Instance.ReturnToPool(player.gameObject);
			}
			return SpawnPlayer();
		}

		public void Hit()
		{
			playerControls.PlayerController = null;
			int result = -1;
			int.TryParse(inputBoneID.text, out result);
			float result2 = 0f;
			float.TryParse(inputForce.text, out result2);
			HitData hitData = new HitData();
			hitData.Bone = targetBone;
			hitData.Impulse = hitImpulse;
			hitData.Direction = director.transform.forward;
			player.HitController.HitImmediate(hitData);
			playerControls.PlayerController = null;
			player = null;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.K))
			{
				Hit();
			}
		}
	}
}
