using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using ProtoBuf.Meta;

namespace ProtoBuf.ServiceModel
{
	public sealed class XmlProtoSerializer : XmlObjectSerializer
	{
		private const string PROTO_ELEMENT = "proto";

		private readonly TypeModel model;

		private readonly int key;

		private readonly bool isList;

		private readonly bool isEnum;

		private readonly Type type;

		internal XmlProtoSerializer(TypeModel model, int key, Type type, bool isList)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model");
			}
			if (key < 0)
			{
				throw new ArgumentOutOfRangeException("key");
			}
			if (type == null)
			{
				throw new ArgumentOutOfRangeException("type");
			}
			this.model = model;
			this.key = key;
			this.isList = isList;
			this.type = type;
			isEnum = Helpers.IsEnum(type);
		}

		public static XmlProtoSerializer TryCreate(TypeModel model, Type type)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			bool flag;
			int num = GetKey(model, ref type, out flag);
			if (num >= 0)
			{
				return new XmlProtoSerializer(model, num, type, flag);
			}
			return null;
		}

		public XmlProtoSerializer(TypeModel model, Type type)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			key = GetKey(model, ref type, out isList);
			this.model = model;
			this.type = type;
			isEnum = Helpers.IsEnum(type);
			if (key < 0)
			{
				throw new ArgumentOutOfRangeException("type", "Type not recognised by the model: " + type.FullName);
			}
		}

		private static int GetKey(TypeModel model, ref Type type, out bool isList)
		{
			if (model != null && type != null)
			{
				int num = model.GetKey(ref type);
				if (num >= 0)
				{
					isList = false;
					return num;
				}
				Type listItemType = TypeModel.GetListItemType(model, type);
				if (listItemType != null)
				{
					num = model.GetKey(ref listItemType);
					if (num >= 0)
					{
						isList = true;
						return num;
					}
				}
			}
			isList = false;
			return -1;
		}

		public override void WriteEndObject(XmlDictionaryWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			writer.WriteEndElement();
		}

		public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			writer.WriteStartElement("proto");
		}

		public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (graph == null)
			{
				writer.WriteAttributeString("nil", "true");
				return;
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				if (isList)
				{
					model.Serialize(memoryStream, graph, null);
				}
				else
				{
					using (ProtoWriter dest = new ProtoWriter(memoryStream, model, null))
					{
						model.Serialize(key, graph, dest);
					}
				}
				byte[] buffer = memoryStream.GetBuffer();
				writer.WriteBase64(buffer, 0, (int)memoryStream.Length);
			}
		}

		public override bool IsStartObject(XmlDictionaryReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			reader.MoveToContent();
			if (reader.NodeType == XmlNodeType.Element)
			{
				return reader.Name == "proto";
			}
			return false;
		}

		public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			reader.MoveToContent();
			bool isEmptyElement = reader.IsEmptyElement;
			bool flag = reader.GetAttribute("nil") == "true";
			reader.ReadStartElement("proto");
			if (flag)
			{
				if (!isEmptyElement)
				{
					reader.ReadEndElement();
				}
				return null;
			}
			if (isEmptyElement)
			{
				if (isList || isEnum)
				{
					return model.Deserialize(Stream.Null, null, type, null);
				}
				ProtoReader protoReader = null;
				try
				{
					protoReader = ProtoReader.Create(Stream.Null, model, null, -1);
					return model.Deserialize(key, null, protoReader);
				}
				finally
				{
					ProtoReader.Recycle(protoReader);
				}
			}
			object result;
			using (MemoryStream source = new MemoryStream(reader.ReadContentAsBase64()))
			{
				if (isList || isEnum)
				{
					result = model.Deserialize(source, null, type, null);
				}
				else
				{
					ProtoReader protoReader2 = null;
					try
					{
						protoReader2 = ProtoReader.Create(source, model, null, -1);
						result = model.Deserialize(key, null, protoReader2);
					}
					finally
					{
						ProtoReader.Recycle(protoReader2);
					}
				}
			}
			reader.ReadEndElement();
			return result;
		}
	}
}
