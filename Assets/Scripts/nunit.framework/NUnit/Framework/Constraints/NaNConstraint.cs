namespace NUnit.Framework.Constraints
{
	public class NaNConstraint : Constraint
	{
		public override string Description
		{
			get
			{
				return "NaN";
			}
		}

		public override ConstraintResult ApplyTo<TActual>(TActual actual)
		{
			return new ConstraintResult(this, actual, (actual is double && double.IsNaN((double)(object)actual)) || (actual is float && float.IsNaN((float)(object)actual)));
		}
	}
}
