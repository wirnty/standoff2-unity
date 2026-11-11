using Axlebolt.Standoff.Inventory;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player
{
	public class HitData : MessageBase
	{
		public Vector3 Point;

		public Vector3 Direction;

		public WeaponId WeaponId;

		public float Impulse;

		public int Damage;

		public int ArmorDamage;

		public BipedMap.Bip Bone;

		public bool Penetrated;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(Point);
			writer.Write(Direction);
			writer.Write((int)WeaponId);
			writer.Write(Impulse);
			writer.WritePackedUInt32((uint)Damage);
			writer.WritePackedUInt32((uint)ArmorDamage);
			writer.Write((int)Bone);
			writer.Write(Penetrated);
		}

		public override void Deserialize(NetworkReader reader)
		{
			Point = reader.ReadVector3();
			Direction = reader.ReadVector3();
			WeaponId = (WeaponId)reader.ReadInt32();
			Impulse = reader.ReadSingle();
			Damage = (int)reader.ReadPackedUInt32();
			ArmorDamage = (int)reader.ReadPackedUInt32();
			Bone = (BipedMap.Bip)reader.ReadInt32();
			Penetrated = reader.ReadBoolean();
		}
	}
}
