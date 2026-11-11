using System;
using Axlebolt.Common.States;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Player;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	public class CameraScopeZoomer : MonoBehaviour
	{
		public enum States
		{
			Zoomed = 0,
			Zooming = 1,
			NotZoomed = 2
		}

		private readonly StateSimple<States> _zoomState = new StateSimple<States>();

		private PlayerController _playerController;

		private Camera _camera;

		private const float ZoomedFov = 20f;

		public PlayerController PlayerController
		{
			get
			{
				return _playerController;
			}
			set
			{
				_playerController = value;
				SetNotZoomed();
			}
		}

		private void Awake()
		{
			_camera = GetComponent<Camera>();
			SetNotZoomed();
		}

		public void SetNotZoomed()
		{
			if (Mathf.Abs(_camera.fieldOfView - 60f) > 0.001f)
			{
				_camera.fieldOfView = 60f;
			}
			_zoomState.SetState(States.NotZoomed, Time.time);
		}

		private void SetZooming()
		{
			if (Math.Abs(_camera.fieldOfView - 20f) > 0.01f)
			{
				_camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, 20f, Time.deltaTime * 20f);
			}
		}

		private void Update()
		{
			if (PlayerController != null && PlayerController.InventoryController.CurrentWeapon != null && PlayerController.InventoryController.CurrentWeapon is GunController)
			{
				GunController gunController = (GunController)PlayerController.InventoryController.CurrentWeapon;
				if (gunController.GunParameters.SightType == SightType.SniperScope && gunController.CurrentAimingMode == GunController.AimingMode.Aiming)
				{
					SetZooming();
					return;
				}
			}
			SetNotZoomed();
		}
	}
}
