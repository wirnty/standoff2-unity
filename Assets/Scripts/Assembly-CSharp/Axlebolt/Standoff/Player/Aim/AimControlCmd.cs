using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Aim
{
	public class AimControlCmd : MessageBase
	{
		public AimType aimType;

		public Vector3 aimEulerAngles;

		public Vector2 deltaAimAngles;

		public override void Deserialize(NetworkReader reader)
		{
			aimType = (AimType)reader.ReadInt16();
			aimEulerAngles = reader.ReadVector3();
			deltaAimAngles = reader.ReadVector2();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write((short)aimType);
			writer.Write(aimEulerAngles);
			writer.Write(deltaAimAngles);
		}
	}
}
