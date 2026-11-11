using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Settings;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	public class WeaponManager : Singleton<WeaponManager>
	{
		public readonly Event<WeaponController> ShootEvent = new Event<WeaponController>();

		private const byte PrimaryWeapon = 15;

		public const int PoolSize = 1;

		private static readonly Log Log = Log.Create(typeof(WeaponManager));

		private WeaponPool _pool;

		public WeaponParameters[] Weapons
		{
			[CompilerGenerated]
			get
			{
				return _pool.Weapons;
			}
		}

		public void Init()
		{
			Init(WeaponUtility.LoadWeapons());
		}

		public void Init(WeaponParameters[] weapons)
		{
			if (_pool != null)
			{
				throw new InvalidOperationException("WeaponManager already initialized");
			}
			_pool = new WeaponPool(weapons, 1, base.transform);
			QualityManager.Instance.ModelDetailChanged += OnModelDetailChanged;
			QualityManager.Instance.ShaderDetailChanged += OnShaderDetailChanged;
		}

		private void CheckIsInitialized()
		{
			if (_pool == null)
			{
				throw new InvalidOperationException("WeaponManager does not initialized");
			}
		}

		public WeaponParameters GetParameters(byte weaponId)
		{
			CheckIsInitialized();
			return _pool.GetParameters(weaponId);
		}

		public WeaponController GetPrimaryWeapon()
		{
			return Get(15);
		}

		public WeaponController Get(WeaponId id)
		{
			return Get((byte)id);
		}

		public WeaponController Get(byte id)
		{
			CheckIsInitialized();
			return _pool.GetFromPool(id);
		}

		public void Return(WeaponController weapon)
		{
			CheckIsInitialized();
			_pool.ReturnToPool(weapon);
		}

		private void OnModelDetailChanged()
		{
			_pool.RefreshLods();
		}

		private void OnShaderDetailChanged()
		{
			_pool.UpdateMaterials();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			QualityManager.Instance.ModelDetailChanged -= OnModelDetailChanged;
			QualityManager.Instance.ShaderDetailChanged -= OnShaderDetailChanged;
		}

		public WeaponId GetRandomGunId(params GunType[] gunTypes)
		{
			GunParameters[] array = (from GunParameters gun in _pool.Weapons.Where((WeaponParameters weapon) => weapon is GunParameters)
				where gunTypes.Contains(gun.GunType)
				select gun).ToArray();
			if (array.Length == 0)
			{
				throw new InvalidOperationException(string.Format("Invalid gunTypes {0}, not found", gunTypes));
			}
			GunParameters gunParameters = array[UnityEngine.Random.Range(0, array.Length)];
			return gunParameters.Id;
		}

		internal void OnShootEvent(WeaponController weaponController)
		{
			ShootEvent.Invoke(weaponController);
		}
	}
}
