using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Axlebolt.RpcSupport.Protobuf;
using ProtoBuf;

namespace Axlebolt.RpcSupport
{
	public class ProtoReflectionUtils
	{
		private ProtoReflectionUtils()
		{
		}

		public static byte[] ToBytes(object obj)
		{
			if (obj == null)
			{
				return new byte[0];
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Serializer.Serialize((Stream)memoryStream, obj);
				return memoryStream.ToArray();
			}
		}

		public static T FromBytes<T>(byte[] data)
		{
			return (T)FromBytes(data, typeof(T));
		}

		public static object FromBytes(byte[] data, Type type)
		{
			if (data == null)
			{
				data = new byte[0];
			}
			using (MemoryStream source = new MemoryStream(data, false))
			{
				return Serializer.NonGeneric.Deserialize(type, source);
			}
		}

		public static RpcMessage.BinaryValue ToBinaryValue(object o)
		{
			RpcMessage.BinaryValue binaryValue = new RpcMessage.BinaryValue();
			if (o == null)
			{
				binaryValue.IsNull = true;
				return binaryValue;
			}
			o = ToProtoValue(o);
			if (o.GetType().IsArray)
			{
				binaryValue.Array = new List<byte[]>();
				binaryValue.IsNull = false;
				IEnumerable enumerable = o as IEnumerable;
				if (enumerable != null)
				{
					foreach (object item in enumerable)
					{
						binaryValue.Array.Add(ToBytes(item));
					}
				}
				return binaryValue;
			}
			binaryValue.One = ToBytes(o);
			binaryValue.IsNull = false;
			return binaryValue;
		}

		public static object FromBinaryValue(RpcMessage.BinaryValue binaryValue, Type type)
		{
			Type type2 = ToProtoType(type);
			if (binaryValue == null || binaryValue.IsNull)
			{
				return null;
			}
			if (type2.IsArray)
			{
				Type elementType = type2.GetElementType();
				Array array = Array.CreateInstance(elementType, binaryValue.Array.Count);
				for (int i = 0; i < array.Length; i++)
				{
					object value = ToOrigValue(FromBytes(binaryValue.Array[i], elementType), type);
					array.SetValue(value, i);
				}
				return array;
			}
			return ToOrigValue(FromBytes(binaryValue.One, type2), type);
		}

		public static bool IsSupportedType(Type type)
		{
			if (type.IsArray)
			{
				type = type.GetElementType();
			}
			if (type == typeof(void))
			{
				return true;
			}
			if (type.IsEnum)
			{
				if (!Attribute.IsDefined(type, typeof(ProtoContractAttribute)))
				{
					return false;
				}
				FieldInfo[] fields = type.GetFields();
				foreach (FieldInfo fieldInfo in fields)
				{
					if (!fieldInfo.Name.Equals("value__") && !Attribute.IsDefined(fieldInfo, typeof(ProtoEnumAttribute)))
					{
						return false;
					}
				}
			}
			if (type == typeof(int) || type == typeof(int?))
			{
				return true;
			}
			if (type.IsArray && type.GetElementType() == typeof(int))
			{
				return true;
			}
			if (type == typeof(float) || type == typeof(float?))
			{
				return true;
			}
			if (type.IsArray && type.GetElementType() == typeof(float))
			{
				return true;
			}
			if (type == typeof(long) || type == typeof(long?))
			{
				return true;
			}
			if (type.IsArray && type.GetElementType() == typeof(long))
			{
				return true;
			}
			if (type == typeof(string))
			{
				return true;
			}
			if (type.IsArray && type.GetElementType() == typeof(string))
			{
				return true;
			}
			if (type == typeof(double) || type == typeof(double?))
			{
				return true;
			}
			if (type.IsArray && type.GetElementType() == typeof(double))
			{
				return true;
			}
			if (type == typeof(bool) || type == typeof(bool?))
			{
				return true;
			}
			if (type.IsArray && type.GetElementType() == typeof(bool))
			{
				return true;
			}
			if (type == typeof(byte) || type == typeof(byte?))
			{
				return true;
			}
			if (type.IsArray && type.GetElementType() == typeof(byte))
			{
				return true;
			}
			return Attribute.IsDefined(type, typeof(ProtoContractAttribute));
		}

		private static object ToProtoValue(object o)
		{
			if (o.GetType().IsArray && o.GetType().GetElementType().IsEnum)
			{
				return o;
			}
			if (o is int)
			{
				RpcMessage.Integer integer = new RpcMessage.Integer();
				integer.Value = (int)o;
				return integer;
			}
			if (o is int?)
			{
				RpcMessage.Integer integer2 = new RpcMessage.Integer();
				integer2.Value = ((int?)o).Value;
				return integer2;
			}
			if (o is int[])
			{
				RpcMessage.IntegerArray integerArray = new RpcMessage.IntegerArray();
				integerArray.Value = new List<int>();
				integerArray.Value.AddRange((int[])o);
				return integerArray;
			}
			if (o is float)
			{
				RpcMessage.Float @float = new RpcMessage.Float();
				@float.Value = (float)o;
				return @float;
			}
			if (o is float?)
			{
				RpcMessage.Float float2 = new RpcMessage.Float();
				float2.Value = ((float?)o).Value;
				return float2;
			}
			if (o is float[])
			{
				RpcMessage.FloatArray floatArray = new RpcMessage.FloatArray();
				floatArray.Value = new List<float>();
				floatArray.Value.AddRange((float[])o);
				return floatArray;
			}
			if (o is long)
			{
				RpcMessage.Long @long = new RpcMessage.Long();
				@long.Value = (long)o;
				return @long;
			}
			if (o is long?)
			{
				RpcMessage.Long long2 = new RpcMessage.Long();
				long2.Value = ((long?)o).Value;
				return long2;
			}
			if (o is long[])
			{
				RpcMessage.LongArray longArray = new RpcMessage.LongArray();
				longArray.Value = new List<long>();
				longArray.Value.AddRange((long[])o);
				return longArray;
			}
			if (o is string)
			{
				RpcMessage.String @string = new RpcMessage.String();
				@string.Value = (string)o;
				return @string;
			}
			if (o is string[])
			{
				RpcMessage.StringArray stringArray = new RpcMessage.StringArray();
				stringArray.Value = new List<string>();
				stringArray.Value.AddRange((string[])o);
				return stringArray;
			}
			if (o is double)
			{
				RpcMessage.Double @double = new RpcMessage.Double();
				@double.Value = (double)o;
				return @double;
			}
			if (o is double?)
			{
				RpcMessage.Double double2 = new RpcMessage.Double();
				double2.Value = ((double?)o).Value;
				return double2;
			}
			if (o is double[])
			{
				RpcMessage.DoubleArray doubleArray = new RpcMessage.DoubleArray();
				doubleArray.Value = new List<double>();
				doubleArray.Value.AddRange((double[])o);
				return doubleArray;
			}
			if (o is bool)
			{
				RpcMessage.Boolean boolean = new RpcMessage.Boolean();
				boolean.Value = (bool)o;
				return boolean;
			}
			if (o is bool?)
			{
				RpcMessage.Boolean boolean2 = new RpcMessage.Boolean();
				boolean2.Value = ((bool?)o).Value;
				return boolean2;
			}
			if (o is bool[])
			{
				RpcMessage.BooleanArray booleanArray = new RpcMessage.BooleanArray();
				booleanArray.Value = new List<bool>();
				booleanArray.Value.AddRange((bool[])o);
				return booleanArray;
			}
			if (o is byte)
			{
				RpcMessage.Byte @byte = new RpcMessage.Byte();
				@byte.Value = new byte[1] { (byte)o };
				return @byte;
			}
			if (o is byte?)
			{
				RpcMessage.Byte byte2 = new RpcMessage.Byte();
				byte2.Value = new byte[1] { ((byte?)o).Value };
				return byte2;
			}
			if (o is byte[])
			{
				RpcMessage.Byte byte3 = new RpcMessage.Byte();
				byte3.Value = (byte[])o;
				return byte3;
			}
			if (o is Enum)
			{
				FieldInfo field = o.GetType().GetField(o.ToString());
				ProtoEnumAttribute protoEnumAttribute = (ProtoEnumAttribute)Attribute.GetCustomAttribute(field, typeof(ProtoEnumAttribute));
				RpcMessage.Enum @enum = new RpcMessage.Enum();
				@enum.Value = protoEnumAttribute.Value;
				return @enum;
			}
			return o;
		}

		private static object ToOrigValue(object protoValue, Type origType)
		{
			if (protoValue is RpcMessage.Integer)
			{
				return ((RpcMessage.Integer)protoValue).Value;
			}
			if (protoValue is RpcMessage.IntegerArray)
			{
				return ((RpcMessage.IntegerArray)protoValue).Value.ToArray();
			}
			if (protoValue is RpcMessage.Float)
			{
				return ((RpcMessage.Float)protoValue).Value;
			}
			if (protoValue is RpcMessage.FloatArray)
			{
				return ((RpcMessage.FloatArray)protoValue).Value.ToArray();
			}
			if (protoValue is RpcMessage.Long)
			{
				return ((RpcMessage.Long)protoValue).Value;
			}
			if (protoValue is RpcMessage.LongArray)
			{
				return ((RpcMessage.LongArray)protoValue).Value.ToArray();
			}
			if (protoValue is RpcMessage.String)
			{
				return ((RpcMessage.String)protoValue).Value;
			}
			if (protoValue is RpcMessage.StringArray)
			{
				return ((RpcMessage.StringArray)protoValue).Value.ToArray();
			}
			if (protoValue is RpcMessage.Double)
			{
				return ((RpcMessage.Double)protoValue).Value;
			}
			if (protoValue is RpcMessage.DoubleArray)
			{
				return ((RpcMessage.DoubleArray)protoValue).Value.ToArray();
			}
			if (protoValue is RpcMessage.Boolean)
			{
				return ((RpcMessage.Boolean)protoValue).Value;
			}
			if (protoValue is RpcMessage.BooleanArray)
			{
				return ((RpcMessage.BooleanArray)protoValue).Value.ToArray();
			}
			if (protoValue is RpcMessage.Byte)
			{
				return ((RpcMessage.Byte)protoValue).Value[0];
			}
			if (protoValue is RpcMessage.ByteArray)
			{
				return ((RpcMessage.ByteArray)protoValue).Value;
			}
			if (protoValue is RpcMessage.Enum)
			{
				int value = ((RpcMessage.Enum)protoValue).Value;
				FieldInfo[] fields = origType.GetFields();
				foreach (FieldInfo fieldInfo in fields)
				{
					ProtoEnumAttribute protoEnumAttribute = (ProtoEnumAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(ProtoEnumAttribute));
					if (protoEnumAttribute != null && value == protoEnumAttribute.Value)
					{
						return Enum.Parse(origType, fieldInfo.Name);
					}
				}
			}
			return protoValue;
		}

		private static Type ToProtoType(Type origType)
		{
			if (origType == typeof(int) || origType == typeof(int?))
			{
				return typeof(RpcMessage.Integer);
			}
			if (origType.IsArray && origType.GetElementType() == typeof(int))
			{
				return typeof(RpcMessage.IntegerArray);
			}
			if (origType == typeof(float) || origType == typeof(float?))
			{
				return typeof(RpcMessage.Float);
			}
			if (origType.IsArray && origType.GetElementType() == typeof(float))
			{
				return typeof(RpcMessage.FloatArray);
			}
			if (origType == typeof(long) || origType == typeof(long?))
			{
				return typeof(RpcMessage.Long);
			}
			if (origType.IsArray && origType.GetElementType() == typeof(long))
			{
				return typeof(RpcMessage.LongArray);
			}
			if (origType == typeof(string))
			{
				return typeof(RpcMessage.String);
			}
			if (origType.IsArray && origType.GetElementType() == typeof(string))
			{
				return typeof(RpcMessage.StringArray);
			}
			if (origType == typeof(double) || origType == typeof(double?))
			{
				return typeof(RpcMessage.Double);
			}
			if (origType.IsArray && origType.GetElementType() == typeof(double))
			{
				return typeof(RpcMessage.DoubleArray);
			}
			if (origType == typeof(bool) || origType == typeof(bool?))
			{
				return typeof(RpcMessage.Boolean);
			}
			if (origType.IsArray && origType.GetElementType() == typeof(bool))
			{
				return typeof(RpcMessage.BooleanArray);
			}
			if (origType == typeof(byte) || origType == typeof(byte?))
			{
				return typeof(RpcMessage.Byte);
			}
			if (origType.IsArray && origType.GetElementType() == typeof(byte))
			{
				return typeof(RpcMessage.ByteArray);
			}
			if (origType.IsEnum)
			{
				return typeof(RpcMessage.Enum);
			}
			return origType;
		}
	}
}
