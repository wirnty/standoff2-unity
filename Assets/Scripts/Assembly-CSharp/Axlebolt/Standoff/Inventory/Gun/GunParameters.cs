using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Effects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Axlebolt.Standoff.Inventory.Gun
{
	[CreateAssetMenu(fileName = "NewGun", menuName = "Standoff/Create Gun", order = 2)]
	public class GunParameters : WeaponParameters
	{
		[SerializeField]
		[Header("Gun Parameters")]
		[Space]
		private GunType _gunType;

		[SerializeField]
		private SightType _sightType;

		[SerializeField]
		private float _scopeAimSensitivityMult;

		[SerializeField]
		private Ammunition _ammunition;

		[SerializeField]
		private Damage _damage;

		[SerializeField]
		private int _fireRate;

		[SerializeField]
		private int _recoilControl;

		[SerializeField]
		private RecoilParameters _recoilParameters;

		[FormerlySerializedAs("AccuracyAdditiveCurve")]
		[SerializeField]
		private AnimationCurve _accuracyAdditiveCurve;

		[SerializeField]
		private AnimationCurve _additiveDispertionCurve;

		[SerializeField]
		private AnimationCurve _additiveNoAimDispertionCurve;

		[SerializeField]
		private int _accurateRange;

		[SerializeField]
		[Range(0f, 100f)]
		private float _armorPenetration;

		[SerializeField]
		private int _penetrationPower;

		[SerializeField]
		private float _reloadDuration;

		[SerializeField]
		private float _magazineInsertTime;

		[SerializeField]
		private float _takeDuration;

		[SerializeField]
		private float _preReloadTime;

		[SerializeField]
		private int _impulse;

		[SerializeField]
		private MuzzleFlashType _muzzleFlashType;

		public GunType GunType
		{
			[CompilerGenerated]
			get
			{
				return _gunType;
			}
		}

		public SightType SightType
		{
			[CompilerGenerated]
			get
			{
				return _sightType;
			}
		}

		public float ScopeAimSensitivityMult
		{
			[CompilerGenerated]
			get
			{
				return _scopeAimSensitivityMult;
			}
		}

		public Ammunition Ammunition
		{
			[CompilerGenerated]
			get
			{
				return _ammunition;
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

		public int FireRate
		{
			[CompilerGenerated]
			get
			{
				return _fireRate;
			}
		}

		public RecoilParameters RecoilParameters
		{
			[CompilerGenerated]
			get
			{
				return _recoilParameters;
			}
		}

		public AnimationCurve AccuracyAdditiveCurve
		{
			[CompilerGenerated]
			get
			{
				return _accuracyAdditiveCurve;
			}
		}

		public AnimationCurve AdditiveNoAimDispertionCurve
		{
			[CompilerGenerated]
			get
			{
				return _additiveNoAimDispertionCurve;
			}
		}

		public int RecoilControl
		{
			[CompilerGenerated]
			get
			{
				return _recoilControl;
			}
		}

		public int AccurateRange
		{
			[CompilerGenerated]
			get
			{
				return _accurateRange;
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

		public int PenetrationPower
		{
			[CompilerGenerated]
			get
			{
				return _penetrationPower;
			}
		}

		public float ReloadDuration
		{
			[CompilerGenerated]
			get
			{
				return _reloadDuration;
			}
		}

		public float MagazineInsertTime
		{
			[CompilerGenerated]
			get
			{
				return _magazineInsertTime;
			}
		}

		public float TakeDuration
		{
			[CompilerGenerated]
			get
			{
				return _takeDuration;
			}
		}

		public float PreReloadTime
		{
			[CompilerGenerated]
			get
			{
				return _preReloadTime;
			}
		}

		public int Impulse
		{
			[CompilerGenerated]
			get
			{
				return _impulse;
			}
		}

		public MuzzleFlashType MuzzleFlashType
		{
			[CompilerGenerated]
			get
			{
				return _muzzleFlashType;
			}
		}
	}
}
