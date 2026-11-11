using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Scripts.Controls;
using Axlebolt.Standoff.Game.UI.HUD;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.TouchControl;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class Controls : MonoBehaviour
	{
		public enum InputSourceType
		{
			Keyboard = 0,
			TouchScreen = 1,
			InputSimulator = 2
		}

		[SerializeField]
		private InputSourceType _sourceType = InputSourceType.TouchScreen;

		private KeyboardControl _keyboardControl;

		private TouchController _touchController;

		private HUDView _hudView;

		private PlayerController _playerController;

		private RandomInputsSimulator _inputsSimulator;

		private readonly HashSet<Object> _disableRequests = new HashSet<Object>();

		public static Controls Instance { get; private set; }

		public InputSourceType SourceType
		{
			[CompilerGenerated]
			get
			{
				return _sourceType;
			}
		}

		public PlayerController PlayerController
		{
			get
			{
				return _playerController;
			}
			set
			{
				_playerController = value;
				_hudView.PlayerController = _playerController;
				if (_sourceType == InputSourceType.TouchScreen)
				{
					_touchController.SetButtonsVisible(PlayerController != null);
				}
			}
		}

		private void Awake()
		{
			Instance = this;
			if (Application.isMobilePlatform)
			{
				_sourceType = InputSourceType.TouchScreen;
			}
			_keyboardControl = base.gameObject.AddComponent<KeyboardControl>();
			_touchController = this.GetRequireComponent<TouchController>();
			_touchController.Initialize();
			_hudView = this.GetRequireComponent<HUDView>();
			_hudView.Initialize(_touchController);
			_inputsSimulator = GetComponent<RandomInputsSimulator>();
		}

		public void RequestDisable(Object owner)
		{
			_disableRequests.Add(owner);
		}

		public void RequestEnable(Object owner)
		{
			_disableRequests.Remove(owner);
		}

		private void Update()
		{
			if (PlayerController == null)
			{
				return;
			}
			PlayerInputs inputs = GetInputs();
			if (PlayerController.InventoryController.CurrentWeapon != null && PlayerController.InventoryController.CurrentWeapon is GunController)
			{
				GunController gunController = (GunController)PlayerController.InventoryController.CurrentWeapon;
				if (gunController.GunParameters.SightType == SightType.CollimatorSight || gunController.GunParameters.SightType == SightType.SniperScope)
				{
					if (gunController.CurrentAimingMode == GunController.AimingMode.Aiming)
					{
						inputs.DeltaAimAngles *= gunController.GunParameters.ScopeAimSensitivityMult;
					}
					if (_sourceType == InputSourceType.TouchScreen)
					{
						_touchController.SetAimButtonActive(true);
					}
				}
				else if (_sourceType == InputSourceType.TouchScreen)
				{
					_touchController.SetAimButtonActive(false);
				}
			}
			PlayerController.SetInputs(inputs, Time.deltaTime);
		}

		private PlayerInputs GetInputs()
		{
			if (IsDisabled())
			{
				return new PlayerInputs();
			}
			if (_sourceType == InputSourceType.TouchScreen)
			{
				_touchController.HandleTouches();
			}
			if (_sourceType == InputSourceType.TouchScreen)
			{
				return _touchController.GetPlayerInputs();
			}
			if (_sourceType == InputSourceType.InputSimulator && _inputsSimulator != null)
			{
				return _inputsSimulator.GetPlayerInputs();
			}
			return _keyboardControl.GetPlayerInputs();
		}

		private bool IsDisabled()
		{
			return _disableRequests.Count > 0;
		}
	}
}
