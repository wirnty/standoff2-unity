using System;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	[Serializable]
	public class Ammunition
	{
		[SerializeField]
		private short _magazineCapacity;

		[SerializeField]
		private short _capacity;

		public short MagazineCapacity
		{
			get
			{
				return _magazineCapacity;
			}
		}

		public short Capacity
		{
			get
			{
				return _capacity;
			}
		}
	}
}
