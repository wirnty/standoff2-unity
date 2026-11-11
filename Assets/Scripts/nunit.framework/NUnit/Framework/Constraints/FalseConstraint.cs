namespace NUnit.Framework.Constraints
{
	public class FalseConstraint : Constraint
	{
		public FalseConstraint()
		{
			Description = "False";
		}

		public override ConstraintResult ApplyTo<TActual>(TActual actual)
		{
			return new ConstraintResult(this, actual, false.Equals(actual));
		}
	}
}
