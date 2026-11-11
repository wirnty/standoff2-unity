using System;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	[Serializable]
	public class InventorySprites
	{
		[SerializeField]
		private Sprite _icon;

		[SerializeField]
		private Sprite _line;

		[SerializeField]
		private Sprite _glow;

		[SerializeField]
		private Sprite _preview;

		public Sprite Glow
		{
			get
			{
				return _glow;
			}
		}

		public Sprite Icon
		{
			get
			{
				return _icon;
			}
		}

		public Sprite Line
		{
			get
			{
				return _line;
			}
		}

		public Sprite Preview
		{
			get
			{
				return _preview;
			}
		}
	}
}
