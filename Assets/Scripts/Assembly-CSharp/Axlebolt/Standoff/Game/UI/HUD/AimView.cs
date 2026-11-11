using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.HUD
{
	public class AimView : View
	{
		[SerializeField]
		private RectTransform _interactiveArea;

		[SerializeField]
		private RectTransform _topAccuracyPanel;

		[SerializeField]
		private RectTransform _topRecoilPanel;

		[SerializeField]
		private RectTransform _bottomAccuracyPanel;

		[SerializeField]
		private RectTransform _bottomRecoilPanel;

		[SerializeField]
		private RectTransform _rightAccuracyPanel;

		[SerializeField]
		private RectTransform _rightRecoilPanel;

		[SerializeField]
		private RectTransform _leftAccuracyPanel;

		[SerializeField]
		private RectTransform _leftRecoilPanel;

		[SerializeField]
		private GameObject _defaultSight;

		[SerializeField]
		private GameObject _sniperSight;

		[SerializeField]
		private Image _sniperCrossLineHor;

		[SerializeField]
		private Image _sniperCrossLineVer;

		[SerializeField]
		private RectTransform _sniperCrossBlurHor;

		[SerializeField]
		private RectTransform _sniperCrossBlurVer;

		[SerializeField]
		private AnimationCurve _sniperCrossLineFadeCurve;

		[SerializeField]
		private AnimationCurve _sniperCrossBlupShrinkCurve;

		[SerializeField]
		private float _sniperCrossBlurWidth;

		public PlayerController PlayerController { get; set; }

		private void DrawDefaultRecoilLines(WeaponController weaponController)
		{
			if (_sniperSight.activeSelf)
			{
				_sniperSight.SetActive(false);
			}
			if (!_defaultSight.activeSelf)
			{
				_defaultSight.SetActive(true);
			}
			AccuracyData accuracyData = weaponController.GetAccuracyData();
			Camera main = Camera.main;
			Vector3 position = Quaternion.AngleAxis(0f - accuracyData.AccuracyAngle, main.transform.right) * main.transform.forward + main.transform.position;
			Vector3 position2 = Quaternion.AngleAxis(0f - accuracyData.RecoilAngle, main.transform.right) * main.transform.forward + main.transform.position;
			Vector3 vector = new Vector3(Screen.width / 2, Screen.height / 2);
			position = main.WorldToScreenPoint(position) - vector;
			position2 = main.WorldToScreenPoint(position2) - vector;
			float num = _interactiveArea.rect.yMax - _interactiveArea.rect.yMin;
			float num2 = position.y / (float)Screen.height;
			float num3 = position2.y / (float)Screen.height;
			float num4 = num * num2;
			float a = num * num3;
			a = Mathf.Max(a, num4);
			_topAccuracyPanel.localPosition = new Vector3(0f, num4, 0f);
			_topRecoilPanel.localPosition = new Vector3(0f, a, 0f);
			_bottomAccuracyPanel.localPosition = new Vector3(0f, num4, 0f);
			_bottomRecoilPanel.localPosition = new Vector3(0f, a, 0f);
			_rightAccuracyPanel.localPosition = new Vector3(0f, num4, 0f);
			_rightRecoilPanel.localPosition = new Vector3(0f, a, 0f);
			_leftAccuracyPanel.localPosition = new Vector3(0f, num4, 0f);
			_leftRecoilPanel.localPosition = new Vector3(0f, a, 0f);
		}

		private void DrawSniperScope(GunController gunController)
		{
			if (_defaultSight.activeSelf)
			{
				_defaultSight.SetActive(false);
			}
			if (gunController.CurrentAimingMode == GunController.AimingMode.Aiming)
			{
				if (!_sniperSight.activeSelf)
				{
					_sniperSight.SetActive(true);
				}
				float time = gunController.GetAccuracyData().AccuracyAngle / 1f;
				_sniperCrossBlurHor.sizeDelta = new Vector2(_sniperCrossBlurWidth * _sniperCrossBlupShrinkCurve.Evaluate(time), 1080f);
				_sniperCrossBlurVer.sizeDelta = new Vector2(_sniperCrossBlurWidth * _sniperCrossBlupShrinkCurve.Evaluate(time), 1080f);
				Color color = _sniperCrossLineHor.color;
				color.a = _sniperCrossLineFadeCurve.Evaluate(time);
				Image sniperCrossLineHor = _sniperCrossLineHor;
				Color color2 = color;
				_sniperCrossLineVer.color = color2;
				sniperCrossLineHor.color = color2;
			}
			if (gunController.CurrentAimingMode != 0 && _sniperSight.activeSelf)
			{
				_sniperSight.SetActive(false);
			}
		}

		private void Update()
		{
			if (!(PlayerController != null) || !(PlayerController.InventoryController.CurrentWeapon != null))
			{
				return;
			}
			WeaponController currentWeapon = PlayerController.InventoryController.CurrentWeapon;
			if (currentWeapon is GunController)
			{
				GunController gunController = (GunController)currentWeapon;
				if (gunController.GunParameters.SightType == SightType.Default)
				{
					DrawDefaultRecoilLines(currentWeapon);
				}
				if (gunController.GunParameters.SightType == SightType.SniperScope)
				{
					DrawSniperScope(gunController);
				}
			}
		}
	}
}
