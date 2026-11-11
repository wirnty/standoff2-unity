using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory.Animation;
using Axlebolt.Standoff.Inventory.Drop;
using Axlebolt.Standoff.Inventory.Gun;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	public class WeaponDropController : MonoBehaviour, IDropableObject
	{
		public enum State
		{
			Thrown = 0,
			Holded = 1,
			NotStated = 3
		}

		private WeaponController _weaponController;

		private WeaponAnimationController _weaponAnimationController;

		private DropableObjectParameters _dropParameters;

		private MeshCollider _collider;

		private DropableObjectConfig _config = new DropableObjectConfig();

		private float _throwTime = 0f;

		private State _state = State.NotStated;

		public DropableObjectConfig Config
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

		public void PreInitialize(WeaponController weaponController)
		{
			_weaponController = weaponController;
			_weaponAnimationController = weaponController.AnimationController;
			_dropParameters = weaponController.WeaponParameters.DropParameters;
			_collider = GetComponent<MeshCollider>();
			if (GetComponent<MeshCollider>() == null)
			{
				_collider = base.gameObject.AddComponent<MeshCollider>();
				_collider.convex = true;
				_collider.enabled = false;
			}
		}

		public void Initialize()
		{
			Rigidbody component = GetComponent<Rigidbody>();
			if (component != null)
			{
				Object.Destroy(component);
			}
			if (_collider != null)
			{
				_collider.enabled = false;
			}
			_state = State.Holded;
			MonoBehaviour.print("INit drop controller");
		}

		public void InitializeDropableObject()
		{
			Vector3 position = base.transform.position;
			Quaternion rotation = base.transform.rotation;
			base.gameObject.transform.SetParent(null);
			base.transform.position = position;
			base.transform.rotation = rotation;
			Rigidbody component = GetComponent<Rigidbody>();
			if (component == null)
			{
				component = base.gameObject.AddComponent<Rigidbody>();
			}
			if (_collider != null)
			{
				_collider.enabled = true;
			}
			_weaponAnimationController.SetActive(false);
			base.gameObject.layer = 10;
		}

		public DropableObejctIdentityData GetDropableObejctIdentityData()
		{
			DropableObejctIdentityData dropableObejctIdentityData = new DropableObejctIdentityData();
			dropableObejctIdentityData.inventoryID = _weaponController.WeaponId;
			if (_weaponController is GunController)
			{
				dropableObejctIdentityData.magazineAmmo = ((GunController)_weaponController).CurMagazineAmmo;
				dropableObejctIdentityData.restAmmo = ((GunController)_weaponController).RestAmmoAmount;
			}
			return dropableObejctIdentityData;
		}

		public InventoryAttachmentType GetAttachmentType()
		{
			return InventoryAttachmentType.PrimaryWeapon;
		}

		public GameObject GetGameObject()
		{
			return base.gameObject;
		}

		public DropableObjectConfig GetDropableObjectConfig()
		{
			return _config;
		}

		public DropableObjectParameters GetDropableObjectParameters()
		{
			return _dropParameters;
		}

		public void Drop(DropableObjectData dropData)
		{
			MonoBehaviour.print("droping : " + dropData.velocity);
			base.transform.position = dropData.transformData.pos;
			base.transform.eulerAngles = dropData.transformData.rot;
			base.transform.localScale = dropData.transformData.scale;
			Rigidbody component = GetComponent<Rigidbody>();
			component.isKinematic = false;
			component.velocity = dropData.velocity;
			component.angularVelocity = dropData.angularVelocity;
			component.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			_state = State.Thrown;
			_throwTime = Time.time;
		}

		private void Update()
		{
			if (_state == State.Thrown && Time.time - _throwTime > _config.autoDestroyTime)
			{
				Rigidbody component = GetComponent<Rigidbody>();
				if (component != null)
				{
					Object.Destroy(component);
				}
				_state = State.NotStated;
				Singleton<WeaponManager>.Instance.Return(_weaponController);
			}
		}
	}
}
