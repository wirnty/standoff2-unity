using Axlebolt.Standoff.Player;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Animation
{
	public class WeaponOffsetHelperTool : MonoBehaviour
	{
		public static WeaponOffsetHelperTool Instance = null;

		[RequiredField]
		[Space(5f)]
		public PlayerController playerController;

		public bool isBoneVisualization = false;

		public void SaveCameraOffsetParameters()
		{
		}
	}
}
