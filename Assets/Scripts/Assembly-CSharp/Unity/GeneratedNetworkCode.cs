using System.Runtime.InteropServices;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Inventory.Drop;
using Axlebolt.Standoff.Player.Mecanim;
using UnityEngine.Networking;

namespace Unity
{
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public class GeneratedNetworkCode
	{
		public static void _WriteTransformTRS_None(NetworkWriter writer, TransformTRS value)
		{
			writer.Write(value.pos);
			writer.Write(value.rot);
			writer.Write(value.scale);
		}

		public static void _WriteDropableObejctIdentityData_None(NetworkWriter writer, DropableObejctIdentityData value)
		{
			writer.WritePackedUInt32(value.inventoryID);
			writer.WritePackedUInt32((uint)value.skinID);
			writer.WritePackedUInt32((uint)value.magazineAmmo);
			writer.WritePackedUInt32((uint)value.restAmmo);
		}

		public static TransformTRS _ReadTransformTRS_None(NetworkReader reader)
		{
			TransformTRS transformTRS = new TransformTRS();
			transformTRS.pos = reader.ReadVector3();
			transformTRS.rot = reader.ReadVector3();
			transformTRS.scale = reader.ReadVector3();
			return transformTRS;
		}

		public static DropableObejctIdentityData _ReadDropableObejctIdentityData_None(NetworkReader reader)
		{
			DropableObejctIdentityData dropableObejctIdentityData = new DropableObejctIdentityData();
			dropableObejctIdentityData.inventoryID = (byte)reader.ReadPackedUInt32();
			dropableObejctIdentityData.skinID = (short)reader.ReadPackedUInt32();
			dropableObejctIdentityData.magazineAmmo = (short)reader.ReadPackedUInt32();
			dropableObejctIdentityData.restAmmo = (short)reader.ReadPackedUInt32();
			return dropableObejctIdentityData;
		}

		public static void _WriteMecanimTransitionInfo_None(NetworkWriter writer, MecanimTransitionInfo value)
		{
			writer.Write(value.name);
			writer.Write(value.duration);
			writer.Write(value.offset);
		}

		public static void _WriteArrayMecanimTransitionInfo_None(NetworkWriter writer, MecanimTransitionInfo[] value)
		{
			if (value == null)
			{
				writer.Write((ushort)0);
				return;
			}
			ushort value2 = (ushort)value.Length;
			writer.Write(value2);
			for (ushort num = 0; num < value.Length; num++)
			{
				_WriteMecanimTransitionInfo_None(writer, value[num]);
			}
		}

		public static MecanimTransitionInfo _ReadMecanimTransitionInfo_None(NetworkReader reader)
		{
			MecanimTransitionInfo mecanimTransitionInfo = new MecanimTransitionInfo();
			mecanimTransitionInfo.name = reader.ReadString();
			mecanimTransitionInfo.duration = reader.ReadSingle();
			mecanimTransitionInfo.offset = reader.ReadSingle();
			return mecanimTransitionInfo;
		}

		public static MecanimTransitionInfo[] _ReadArrayMecanimTransitionInfo_None(NetworkReader reader)
		{
			int num = reader.ReadUInt16();
			if (num == 0)
			{
				return new MecanimTransitionInfo[0];
			}
			MecanimTransitionInfo[] array = new MecanimTransitionInfo[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = _ReadMecanimTransitionInfo_None(reader);
			}
			return array;
		}
	}
}
