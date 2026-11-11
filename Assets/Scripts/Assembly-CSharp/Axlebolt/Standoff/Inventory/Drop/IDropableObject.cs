using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Drop
{
	public interface IDropableObject
	{
		DropableObjectParameters GetDropableObjectParameters();

		DropableObejctIdentityData GetDropableObejctIdentityData();

		InventoryAttachmentType GetAttachmentType();

		void InitializeDropableObject();

		GameObject GetGameObject();

		DropableObjectConfig GetDropableObjectConfig();

		void Drop(DropableObjectData dropData);
	}
}
