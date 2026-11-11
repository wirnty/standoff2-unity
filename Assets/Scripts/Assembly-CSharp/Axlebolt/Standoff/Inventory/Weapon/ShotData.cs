using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Inventory.Weapon
{
	public class ShotData : MessageBase
	{
		public short shootNo;

		public float shootTime;

		public Vector3 direction;

		public Vector3 startPos;

		public override void Serialize(NetworkWriter writer)
		{
			writer.WritePackedUInt32((uint)shootNo);
			writer.Write(shootTime);
			writer.Write(direction);
			writer.Write(startPos);
		}

		public override void Deserialize(NetworkReader reader)
		{
			shootNo = (short)reader.ReadPackedUInt32();
			shootTime = reader.ReadSingle();
			direction = reader.ReadVector3();
			startPos = reader.ReadVector3();
		}
	}
}
