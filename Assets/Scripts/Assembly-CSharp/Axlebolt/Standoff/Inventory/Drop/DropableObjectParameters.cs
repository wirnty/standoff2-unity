using System;

namespace Axlebolt.Standoff.Inventory.Drop
{
	[Serializable]
	public class DropableObjectParameters
	{
		public float initialVelocityMult = 1f;

		public float inertialVelocityMult = 1f;

		public float impulseVelocityMult = 1f;
	}
}
