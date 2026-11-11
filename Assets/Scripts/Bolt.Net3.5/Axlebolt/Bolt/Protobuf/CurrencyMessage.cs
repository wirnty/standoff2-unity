using System.Collections.Generic;
using ProtoBuf;

namespace Axlebolt.Bolt.Protobuf
{
	public class CurrencyMessage
	{
		[ProtoContract]
		public class Currency
		{
			[ProtoMember(1)]
			public int Id { get; set; }

			[ProtoMember(2)]
			public float ExchangeRatio { get; set; }

			[ProtoMember(3)]
			public List<int> ExchangableCurrencies { get; set; }
		}

		[ProtoContract]
		public class CurrencyAmount
		{
			[ProtoMember(1)]
			public int CurrencyId { get; set; }

			[ProtoMember(2)]
			public int Value { get; set; }
		}
	}
}
