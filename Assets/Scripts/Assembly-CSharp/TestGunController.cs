using Axlebolt.Standoff.Inventory.Drop;
using UnityEngine;

public class TestGunController : MonoBehaviour, IDropableObject
{
	public DropableObjectParameters throwParameters;

	public DropableObjectParameters GetDropableObjectParameters()
	{
		return throwParameters;
	}

	public DropableObejctIdentityData GetDropableObejctIdentityData()
	{
		return new DropableObejctIdentityData();
	}

	public void InitializeDropableObject()
	{
		base.transform.SetParent(null, true);
	}

	public InventoryAttachmentType GetAttachmentType()
	{
		return InventoryAttachmentType.PrimaryWeapon;
	}

	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	public DropableObjectConfig GetDropableObjectConfig()
	{
		return new DropableObjectConfig();
	}

	public void Drop(DropableObjectData dropableObjectData)
	{
	}
}
