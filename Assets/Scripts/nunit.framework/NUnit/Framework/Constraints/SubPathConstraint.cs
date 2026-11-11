namespace NUnit.Framework.Constraints
{
	public class SubPathConstraint : PathConstraint
	{
		public override string Description
		{
			get
			{
				return "Subpath of " + MsgUtils.FormatValue(expected);
			}
		}

		public SubPathConstraint(string expected)
			: base(expected)
		{
		}

		protected override bool Matches(string actual)
		{
			if (actual != null)
			{
				return IsSubPath(Canonicalize(expected), Canonicalize(actual));
			}
			return false;
		}
	}
}
