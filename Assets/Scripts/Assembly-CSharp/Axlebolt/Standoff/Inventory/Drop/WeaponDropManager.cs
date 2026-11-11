using Axlebolt.Standoff.Core;

namespace Axlebolt.Standoff.Inventory.Drop
{
	public class WeaponDropManager : Singleton<WeaponDropManager>
	{
		public void DropInventory(DropableObjectData dropableInventoryData)
		{
			WeaponController weaponController = Singleton<WeaponManager>.Instance.Get(dropableInventoryData.identityData.inventoryID);
			WeaponDropController dropController = weaponController.DropController;
			dropController.InitializeDropableObject();
			dropController.Drop(dropableInventoryData);
			dropController.gameObject.SetActive(true);
		}
	}
}
