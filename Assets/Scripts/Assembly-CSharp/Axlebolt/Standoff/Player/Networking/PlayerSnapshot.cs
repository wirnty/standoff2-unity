using Axlebolt.Networking;
using Axlebolt.Standoff.Player.Aim;
using Axlebolt.Standoff.Player.Inventory;
using Axlebolt.Standoff.Player.Mecanim;
using Axlebolt.Standoff.Player.Movement;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Networking
{
	public class PlayerSnapshot : ObjectSnapshot
	{
		public MovementSnapshot movementSnapshot = new MovementSnapshot();

		public MecanimSnapshot mecanimSnapshot = new MecanimSnapshot();

		public AimSnapshot aimingSnapshot = new AimSnapshot();

		public InventorySnapshot inventorySnapshot = new InventorySnapshot();

		public override void Deserialize(NetworkReader reader)
		{
			time = reader.ReadSingle();
			mecanimSnapshot.time = time;
			movementSnapshot.time = time;
			aimingSnapshot.time = time;
			inventorySnapshot.time = time;
			movementSnapshot.Deserialize(reader);
			mecanimSnapshot.Deserialize(reader);
			aimingSnapshot.Deserialize(reader);
			inventorySnapshot.Deserialize(reader);
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(time);
			movementSnapshot.Serialize(writer);
			mecanimSnapshot.Serialize(writer);
			aimingSnapshot.Serialize(writer);
			inventorySnapshot.Serialize(writer);
		}
	}
}
