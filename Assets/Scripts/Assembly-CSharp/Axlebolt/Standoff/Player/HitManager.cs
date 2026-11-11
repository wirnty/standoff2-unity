using System;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Inventory;
using JetBrains.Annotations;

namespace Axlebolt.Standoff.Player
{
	public class HitManager : Singleton<HitManager>
	{
		public readonly Event<HitEventArgs> HitEvent = new Event<HitEventArgs>();

		public readonly Event<PhotonPlayer> SuicideEvent = new Event<PhotonPlayer>();

		private bool _initialized;

		public bool FriendlyFireOn { get; private set; }

		public void Init(bool friendFireOn)
		{
			if (_initialized)
			{
				throw new InvalidOperationException("HitManager already initialized");
			}
			FriendlyFireOn = friendFireOn;
			_initialized = true;
		}

		public void Hit([NotNull] PhotonPlayer shooter, [NotNull] PhotonPlayer victim, [NotNull] HitData hitData, [NotNull] Action<bool> onHit)
		{
			if (shooter == null)
			{
				throw new ArgumentNullException("shooter");
			}
			if (victim == null)
			{
				throw new ArgumentNullException("victim");
			}
			if (hitData == null)
			{
				throw new ArgumentNullException("hitData");
			}
			if (onHit == null)
			{
				throw new ArgumentNullException("onHit");
			}
			if (victim.IsDead() || shooter.IsDead() || (victim.GetTeam() == shooter.GetTeam() && !FriendlyFireOn))
			{
				return;
			}
			int health = victim.GetHealth();
			int num = victim.GetArmor();
			int num2 = hitData.Damage;
			int armorDamage = hitData.ArmorDamage;
			bool flag = BipedMap.IsHead(hitData.Bone);
			if ((flag && victim.HasHelmet()) || !flag)
			{
				num -= armorDamage;
				if (num < 0)
				{
					num2 += -num;
					num = 0;
				}
			}
			else
			{
				num2 += armorDamage;
			}
			health -= num2;
			victim.SetHealth(health);
			victim.SetArmor(num);
			onHit(victim.IsDead());
			HitEventArgs hitEventArgs = GetHitEventArgs(shooter, victim, hitData, num2);
			OnHitEvent(hitEventArgs);
		}

		private static HitEventArgs GetHitEventArgs(PhotonPlayer shooter, PhotonPlayer victim, HitData hitData, int damage)
		{
			WeaponParameters parameters = Singleton<WeaponManager>.Instance.GetParameters((byte)hitData.WeaponId);
			bool headshot = BipedMap.IsHead(hitData.Bone);
			return new HitEventArgs(shooter, victim, damage, parameters, headshot, hitData.Penetrated);
		}

		public void Suicide(PhotonPlayer photonPlayer)
		{
			photonPlayer.SetHealth(0);
			photonPlayer.SetArmor(0);
			OnSuicideEvent(photonPlayer);
		}

		private void OnHitEvent(HitEventArgs eventArgs)
		{
			HitEvent.Invoke(eventArgs);
		}

		protected virtual void OnSuicideEvent(PhotonPlayer player)
		{
			SuicideEvent.Invoke(player);
		}
	}
}
