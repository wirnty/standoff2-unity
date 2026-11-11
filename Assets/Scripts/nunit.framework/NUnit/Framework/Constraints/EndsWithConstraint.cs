namespace NUnit.Framework.Constraints
{
	public class EndsWithConstraint : StringConstraint
	{
		public EndsWithConstraint(string expected)
			: base(expected)
		{
			descriptionText = "String ending with";
		}

		protected override bool Matches(string actual)
		{
			if (caseInsensitive)
			{
				if (actual != null)
				{
					return actual.ToLower().EndsWith(expected.ToLower());
				}
				return false;
			}
			if (actual != null)
			{
				return actual.EndsWith(expected);
			}
			return false;
		}
	}
}
