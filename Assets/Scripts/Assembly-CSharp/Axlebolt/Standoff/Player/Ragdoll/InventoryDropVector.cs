using System;
using Axlebolt.Standoff.Inventory.Drop;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Ragdoll
{
	[Serializable]
	public class InventoryDropVector
	{
		public Vector3 throwVector;

		public float throwSpeed;

		public Vector3 angularVelocityAxis;

		public float angularSpeed;

		public InventoryAttachmentType attachmentType;
	}
}
