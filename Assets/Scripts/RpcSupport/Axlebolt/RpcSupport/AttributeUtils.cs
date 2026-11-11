using System;
using System.Linq;
using System.Reflection;

namespace Axlebolt.RpcSupport
{
	public class AttributeUtils
	{
		public static T GetAttribute<T>(Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(T), true);
			if (customAttributes.Length > 0)
			{
				return (T)customAttributes[0];
			}
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type2 in interfaces)
			{
				T attribute = GetAttribute<T>(type2);
				if (Convert.ChangeType(attribute, typeof(T)) != null)
				{
					return attribute;
				}
			}
			return default(T);
		}

		public static T GetAttribute<T>(MethodInfo methodInfo)
		{
			object[] customAttributes = methodInfo.GetCustomAttributes(typeof(T), true);
			if (customAttributes.Length > 0)
			{
				return (T)customAttributes[0];
			}
			Type[] interfaces = methodInfo.ReflectedType.GetInterfaces();
			foreach (Type type in interfaces)
			{
				Type[] types = (from param in methodInfo.GetParameters()
					select param.ParameterType).ToArray();
				MethodInfo method = type.GetMethod(methodInfo.Name, types);
				if (method != null)
				{
					return GetAttribute<T>(method);
				}
			}
			return default(T);
		}
	}
}
