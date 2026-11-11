using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Inventory;
using JetBrains.Annotations;

namespace Axlebolt.Standoff.Player
{
	public class HitEventArgs
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly PhotonPlayer _003CShooter_003Ek__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly PhotonPlayer _003CVictim_003Ek__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly int _003CDamage_003Ek__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly WeaponParameters _003CWeapon_003Ek__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly bool _003CHeadshot_003Ek__BackingField;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly bool _003CPenetrated_003Ek__BackingField;

		public PhotonPlayer Shooter
		{
			[CompilerGenerated]
			get
			{
				return _003CShooter_003Ek__BackingField;
			}
		}

		public PhotonPlayer Victim
		{
			[CompilerGenerated]
			get
			{
				return _003CVictim_003Ek__BackingField;
			}
		}

		public int Damage
		{
			[CompilerGenerated]
			get
			{
				return _003CDamage_003Ek__BackingField;
			}
		}

		public WeaponParameters Weapon
		{
			[CompilerGenerated]
			get
			{
				return _003CWeapon_003Ek__BackingField;
			}
		}

		public bool Headshot
		{
			[CompilerGenerated]
			get
			{
				return _003CHeadshot_003Ek__BackingField;
			}
		}

		public bool Penetrated
		{
			[CompilerGenerated]
			get
			{
				return _003CPenetrated_003Ek__BackingField;
			}
		}

		public HitEventArgs([NotNull] PhotonPlayer shooter, [NotNull] PhotonPlayer victim, int damage, [NotNull] WeaponParameters weapon, bool headshot, bool penetrated)
		{
			_003CShooter_003Ek__BackingField = shooter;
			_003CVictim_003Ek__BackingField = victim;
			_003CDamage_003Ek__BackingField = damage;
			_003CWeapon_003Ek__BackingField = weapon;
			_003CHeadshot_003Ek__BackingField = headshot;
			_003CPenetrated_003Ek__BackingField = penetrated;
		}
	}
}
