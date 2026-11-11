using System;

namespace NUnit.Framework.Constraints
{
	internal class ExceptionNotThrownConstraint : Constraint
	{
		public override string Description
		{
			get
			{
				return "No Exception to be thrown";
			}
		}

		public override ConstraintResult ApplyTo<TActual>(TActual actual)
		{
			Exception ex = actual as Exception;
			return new ConstraintResult(this, ex, ex == null);
		}
	}
}
