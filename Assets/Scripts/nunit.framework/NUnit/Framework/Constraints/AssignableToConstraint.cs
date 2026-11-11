using System;
using NUnit.Compatibility;

namespace NUnit.Framework.Constraints
{
	public class AssignableToConstraint : TypeConstraint
	{
		public AssignableToConstraint(Type type)
			: base(type, "assignable to ")
		{
		}

		protected override bool Matches(object actual)
		{
			if (expectedType != null && actual != null)
			{
				return expectedType.GetTypeInfo().IsAssignableFrom(actual.GetType().GetTypeInfo());
			}
			return false;
		}
	}
}
