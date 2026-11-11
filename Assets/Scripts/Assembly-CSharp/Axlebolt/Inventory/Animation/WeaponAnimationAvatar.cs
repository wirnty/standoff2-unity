using UnityEngine;

namespace Axlebolt.Inventory.Animation
{
	public class WeaponAnimationAvatar
	{
		public enum AvatarDefinition
		{
			Weapon = 0,
			Magazine1 = 1,
			Magazine2 = 2,
			Trigger = 3,
			GunLock1 = 4,
			GunLock2 = 5,
			RightHand = 6,
			LeftHand = 7
		}

		public class AvatarElement
		{
			public AvatarDefinition element;

			public Transform elementTransform;
		}
	}
}
