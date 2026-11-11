using System;

namespace NUnit.Framework.Constraints
{
	public class ExactTypeConstraint : TypeConstraint
	{
		public override string DisplayName
		{
			get
			{
				return "TypeOf";
			}
		}

		public ExactTypeConstraint(Type type)
			: base(type, string.Empty)
		{
		}

		protected override bool Matches(object actual)
		{
			if (actual != null)
			{
				return actual.GetType() == expectedType;
			}
			return false;
		}
	}
}
