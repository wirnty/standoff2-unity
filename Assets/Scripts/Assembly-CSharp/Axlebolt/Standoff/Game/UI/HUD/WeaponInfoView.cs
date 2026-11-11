using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.HUD
{
	public class WeaponInfoView : View
	{
		[SerializeField]
		private Image _weaponImage;

		[SerializeField]
		private Text _magazineAmmo;

		[SerializeField]
		private Text _ammoSlash;

		[SerializeField]
		private Text _restAmmo;

		[SerializeField]
		private Color _magazineColorNormal;

		[SerializeField]
		private Color _magazineColorCritical;

		private WeaponController _weapon;

		private PlayerController _playerController;

		public PlayerController PlayerController
		{
			get
			{
				return _playerController;
			}
			set
			{
				_playerController = value;
			}
		}

		private void SetTextViewVisible(bool isEnabled)
		{
			Text magazineAmmo = _magazineAmmo;
			bool flag = isEnabled;
			_ammoSlash.enabled = flag;
			flag = flag;
			_restAmmo.enabled = flag;
			magazineAmmo.enabled = flag;
		}

		private void Update()
		{
			if (!(_playerController != null) || !(_playerController.InventoryController.CurrentWeapon != null))
			{
				_weaponImage.enabled = false;
				SetTextViewVisible(false);
				return;
			}
			if (_weapon != _playerController.InventoryController.CurrentWeapon)
			{
				_weapon = _playerController.InventoryController.CurrentWeapon;
				_weaponImage.sprite = _weapon.WeaponParameters.Sprites.Icon;
				_weaponImage.enabled = true;
			}
			if (_weapon is GunController)
			{
				SetTextViewVisible(true);
				GunController gunController = (GunController)_weapon;
				_magazineAmmo.text = "" + gunController.CurMagazineAmmo;
				_restAmmo.text = "" + gunController.RestAmmoAmount;
				if ((short)((float)gunController.GunParameters.Ammunition.MagazineCapacity * 0.25f) >= gunController.CurMagazineAmmo)
				{
					_magazineAmmo.color = _magazineColorCritical;
				}
				else
				{
					_magazineAmmo.color = _magazineColorNormal;
				}
			}
		}
	}
}
