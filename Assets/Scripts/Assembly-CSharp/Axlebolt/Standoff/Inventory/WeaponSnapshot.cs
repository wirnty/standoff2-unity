using UnityEngine.Networking;

namespace Axlebolt.Standoff.Inventory
{
	public class WeaponSnapshot : MessageBase
	{
		public float time;

		public virtual byte GetDataType()
		{
			return 0;
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(time);
		}

		public override void Deserialize(NetworkReader reader)
		{
			time = reader.ReadSingle();
		}
	}
}
