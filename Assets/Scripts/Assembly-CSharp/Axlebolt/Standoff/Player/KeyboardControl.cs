using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class KeyboardControl : MonoBehaviour
	{
		private float sensitivityX = 5f;

		private float sensitivityY = 5f;

		public PlayerInputs GetPlayerInputs()
		{
			PlayerInputs playerInputs = new PlayerInputs();
			playerInputs.SwitchToWeapon = -1;
			playerInputs.SwitchToWeapon = ((!Input.GetKeyDown("1")) ? playerInputs.SwitchToWeapon : 0);
			playerInputs.SwitchToWeapon = (Input.GetKeyDown("2") ? 1 : playerInputs.SwitchToWeapon);
			playerInputs.SwitchToWeapon = ((!Input.GetKeyDown("3")) ? playerInputs.SwitchToWeapon : 2);
			playerInputs.SwitchToWeapon = ((!Input.GetKeyDown("4")) ? playerInputs.SwitchToWeapon : 3);
			playerInputs.SwitchToWeapon = ((!Input.GetKeyDown("5")) ? playerInputs.SwitchToWeapon : 4);
			playerInputs.IsToReload = false;
			playerInputs.IsToReload = Input.GetKeyDown("r");
			playerInputs.IsToFire = Input.GetMouseButton(0);
			playerInputs.IsToAim = Input.GetMouseButtonDown(1);
			playerInputs.Vertical = Input.GetAxis("Vertical");
			playerInputs.Horizontal = Input.GetAxis("Horizontal");
			playerInputs.IsCrouching = Input.GetKey(KeyCode.LeftControl);
			playerInputs.IsToJump = Input.GetKeyDown(KeyCode.Space);
			playerInputs.DeltaAimAngles.y = Input.GetAxis("Mouse X") * sensitivityY;
			playerInputs.DeltaAimAngles.x = Input.GetAxis("Mouse Y") * sensitivityX;
			return playerInputs;
		}
	}
}
