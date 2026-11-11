using System.Collections.Generic;
using ProtoBuf;

namespace Axlebolt.RpcSupport.Protobuf
{
	public class RpcMessage
	{
		[ProtoContract]
		public class RpcRequest
		{
			[ProtoMember(1)]
			public string Id { get; set; }

			[ProtoMember(2)]
			public string ServiceName { get; set; }

			[ProtoMember(3)]
			public string MethodName { get; set; }

			[ProtoMember(4)]
			public List<BinaryValue> Params { get; set; }
		}

		[ProtoContract]
		public class ResponseMessage
		{
			[ProtoMember(1)]
			public RpcResponse RpcResponse { get; set; }

			[ProtoMember(2)]
			public EventResponse EventResponse { get; set; }
		}

		[ProtoContract]
		public class RpcResponse
		{
			[ProtoMember(1)]
			public string Id { get; set; }

			[ProtoMember(2)]
			public Exception Exception { get; set; }

			[ProtoMember(3)]
			public BinaryValue Return { get; set; }
		}

		[ProtoContract]
		public class Exception
		{
			[ProtoMember(1)]
			public long Id { get; set; }

			[ProtoMember(2)]
			public int Code { get; set; }
		}

		[ProtoContract]
		public class EventResponse
		{
			[ProtoMember(1)]
			public string ListenerName { get; set; }

			[ProtoMember(2)]
			public string EventName { get; set; }

			[ProtoMember(3)]
			public List<BinaryValue> Params { get; set; }
		}

		[ProtoContract]
		public class BinaryValue
		{
			[ProtoMember(1)]
			public bool IsNull { get; set; }

			[ProtoMember(2)]
			public List<byte[]> Array { get; set; }

			[ProtoMember(3)]
			public byte[] One { get; set; }
		}

		[ProtoContract]
		public class String
		{
			[ProtoMember(1)]
			public string Value { get; set; }
		}

		[ProtoContract]
		public class StringArray
		{
			[ProtoMember(1)]
			public List<string> Value { get; set; }
		}

		[ProtoContract]
		public class Integer
		{
			[ProtoMember(1)]
			public int Value { get; set; }
		}

		[ProtoContract]
		public class IntegerArray
		{
			[ProtoMember(1)]
			public List<int> Value { get; set; }
		}

		[ProtoContract]
		public class Long
		{
			[ProtoMember(1)]
			public long Value { get; set; }
		}

		[ProtoContract]
		public class LongArray
		{
			[ProtoMember(1)]
			public List<long> Value { get; set; }
		}

		[ProtoContract]
		public class Double
		{
			[ProtoMember(1)]
			public double Value { get; set; }
		}

		[ProtoContract]
		public class DoubleArray
		{
			[ProtoMember(1)]
			public List<double> Value { get; set; }
		}

		[ProtoContract]
		public class Float
		{
			[ProtoMember(1)]
			public float Value { get; set; }
		}

		[ProtoContract]
		public class FloatArray
		{
			[ProtoMember(1)]
			public List<float> Value { get; set; }
		}

		[ProtoContract]
		public class Boolean
		{
			[ProtoMember(1)]
			public bool Value { get; set; }
		}

		[ProtoContract]
		public class BooleanArray
		{
			[ProtoMember(1)]
			public List<bool> Value { get; set; }
		}

		[ProtoContract]
		public class Byte
		{
			[ProtoMember(1)]
			public byte[] Value { get; set; }
		}

		[ProtoContract]
		public class ByteArray
		{
			[ProtoMember(1)]
			public byte[] Value { get; set; }
		}

		[ProtoContract]
		public class Enum
		{
			[ProtoMember(1)]
			public int Value { get; set; }
		}
	}
}
