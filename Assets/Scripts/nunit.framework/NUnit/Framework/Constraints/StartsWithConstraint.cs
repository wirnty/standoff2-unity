namespace NUnit.Framework.Constraints
{
	public class StartsWithConstraint : StringConstraint
	{
		public StartsWithConstraint(string expected)
			: base(expected)
		{
			descriptionText = "String starting with";
		}

		protected override bool Matches(string actual)
		{
			if (caseInsensitive)
			{
				if (actual != null)
				{
					return actual.ToLower().StartsWith(expected.ToLower());
				}
				return false;
			}
			if (actual != null)
			{
				return actual.StartsWith(expected);
			}
			return false;
		}
	}
}
