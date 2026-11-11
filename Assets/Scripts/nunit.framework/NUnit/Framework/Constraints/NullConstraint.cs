namespace NUnit.Framework.Constraints
{
	public class NullConstraint : Constraint
	{
		public NullConstraint()
		{
			Description = "null";
		}

		public override ConstraintResult ApplyTo<TActual>(TActual actual)
		{
			return new ConstraintResult(this, actual, actual == null);
		}
	}
}
