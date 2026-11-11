using System;
using System.Collections.Generic;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Effects;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Drop;
using Axlebolt.Standoff.Player.Ragdoll;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player
{
	public class PlayerHitController : MonoBehaviour
	{
		private PlayerController _playerController;

		private PlayerRagdollController _playerRagdollController;

		private PlayerHitboxConfig _config;

		private readonly Dictionary<BipedMap.Bip, PlayerHitbox> _playerHitboxes = new Dictionary<BipedMap.Bip, PlayerHitbox>();

		public Action<HitData> OnHit { get; set; }

		public CapsuleCollider Trigger { get; set; }

		public void PreInitialize(PlayerHitboxConfig hitboxConfig)
		{
			_config = hitboxConfig;
			_playerController = GetComponent<PlayerController>();
			_playerRagdollController = GetComponent<PlayerRagdollController>();
			_playerController.CharacterSkinSetEvent += OnCharacterSkinSet;
			GameObject gameObject = new GameObject();
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.name = Tags.PlayerHitTrigger;
			Trigger = gameObject.AddComponent<CapsuleCollider>();
			Trigger.center = hitboxConfig.trigger.center;
			Trigger.direction = hitboxConfig.trigger.direction;
			Trigger.height = hitboxConfig.trigger.height;
			Trigger.radius = hitboxConfig.trigger.radius;
			Trigger.isTrigger = true;
			gameObject.layer = LayerMask.NameToLayer("PlayerHitbox");
			gameObject.tag = Tags.PlayerHitTrigger;
			foreach (PlayerHitboxConfig.HitboxConfig hitbox in _config.hitboxes)
			{
				_playerHitboxes[hitbox.bone] = ConfigureBone(_playerController.BipedMap.GetBone(hitbox.bone).gameObject, hitbox);
			}
		}

		private void OnCharacterSkinSet(BipedMap bipedMap)
		{
			_playerHitboxes.Clear();
			foreach (PlayerHitboxConfig.HitboxConfig hitbox in _config.hitboxes)
			{
				_playerHitboxes[hitbox.bone] = ConfigureBone(bipedMap.GetBone(hitbox.bone).gameObject, hitbox);
			}
		}

		private PlayerHitbox ConfigureBone(GameObject boneGO, PlayerHitboxConfig.HitboxConfig hitboxConfig)
		{
			PlayerHitbox playerHitbox = boneGO.AddComponent<PlayerHitbox>();
			playerHitbox.PlayerHitController = this;
			playerHitbox.Initialize(hitboxConfig, _config.layer);
			return playerHitbox;
		}

		public List<IDropableObject> GetThrowableObjects()
		{
			List<WeaponController> throwableWeapons = _playerController.InventoryController.GetThrowableWeapons();
			List<IDropableObject> list = new List<IDropableObject>();
			foreach (WeaponController item in throwableWeapons)
			{
				list.Add(item.DropController);
			}
			return list;
		}

		public void EnableBoneHit()
		{
			Trigger.enabled = false;
			foreach (KeyValuePair<BipedMap.Bip, PlayerHitbox> playerHitbox in _playerHitboxes)
			{
				playerHitbox.Value.Enable();
			}
		}

		public void DisableBoneHit()
		{
			Trigger.enabled = true;
			foreach (KeyValuePair<BipedMap.Bip, PlayerHitbox> playerHitbox in _playerHitboxes)
			{
				playerHitbox.Value.Disable();
			}
		}

		public void Hit(HitData hitData)
		{
			NetworkWriter networkWriter = new NetworkWriter();
			hitData.Serialize(networkWriter);
			_playerController.PhotonView.RPC("HitViaServer", PhotonTargets.AllBufferedViaServer, networkWriter.AsArray());
		}

		[PunRPC]
		private void HitViaServer(byte[] bytes, PhotonMessageInfo msgInfo)
		{
			PhotonPlayer victim = _playerController.PhotonView.owner;
			PhotonPlayer sender = msgInfo.sender;
			HitData hitData = DeserializeHitData(bytes);
			if (!victim.IsLocal)
			{
				Singleton<CharacterImpactsEmitter>.Instance.Emit(hitData.Point, hitData.Direction, sender.IsLocal);
			}
			Singleton<HitManager>.Instance.Hit(sender, victim, hitData, delegate(bool isDeath)
			{
				if (isDeath)
				{
					CreateRagdoll(hitData);
					if (victim.IsLocal)
					{
						_playerController.DestroyPlayer();
					}
				}
				else
				{
					Action<HitData> onHit = OnHit;
					if (onHit != null)
					{
						onHit(hitData);
					}
				}
			});
		}

		public void HitImmediate(HitData hitData)
		{
			CreateRagdoll(hitData);
			Singleton<PlayerManager>.Instance.ReturnToPool(base.gameObject);
		}

		private void CreateRagdoll(HitData hitData)
		{
			List<IDropableObject> throwableObjects = GetThrowableObjects();
			List<DropableObjectData> objectsDropData = Singleton<RagdollManager>.Instance.GetObjectsDropData(hitData, _playerRagdollController.GetCharacterVelocity(), base.transform, throwableObjects);
			foreach (DropableObjectData item in objectsDropData)
			{
				Singleton<WeaponDropManager>.Instance.DropInventory(item);
			}
			Singleton<RagdollManager>.Instance.Simulate(_playerRagdollController, hitData);
		}

		private static HitData DeserializeHitData(byte[] bytes)
		{
			NetworkReader reader = new NetworkReader(bytes);
			HitData hitData = new HitData();
			hitData.Deserialize(reader);
			return hitData;
		}
	}
}
