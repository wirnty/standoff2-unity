namespace NUnit.Framework.Constraints
{
	public class SubstringConstraint : StringConstraint
	{
		public SubstringConstraint(string expected)
			: base(expected)
		{
			descriptionText = "String containing";
		}

		protected override bool Matches(string actual)
		{
			if (caseInsensitive)
			{
				if (actual != null)
				{
					return actual.ToLower().IndexOf(expected.ToLower()) >= 0;
				}
				return false;
			}
			if (actual != null)
			{
				return actual.IndexOf(expected) >= 0;
			}
			return false;
		}
	}
}
