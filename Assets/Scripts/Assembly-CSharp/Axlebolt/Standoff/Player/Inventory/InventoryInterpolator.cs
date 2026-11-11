using Axlebolt.Networking;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Inventory
{
	public class InventoryInterpolator : Interpolator
	{
		public override ObjectSnapshot Interpolate(ObjectSnapshot a, ObjectSnapshot b, float progress)
		{
			InventorySnapshot inventorySnapshot = (InventorySnapshot)a;
			InventorySnapshot inventorySnapshot2 = (InventorySnapshot)b;
			inventorySnapshot2.weaponSnapshot.time = Mathf.Lerp(inventorySnapshot.time, inventorySnapshot2.time, progress);
			return inventorySnapshot2;
		}
	}
}
