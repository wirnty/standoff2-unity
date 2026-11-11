namespace NUnit.Framework.Constraints
{
	public class SameAsConstraint : Constraint
	{
		private readonly object expected;

		public override string Description
		{
			get
			{
				return "same as " + MsgUtils.FormatValue(expected);
			}
		}

		public SameAsConstraint(object expected)
			: base(expected)
		{
			this.expected = expected;
		}

		public override ConstraintResult ApplyTo<TActual>(TActual actual)
		{
			bool isSuccess = expected == (object)actual;
			return new ConstraintResult(this, actual, isSuccess);
		}
	}
}
