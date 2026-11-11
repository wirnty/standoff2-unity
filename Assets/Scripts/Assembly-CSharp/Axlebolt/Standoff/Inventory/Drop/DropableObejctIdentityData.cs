using UnityEngine.Networking;

namespace Axlebolt.Standoff.Inventory.Drop
{
	public class DropableObejctIdentityData : MessageBase
	{
		public byte inventoryID;

		public short skinID;

		public short magazineAmmo;

		public short restAmmo;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32(inventoryID);
			writer.WritePackedUInt32((uint)skinID);
			writer.WritePackedUInt32((uint)magazineAmmo);
			writer.WritePackedUInt32((uint)restAmmo);
		}

		public override void Deserialize(NetworkReader reader)
		{
			inventoryID = (byte)reader.ReadPackedUInt32();
			skinID = (short)reader.ReadPackedUInt32();
			magazineAmmo = (short)reader.ReadPackedUInt32();
			restAmmo = (short)reader.ReadPackedUInt32();
		}
	}
}
