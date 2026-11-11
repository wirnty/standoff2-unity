using System.Collections.Generic;
using Axlebolt.Standoff.Game.UI.HUD.Test;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.TouchControl;
using Axlebolt.Standoff.UI;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI.HUD
{
	public class HUDView : View
	{
		public static HUDView Instance;

		public AimView AimView;

		public ItemSelectView ItemSelectView;

		public WeaponInfoView WeaponInfoView;

		public PlayerStatsView PlayerStatsView;

		public HitIndicatorView HitIndicatorView;

		public PlayerTestView PlayerTestView;

		private PlayerController _playerConttoller;

		private TouchController _touchController;

		public PlayerController PlayerController
		{
			set
			{
				_playerConttoller = value;
				AimView.PlayerController = _playerConttoller;
				WeaponInfoView.PlayerController = _playerConttoller;
				PlayerStatsView.PlayerController = _playerConttoller;
				PlayerTestView.PlayerController = _playerConttoller;
				HitIndicatorView.PlayerController = _playerConttoller;
			}
		}

		private void Awake()
		{
			Instance = this;
		}

		public void Initialize(TouchController touchController)
		{
			_touchController = touchController;
			_touchController.WeaponSelectArea.onItemSelect += OnWeaponSelect;
			_touchController.WeaponSelectArea.onItemSelected += OnWeaponSelected;
			_touchController.WeaponSelectArea.onTouchDownEvent += OnWeaponTouchDown;
			_touchController.GrenadeSelectArea.onItemSelect += OnGrenadeSelect;
			_touchController.GrenadeSelectArea.onItemSelected += OnGrenadeSelected;
			_touchController.GrenadeSelectArea.onTouchDownEvent += OnGreandeTouchDown;
		}

		private void OnWeaponSelect(int slotNO)
		{
			ItemSelectView.SetSelectedSlot(slotNO);
		}

		private void OnWeaponSelected(int slotNO)
		{
			ItemSelectView.Hide();
		}

		private void OnWeaponTouchDown()
		{
			WeaponController[] weapons = _playerConttoller.InventoryController.GetWeapons();
			List<WeaponController> list = new List<WeaponController>();
			for (int i = 0; i < 4; i++)
			{
				list.Add(null);
			}
			int num = 0;
			WeaponController[] array = weapons;
			foreach (WeaponController weaponController in array)
			{
				if (num > 3)
				{
					Debug.LogError("Max Inventory Count Exceeded");
					break;
				}
				list[weaponController.SlotIndex] = weaponController;
				num++;
			}
			ItemSelectView.Show();
			ItemSelectView.SetWeaponList(list);
		}

		private void OnGrenadeSelect(int slotNO)
		{
		}

		private void OnGrenadeSelected(int slotNO)
		{
		}

		public void OnGreandeTouchDown()
		{
		}
	}
}
