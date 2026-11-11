using System;
using NUnit.Compatibility;

namespace NUnit.Framework.Constraints
{
	public class AssignableFromConstraint : TypeConstraint
	{
		public AssignableFromConstraint(Type type)
			: base(type, "assignable from ")
		{
		}

		protected override bool Matches(object actual)
		{
			if (actual != null)
			{
				return actual.GetType().GetTypeInfo().IsAssignableFrom(expectedType.GetTypeInfo());
			}
			return false;
		}
	}
}
