using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Inventory
{
	public class InventoryControllerCmd : MessageBase
	{
		public bool toFire = false;

		public bool toAim = false;

		public bool toReload = false;

		public int slotIndex = -1;

		public bool toThrow = false;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(toFire);
			writer.Write(toAim);
			writer.Write(toReload);
			writer.WritePackedUInt32((uint)slotIndex);
			writer.Write(toThrow);
		}

		public override void Deserialize(NetworkReader reader)
		{
			toFire = reader.ReadBoolean();
			toAim = reader.ReadBoolean();
			toReload = reader.ReadBoolean();
			slotIndex = (int)reader.ReadPackedUInt32();
			toThrow = reader.ReadBoolean();
		}
	}
}
