using System;
using System.Reflection;

namespace NUnit.Compatibility
{
	public static class AttributeHelper
	{
		public static Attribute[] GetCustomAttributes(object actual, Type attributeType, bool inherit)
		{
			ICustomAttributeProvider obj = actual as ICustomAttributeProvider;
			if (obj == null)
			{
				throw new ArgumentException(string.Format("Actual value {0} does not implement ICustomAttributeProvider.", actual), "actual");
			}
			return (Attribute[])obj.GetCustomAttributes(attributeType, inherit);
		}
	}
}
