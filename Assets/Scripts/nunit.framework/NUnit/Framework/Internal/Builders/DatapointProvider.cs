using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Compatibility;
using NUnit.Framework.Interfaces;

namespace NUnit.Framework.Internal.Builders
{
	public class DatapointProvider : IParameterDataProvider
	{
		public bool HasDataFor(IParameterInfo parameter)
		{
			IMethodInfo method = parameter.Method;
			if (!method.IsDefined<TheoryAttribute>(true))
			{
				return false;
			}
			Type parameterType = parameter.ParameterType;
			if (parameterType == typeof(bool) || parameterType.GetTypeInfo().IsEnum)
			{
				return true;
			}
			MemberInfo[] members = method.TypeInfo.Type.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			foreach (MemberInfo memberInfo in members)
			{
				if (memberInfo.IsDefined(typeof(DatapointAttribute), true) && GetTypeFromMemberInfo(memberInfo) == parameterType)
				{
					return true;
				}
				if (memberInfo.IsDefined(typeof(DatapointSourceAttribute), true) && GetElementTypeFromMemberInfo(memberInfo) == parameterType)
				{
					return true;
				}
			}
			return false;
		}

		public IEnumerable GetDataFor(IParameterInfo parameter)
		{
			List<object> list = new List<object>();
			Type parameterType = parameter.ParameterType;
			Type type = parameter.Method.TypeInfo.Type;
			MemberInfo[] members = type.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
			foreach (MemberInfo memberInfo in members)
			{
				if (memberInfo.IsDefined(typeof(DatapointAttribute), true))
				{
					FieldInfo fieldInfo = memberInfo as FieldInfo;
					if (GetTypeFromMemberInfo(memberInfo) == parameterType && fieldInfo != null)
					{
						if (fieldInfo.IsStatic)
						{
							list.Add(fieldInfo.GetValue(null));
						}
						else
						{
							list.Add(fieldInfo.GetValue(ProviderCache.GetInstanceOf(type)));
						}
					}
				}
				else
				{
					if (!memberInfo.IsDefined(typeof(DatapointSourceAttribute), true) || GetElementTypeFromMemberInfo(memberInfo) != parameterType)
					{
						continue;
					}
					FieldInfo fieldInfo2 = memberInfo as FieldInfo;
					PropertyInfo propertyInfo = memberInfo as PropertyInfo;
					MethodInfo methodInfo = memberInfo as MethodInfo;
					if (fieldInfo2 != null)
					{
						object obj = (fieldInfo2.IsStatic ? null : ProviderCache.GetInstanceOf(type));
						foreach (object item in (IEnumerable)fieldInfo2.GetValue(obj))
						{
							list.Add(item);
						}
					}
					else if (propertyInfo != null)
					{
						object obj = (propertyInfo.GetGetMethod(true).IsStatic ? null : ProviderCache.GetInstanceOf(type));
						foreach (object item2 in (IEnumerable)propertyInfo.GetValue(obj, null))
						{
							list.Add(item2);
						}
					}
					else
					{
						if (methodInfo == null)
						{
							continue;
						}
						object obj = (methodInfo.IsStatic ? null : ProviderCache.GetInstanceOf(type));
						foreach (object item3 in (IEnumerable)methodInfo.Invoke(obj, new Type[0]))
						{
							list.Add(item3);
						}
					}
				}
			}
			if (list.Count == 0)
			{
				if (parameterType == typeof(bool))
				{
					list.Add(true);
					list.Add(false);
				}
				else if (parameterType.GetTypeInfo().IsEnum)
				{
					foreach (object enumValue in TypeHelper.GetEnumValues(parameterType))
					{
						list.Add(enumValue);
					}
				}
			}
			return list;
		}

		private Type GetTypeFromMemberInfo(MemberInfo member)
		{
			FieldInfo fieldInfo = member as FieldInfo;
			if (fieldInfo != null)
			{
				return fieldInfo.FieldType;
			}
			PropertyInfo propertyInfo = member as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.PropertyType;
			}
			MethodInfo methodInfo = member as MethodInfo;
			if (methodInfo != null)
			{
				return methodInfo.ReturnType;
			}
			return null;
		}

		private Type GetElementTypeFromMemberInfo(MemberInfo member)
		{
			Type typeFromMemberInfo = GetTypeFromMemberInfo(member);
			if (typeFromMemberInfo == null)
			{
				return null;
			}
			if (typeFromMemberInfo.IsArray)
			{
				return typeFromMemberInfo.GetElementType();
			}
			if (typeFromMemberInfo.GetTypeInfo().IsGenericType && typeFromMemberInfo.Name == "IEnumerable`1")
			{
				return typeFromMemberInfo.GetGenericArguments()[0];
			}
			return null;
		}
	}
}
