using System.Collections.Generic;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Inventory.Weapon
{
	public class GunSnapshot : WeaponSnapshot
	{
		public List<ShotData> shootDataList = new List<ShotData>();

		public short magazineAmmo;

		public short restAmmo;

		public override byte GetDataType()
		{
			return 0;
		}

		public override void Deserialize(NetworkReader reader)
		{
			short num = reader.ReadInt16();
			shootDataList.Clear();
			for (int i = 0; i < num; i++)
			{
				ShotData shotData = new ShotData();
				shotData.Deserialize(reader);
				shootDataList.Add(shotData);
			}
			magazineAmmo = reader.ReadInt16();
			restAmmo = reader.ReadInt16();
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write((short)shootDataList.Count);
			for (int i = 0; i < shootDataList.Count; i++)
			{
				writer.Write(shootDataList[i]);
			}
			writer.Write(magazineAmmo);
			writer.Write(restAmmo);
		}
	}
}
