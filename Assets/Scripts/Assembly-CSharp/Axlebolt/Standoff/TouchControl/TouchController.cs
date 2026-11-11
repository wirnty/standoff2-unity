using Axlebolt.Standoff.Player;
using UnityEngine;

namespace Axlebolt.Standoff.TouchControl
{
	public class TouchController : MonoBehaviour
	{
		[SerializeField]
		private RectTransform _interactiveArea;

		[SerializeField]
		private AimingArea _aimingArea;

		public ItemSelectArea WeaponSelectArea;

		public ItemSelectArea GrenadeSelectArea;

		[SerializeField]
		private FloatingJoystick _joystick;

		[SerializeField]
		private ShootArea _shootArea;

		[SerializeField]
		private InteractableButton _shootButton;

		[SerializeField]
		private InteractableButton _jumpButton;

		[SerializeField]
		private InteractableButton _aimButton;

		[SerializeField]
		private InteractableButton _crouchButton;

		[SerializeField]
		private ClingingButton _crouchButtonClinger;

		[SerializeField]
		private DoubleTapArea _jumpDoubleTapArea;

		private TouchHandler _touchHandler;

		private bool _isAimButtonActive = true;

		private PlayerInputs _playerInputs = new PlayerInputs();

		private bool _buttonsVisible = true;

		public InteractableButton AimButton
		{
			get
			{
				return _aimButton;
			}
			set
			{
				_aimButton = value;
			}
		}

		public void Initialize()
		{
			_touchHandler = base.gameObject.AddComponent<TouchHandler>();
			_touchHandler.interactiveArea = _interactiveArea;
			_touchHandler.interactableZoneList.Add(_aimingArea);
			_touchHandler.interactableZoneList.Add(WeaponSelectArea);
			_touchHandler.interactableZoneList.Add(GrenadeSelectArea);
			_touchHandler.interactableZoneList.Add(_joystick);
			_touchHandler.interactableZoneList.Add(_shootArea);
			_touchHandler.interactableZoneList.Add(_shootButton);
			_touchHandler.interactableZoneList.Add(_jumpButton);
			_touchHandler.interactableZoneList.Add(_aimButton);
			_touchHandler.interactableZoneList.Add(_crouchButton);
			_touchHandler.interactableZoneList.Add(_jumpDoubleTapArea);
			GrenadeSelectArea.IsActive = false;
			_touchHandler.Initialize();
			WeaponSelectArea.onItemSelect += OnWeaponSelect;
			WeaponSelectArea.onItemSelected += OnWeaponSelected;
			GrenadeSelectArea.onItemSelect += OnGrenadeSelect;
			GrenadeSelectArea.onItemSelected += OnGrenadeSelected;
			_aimButton.onButtonDownEvent += OnAimClicked;
			_jumpDoubleTapArea.onSecondTapEvent += OnJumpDoubleTouch;
		}

		private void Awake()
		{
			Initialize();
		}

		public void OnWeaponSelect(int slotIndex)
		{
		}

		private void OnWeaponSelected(int slotIndex)
		{
			if (slotIndex == 0)
			{
				_playerInputs.IsToReload = true;
			}
			else
			{
				_playerInputs.SwitchToWeapon = slotIndex - 1;
			}
		}

		private void OnGrenadeSelect(int slotIndex)
		{
		}

		private void OnGrenadeSelected(int slotIndex)
		{
		}

		public void SetAimButtonActive(bool isActive)
		{
			_aimButton.gameObject.SetActive(isActive);
			_isAimButtonActive = isActive;
		}

		private void OnAimClicked()
		{
			_playerInputs.IsToAim = true;
		}

		private void OnJumpDoubleTouch()
		{
			_playerInputs.IsToJump = true;
		}

		public PlayerInputs GetPlayerInputs()
		{
			return _playerInputs;
		}

		public void HandleTouches()
		{
			_playerInputs = new PlayerInputs();
			_touchHandler.Handle();
			_playerInputs.Horizontal = _joystick.joystickAxis.x;
			_playerInputs.Vertical = _joystick.joystickAxis.y;
			_playerInputs.DeltaAimAngles.x = _aimingArea.AimingAxis.y;
			_playerInputs.DeltaAimAngles.y = _aimingArea.AimingAxis.x;
			_playerInputs.IsToFire = _shootArea.IsShooting || _shootButton.IsButtonDown;
			_playerInputs.IsCrouching = _crouchButtonClinger.IsClicked;
		}

		public void SetButtonsVisible(bool isVisible)
		{
			if (isVisible != _buttonsVisible)
			{
				_buttonsVisible = isVisible;
				_shootButton.gameObject.SetActive(isVisible);
				if (_isAimButtonActive)
				{
					_aimButton.gameObject.SetActive(isVisible);
				}
				_crouchButton.gameObject.SetActive(isVisible);
			}
		}
	}
}
