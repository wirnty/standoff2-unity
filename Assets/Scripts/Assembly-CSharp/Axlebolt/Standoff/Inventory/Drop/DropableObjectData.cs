using Axlebolt.Standoff.Common;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Inventory.Drop
{
	public class DropableObjectData : MessageBase
	{
		public TransformTRS transformData = new TransformTRS();

		public Vector3 velocity;

		public Vector3 angularVelocity;

		public DropableObejctIdentityData identityData = new DropableObejctIdentityData();

		public override void Serialize(NetworkWriter writer)
		{
			GeneratedNetworkCode._WriteTransformTRS_None(writer, transformData);
			writer.Write(velocity);
			writer.Write(angularVelocity);
			GeneratedNetworkCode._WriteDropableObejctIdentityData_None(writer, identityData);
		}

		public override void Deserialize(NetworkReader reader)
		{
			transformData = GeneratedNetworkCode._ReadTransformTRS_None(reader);
			velocity = reader.ReadVector3();
			angularVelocity = reader.ReadVector3();
			identityData = GeneratedNetworkCode._ReadDropableObejctIdentityData_None(reader);
		}
	}
}
