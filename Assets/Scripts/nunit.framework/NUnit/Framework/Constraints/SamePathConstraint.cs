using NUnit.Framework.Internal;

namespace NUnit.Framework.Constraints
{
	public class SamePathConstraint : PathConstraint
	{
		public override string Description
		{
			get
			{
				return "Path matching " + MsgUtils.FormatValue(expected);
			}
		}

		public SamePathConstraint(string expected)
			: base(expected)
		{
		}

		protected override bool Matches(string actual)
		{
			if (actual != null)
			{
				return StringUtil.StringsEqual(Canonicalize(expected), Canonicalize(actual), caseInsensitive);
			}
			return false;
		}
	}
}
