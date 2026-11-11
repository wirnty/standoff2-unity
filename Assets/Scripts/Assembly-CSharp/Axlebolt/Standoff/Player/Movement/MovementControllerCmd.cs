using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Movement
{
	public class MovementControllerCmd : MessageBase
	{
		public float horizontal = 0f;

		public float vertical = 0f;

		public bool isCrouching = false;

		public bool isToJump = false;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(horizontal);
			writer.Write(vertical);
			writer.Write(isCrouching);
			writer.Write(isToJump);
		}

		public override void Deserialize(NetworkReader reader)
		{
			horizontal = reader.ReadSingle();
			vertical = reader.ReadSingle();
			isCrouching = reader.ReadBoolean();
			isToJump = reader.ReadBoolean();
		}
	}
}
