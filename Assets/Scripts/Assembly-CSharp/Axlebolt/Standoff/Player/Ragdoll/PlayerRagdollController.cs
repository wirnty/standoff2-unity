using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Player.Aim;
using Axlebolt.Standoff.Player.Inventory;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Ragdoll
{
	public class PlayerRagdollController : MonoBehaviour, IFallingCharacter
	{
		private PlayerController _playerController;

		private InventoryController _inventoryController;

		private AimController _aimController;

		private FallingCharacterConfig _config = new FallingCharacterConfig();

		private HitData _hitData = new HitData();

		public FallingCharacterConfig Config
		{
			get
			{
				return _config;
			}
			set
			{
				_config = value;
			}
		}

		public void PreInitialize()
		{
			_playerController = GetComponent<PlayerController>();
			_inventoryController = GetComponent<InventoryController>();
			_aimController = _playerController.AimController;
		}

		public void HitBone(HitData hitData)
		{
			_hitData = hitData;
			_aimController.onAllChangesApplied += ApplyBoneHit;
		}

		public FallingCharacterConfig GetFallingCharacterConfig()
		{
			return _config;
		}

		private void ApplyBoneHit()
		{
			Singleton<RagdollManager>.Instance.Simulate(this, _hitData);
		}

		public string GetName()
		{
			return base.gameObject.name;
		}

		public BipedMap GetBipedMap()
		{
			return _playerController.BipedMap;
		}

		public Vector3 GetCharacterVelocity()
		{
			return _playerController.MovementController.Velocity;
		}

		public int GetPlayerId()
		{
			return _playerController.PlayerId;
		}

		public void OnSimulateFalling()
		{
			_inventoryController.Clear();
			base.gameObject.SetActive(false);
		}
	}
}
