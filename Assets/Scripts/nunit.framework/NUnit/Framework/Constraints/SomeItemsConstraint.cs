using System;
using System.Collections;

namespace NUnit.Framework.Constraints
{
	public class SomeItemsConstraint : PrefixConstraint
	{
		public override string DisplayName
		{
			get
			{
				return "Some";
			}
		}

		public SomeItemsConstraint(IConstraint itemConstraint)
			: base(itemConstraint)
		{
			base.DescriptionPrefix = "some item";
		}

		public override ConstraintResult ApplyTo<TActual>(TActual actual)
		{
			if (!(actual is IEnumerable))
			{
				throw new ArgumentException("The actual value must be an IEnumerable", "actual");
			}
			foreach (object item in (IEnumerable)(object)actual)
			{
				if (base.BaseConstraint.ApplyTo(item).IsSuccess)
				{
					return new ConstraintResult(this, actual, ConstraintStatus.Success);
				}
			}
			return new ConstraintResult(this, actual, ConstraintStatus.Failure);
		}
	}
}
