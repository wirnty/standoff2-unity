using System.Collections.Generic;
using ProtoBuf;

namespace Axlebolt.Bolt.Protobuf
{
	public class InventoryMessage
	{
		[ProtoContract]
		public class PlayerInventory
		{
			[ProtoMember(1)]
			public List<CurrencyMessage.CurrencyAmount> Currencies { get; set; }

			[ProtoMember(2)]
			public List<InventoryItemStack> InventoryItems { get; set; }
		}

		[ProtoContract]
		public class InventoryItemStack
		{
			[ProtoMember(1)]
			public int StackId { get; set; }

			[ProtoMember(2)]
			public InventoryItem InventoryItem { get; set; }

			[ProtoMember(3)]
			public int Quantity { get; set; }

			[ProtoMember(4)]
			public int Flags { get; set; }
		}

		[ProtoContract]
		public class InventoryItem
		{
			[ProtoMember(1)]
			public int Id { get; set; }

			[ProtoMember(2)]
			public string DisplayName { get; set; }

			[ProtoMember(3)]
			public Dictionary<string, string> Properties { get; set; }

			[ProtoMember(4)]
			public List<CurrencyMessage.CurrencyAmount> BuyPrice { get; set; }

			[ProtoMember(5)]
			public List<CurrencyMessage.CurrencyAmount> SellPrice { get; set; }
		}
	}
}
