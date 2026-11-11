using System.Collections.Generic;
using Axlebolt.Networking;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Weapon;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Inventory
{
	public class InventorySnapshot : ObjectSnapshot
	{
		public class SlotItem : MessageBase
		{
			public byte slotIndex;

			public byte inventoryId;

			public byte inventoryInstanceId;

			public override void Deserialize(NetworkReader reader)
			{
				slotIndex = reader.ReadByte();
				inventoryId = reader.ReadByte();
				inventoryInstanceId = reader.ReadByte();
			}

			public override void Serialize(NetworkWriter writer)
			{
				writer.Write(slotIndex);
				writer.Write(inventoryId);
				writer.Write(inventoryInstanceId);
			}
		}

		public int weaponCount;

		public byte currentSlotIndex;

		public InventoryDataType inventoryDataType = InventoryDataType.Gun;

		public List<SlotItem> slot = new List<SlotItem>();

		public WeaponSnapshot weaponSnapshot = new WeaponSnapshot();

		public static WeaponSnapshot GetInventoryDataInstance(InventoryDataType dataType)
		{
			if (dataType == InventoryDataType.Gun)
			{
				return new GunSnapshot();
			}
			Debug.LogError("Inventory _parameters Imp. not found. Parameters type : " + dataType);
			return null;
		}

		public override void Deserialize(NetworkReader reader)
		{
			weaponCount = reader.ReadByte();
			currentSlotIndex = reader.ReadByte();
			inventoryDataType = (InventoryDataType)reader.ReadByte();
			slot.Clear();
			for (int i = 0; i < weaponCount; i++)
			{
				SlotItem slotItem = new SlotItem();
				slotItem.Deserialize(reader);
				slot.Add(slotItem);
			}
			weaponSnapshot = GetInventoryDataInstance(inventoryDataType);
			weaponSnapshot.Deserialize(reader);
		}

		public override void Serialize(NetworkWriter writer)
		{
			weaponCount = slot.Count;
			writer.Write((byte)weaponCount);
			writer.Write(currentSlotIndex);
			inventoryDataType = (InventoryDataType)weaponSnapshot.GetDataType();
			writer.Write((byte)inventoryDataType);
			for (int i = 0; i < weaponCount; i++)
			{
				slot[i].Serialize(writer);
			}
			weaponSnapshot.Serialize(writer);
		}
	}
}
