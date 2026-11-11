using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Gun;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.HUD
{
	public class ItemSelectViewItem : MonoBehaviour
	{
		[SerializeField]
		private Image _weaponImage;

		[SerializeField]
		private Text _magazineAmmo;

		[SerializeField]
		private Text _slashAmmo;

		[SerializeField]
		private Text _restAmmo;

		[SerializeField]
		private Text _quantity;

		[SerializeField]
		private GameObject _headerGO;

		[SerializeField]
		private CanvasGroup _backgroundCanvasGroup;

		[SerializeField]
		private AnimationCurve _fadeInCurve;

		[SerializeField]
		private float _fadeInDuration;

		private float _fadeinStartTime;

		private float _targetAlpha = 0f;

		private WeaponController _weaponController;

		private CanvasGroup _canvasGroup
		{
			get
			{
				return GetComponent<CanvasGroup>();
			}
		}

		public void SetWeapon(WeaponController weaponController)
		{
			_weaponController = weaponController;
			_magazineAmmo.enabled = false;
			_slashAmmo.enabled = false;
			_restAmmo.enabled = false;
			_quantity.enabled = false;
			_weaponImage.enabled = false;
			_weaponImage.enabled = false;
			if (!(weaponController == null))
			{
				_weaponImage.enabled = true;
				_weaponImage.sprite = _weaponController.WeaponParameters.Sprites.Glow;
				if (_weaponController is GunController)
				{
					_weaponImage.enabled = true;
					_weaponImage.sprite = _weaponController.WeaponParameters.Sprites.Icon;
					_magazineAmmo.enabled = true;
					_magazineAmmo.text = "" + ((GunController)_weaponController).CurMagazineAmmo;
					_slashAmmo.enabled = true;
					_restAmmo.enabled = true;
					_restAmmo.text = "" + ((GunController)_weaponController).RestAmmoAmount;
				}
			}
		}

		public void FadeIn()
		{
			_canvasGroup.alpha = 0f;
			_backgroundCanvasGroup.alpha = 0f;
			_fadeinStartTime = Time.time;
		}

		public void SetSelected(bool isSelected)
		{
			_headerGO.SetActive(isSelected);
			if (isSelected)
			{
				_targetAlpha = 1f;
			}
			else
			{
				_targetAlpha = 0.7f;
			}
		}

		private void Update()
		{
			if (Time.time - _fadeinStartTime < _fadeInDuration)
			{
				_backgroundCanvasGroup.alpha = _fadeInCurve.Evaluate((Time.time - _fadeinStartTime) / _fadeInDuration);
				_canvasGroup.alpha = Mathf.Min(_targetAlpha, _fadeInCurve.Evaluate((Time.time - _fadeinStartTime) / _fadeInDuration));
			}
			else
			{
				_backgroundCanvasGroup.alpha = 1f;
				_canvasGroup.alpha = _targetAlpha;
			}
		}
	}
}
