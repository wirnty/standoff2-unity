using System;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI.Weaponary
{
	public class PlayerWeaponsView : View
	{
		public const int MaxWeaponCount = 5;

		[SerializeField]
		[NotNull]
		private PlayerWeaponView _weaponPrefab;

		private ViewPool<PlayerWeaponView> _pool;

		public WeaponParameters[] Weapons
		{
			set
			{
				int num = Math.Min(value.Length, 5);
				PlayerWeaponView[] items = _pool.GetItems(num);
				for (int i = 0; i < num; i++)
				{
					items[i].Weapon = value[i];
				}
			}
		}

		private void Awake()
		{
			_pool = new ViewPool<PlayerWeaponView>(_weaponPrefab, 5);
		}
	}
}
