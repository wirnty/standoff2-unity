using System;
using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Player;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.HitHandling
{
	[Serializable]
	public class BulletParameters
	{
		[SerializeField]
		private WeaponId _weaponId;

		[SerializeField]
		private float _penetrationPower;

		[SerializeField]
		private float _armorPenetration;

		[SerializeField]
		private float _impulse;

		[SerializeField]
		private Damage _damage;

		public WeaponId WeaponId
		{
			[CompilerGenerated]
			get
			{
				return _weaponId;
			}
		}

		public float PenetrationPower
		{
			[CompilerGenerated]
			get
			{
				return _penetrationPower;
			}
		}

		public float ArmorPenetration
		{
			[CompilerGenerated]
			get
			{
				return _armorPenetration;
			}
		}

		public float Impulse
		{
			[CompilerGenerated]
			get
			{
				return _impulse;
			}
		}

		public Damage Damage
		{
			[CompilerGenerated]
			get
			{
				return _damage;
			}
		}

		public static BulletParameters Create(WeaponId weaponId, float penetrationPower, float armorPenetration, float impulse, [NotNull] Damage damage)
		{
			if (damage == null)
			{
				throw new ArgumentNullException("damage");
			}
			BulletParameters bulletParameters = new BulletParameters();
			bulletParameters._weaponId = weaponId;
			bulletParameters._penetrationPower = penetrationPower;
			bulletParameters._armorPenetration = armorPenetration;
			bulletParameters._impulse = impulse;
			bulletParameters._damage = damage;
			return bulletParameters;
		}

		public int GetDamage(BipedMap.Bip bone)
		{
			if (BipedMap.IsHead(bone))
			{
				return _damage.HeadDamage;
			}
			if (BipedMap.IsChestAndArmsDamage(bone))
			{
				return _damage.ChestAndArmsDamage;
			}
			if (BipedMap.IsLegs(bone))
			{
				return _damage.LegsDamage;
			}
			if (BipedMap.IsStomach(bone))
			{
				return _damage.StomachDamage;
			}
			Debug.LogError(string.Format("Unsupported bone {0}", bone));
			return 0;
		}
	}
}
