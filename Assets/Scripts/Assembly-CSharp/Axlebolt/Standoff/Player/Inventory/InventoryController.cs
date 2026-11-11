using System.Collections.Generic;
using System.Linq;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player.Aim;
using Axlebolt.Standoff.Player.Mecanim;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Inventory
{
	public class InventoryController : BaseController<InventorySnapshot, InventoryControllerCmd>, IAimingEvents
	{
		private PlayerController _playerController;

		private MecanimController _mecanimController;

		private bool _isFpsViewDisabled;

		private readonly Dictionary<byte, WeaponController> _weaponSlots = new Dictionary<byte, WeaponController>();

		private byte _currentSlot = byte.MaxValue;

		private readonly List<IInventoryEvents> _inventoryEventHandlers = new List<IInventoryEvents>();

		private ViewMode _viewMode;

		private bool _weaponsVisible;

		public WeaponController CurrentWeapon { get; private set; }

		public float LocalTime { get; private set; }

		public float DeltaTime { get; private set; }

		public override void PreInitialize()
		{
			_playerController = GetComponent<PlayerController>();
			_playerController.CharacterSkinSetEvent += CharacterSkinSet;
			_mecanimController = _playerController.MecanimController;
			LocalTime = _playerController.LocalTime;
			_mecanimController.onAnimatorUpdated += OnAnimatorUpdated;
			_inventoryEventHandlers.AddRange(GetComponents<IInventoryEvents>());
		}

		public override void Initialize()
		{
			Clear();
		}

		public void Clear()
		{
			foreach (KeyValuePair<byte, WeaponController> weaponSlot in _weaponSlots)
			{
				Singleton<WeaponManager>.Instance.Return(weaponSlot.Value);
			}
			_weaponSlots.Clear();
			CurrentWeapon = null;
			_currentSlot = byte.MaxValue;
			EnableFPSView();
		}

		public void CharacterSkinSet(BipedMap bipedMap)
		{
			foreach (WeaponController value in _weaponSlots.Values)
			{
				Vector3 localPosition = value.transform.localPosition;
				Quaternion localRotation = value.transform.localRotation;
				value.transform.SetParent(bipedMap.GetBone(BipedMap.Bip.RightHand).transform);
				value.transform.localPosition = localPosition;
				value.transform.localRotation = localRotation;
				value.OnCharacterSkinSet(bipedMap);
			}
		}

		private void OnAnimatorUpdated()
		{
			if (CurrentWeapon != null)
			{
				CurrentWeapon.OnAnimatorUpdated();
			}
		}

		public override void ExecuteCommands(InventoryControllerCmd commands, float duration, float time)
		{
			LocalTime = time;
			DeltaTime = duration;
			LocalUpdate();
			if (commands.slotIndex != -1 && commands.slotIndex != _currentSlot)
			{
				SwitchWeapon((byte)commands.slotIndex);
			}
			if (CurrentWeapon != null)
			{
				CurrentWeapon.ExecuteCommands(commands, duration, time);
			}
		}

		public override InventorySnapshot GetSnapshot()
		{
			InventorySnapshot inventorySnapshot = new InventorySnapshot();
			inventorySnapshot.time = LocalTime;
			InventorySnapshot inventorySnapshot2 = inventorySnapshot;
			foreach (KeyValuePair<byte, WeaponController> weaponSlot in _weaponSlots)
			{
				WeaponController value = weaponSlot.Value;
				InventorySnapshot.SlotItem slotItem = new InventorySnapshot.SlotItem();
				slotItem.slotIndex = weaponSlot.Key;
				slotItem.inventoryId = value.WeaponId;
				slotItem.inventoryInstanceId = value.InventoryInstanceId;
				InventorySnapshot.SlotItem item = slotItem;
				inventorySnapshot2.slot.Add(item);
			}
			inventorySnapshot2.currentSlotIndex = _currentSlot;
			if (CurrentWeapon != null)
			{
				inventorySnapshot2.weaponSnapshot = CurrentWeapon.GetSnapshot();
			}
			return inventorySnapshot2;
		}

		public override void SetSnapshot(InventorySnapshot snapshot)
		{
			List<byte> list = new List<byte>();
			list.AddRange(_weaponSlots.Keys);
			foreach (InventorySnapshot.SlotItem item in snapshot.slot)
			{
				if (!_weaponSlots.ContainsKey(item.slotIndex))
				{
					_weaponSlots[item.slotIndex] = GetWeapon(item.inventoryId);
					list.Remove(item.slotIndex);
					continue;
				}
				WeaponController weaponController = _weaponSlots[item.slotIndex];
				if (weaponController.WeaponId != item.inventoryId || weaponController.InventoryInstanceId != item.inventoryInstanceId)
				{
					DropWeapon(weaponController);
					_weaponSlots[item.slotIndex] = GetWeapon(item.inventoryId);
				}
				list.Remove(item.slotIndex);
			}
			foreach (byte item2 in list)
			{
				WeaponController weapon = _weaponSlots[item2];
				_weaponSlots.Remove(item2);
				DropWeapon(weapon);
			}
			CurrentWeapon = _weaponSlots[snapshot.currentSlotIndex];
			if (CurrentWeapon.HandlingState != 0)
			{
				CurrentWeapon.SetAsDefault(LocalTime);
			}
			foreach (WeaponController value in _weaponSlots.Values)
			{
				if (value != CurrentWeapon && value.HandlingState != WeaponController.HandleState.Secondary)
				{
					value.SetAsSecondary();
				}
			}
			CurrentWeapon.SetSnapshot(snapshot.weaponSnapshot);
			SetWeaponsVisible(_weaponsVisible);
			_currentSlot = snapshot.currentSlotIndex;
		}

		private WeaponController GetWeapon(byte inventoryId)
		{
			WeaponController weaponController = Singleton<WeaponManager>.Instance.Get(inventoryId);
			weaponController.SetPlayer(_playerController);
			return weaponController;
		}

		private void DisableFPSView()
		{
			_playerController.FpsCameraHolder.SetActive(false);
			_isFpsViewDisabled = true;
		}

		private void EnableFPSView()
		{
			_playerController.FpsCameraHolder.SetActive(true);
			_isFpsViewDisabled = false;
		}

		private void LocalUpdate()
		{
			if (CurrentWeapon != null && _isFpsViewDisabled && !_mecanimController.IsStartingWeaponSwitch(CurrentWeapon.WeaponName))
			{
				EnableFPSView();
			}
		}

		public void SetWeapon(WeaponController weapon)
		{
			byte slotIndex = weapon.SlotIndex;
			if (_weaponSlots.ContainsKey(slotIndex))
			{
				DropWeapon(_weaponSlots[slotIndex]);
			}
			_weaponSlots[slotIndex] = weapon;
			weapon.SetViewMode(_viewMode);
			weapon.SetPlayer(_playerController);
			weapon.SetVisible(_weaponsVisible);
			TakeWeapon(slotIndex);
		}

		public WeaponController[] GetWeapons()
		{
			return _weaponSlots.Values.ToArray();
		}

		public void SwitchWeapon(byte slotIndex)
		{
			if (_currentSlot != slotIndex && _weaponSlots.ContainsKey(slotIndex))
			{
				TakeWeapon(slotIndex);
			}
		}

		public void ReswitchCurrentWeapon()
		{
			TakeWeapon(_currentSlot);
		}

		private void TakeWeapon(byte slotIndex)
		{
			if (CurrentWeapon != null)
			{
				CurrentWeapon.SetAsSecondary();
			}
			CurrentWeapon = _weaponSlots[slotIndex];
			_currentSlot = slotIndex;
			CurrentWeapon.SetAsDefault(LocalTime);
			CurrentWeapon.SetVisible(_weaponsVisible);
			DisableFPSView();
			_mecanimController.SwitchWeapon(CurrentWeapon.WeaponId);
			OnWeaponSet(CurrentWeapon);
		}

		private void DropWeapon(WeaponController weapon)
		{
			if (weapon == CurrentWeapon)
			{
				CurrentWeapon = null;
			}
			Singleton<WeaponManager>.Instance.Return(weapon);
		}

		public List<WeaponController> GetThrowableWeapons()
		{
			List<WeaponController> list = new List<WeaponController>();
			foreach (KeyValuePair<byte, WeaponController> weaponSlot in _weaponSlots)
			{
				if (weaponSlot.Value.IsDroppable())
				{
					list.Add(weaponSlot.Value);
				}
			}
			return list;
		}

		public void SetWeaponsVisible(bool isVisible)
		{
			_weaponsVisible = isVisible;
			foreach (WeaponController value in _weaponSlots.Values)
			{
				value.SetVisible(isVisible);
			}
		}

		private void OnWeaponSet(WeaponController weapon)
		{
			foreach (IInventoryEvents inventoryEventHandler in _inventoryEventHandlers)
			{
				inventoryEventHandler.OnWeaponSet(weapon);
			}
		}

		public void OnViewModeSet(ViewMode viewMode)
		{
			_viewMode = viewMode;
			foreach (WeaponController value in _weaponSlots.Values)
			{
				value.SetViewMode(viewMode);
			}
		}
	}
}
