using System;
using System.IO;

namespace NUnit.Framework.Constraints
{
	public class EmptyConstraint : Constraint
	{
		private Constraint realConstraint;

		public override string Description
		{
			get
			{
				if (realConstraint != null)
				{
					return realConstraint.Description;
				}
				return "<empty>";
			}
		}

		public override ConstraintResult ApplyTo<TActual>(TActual actual)
		{
			if (typeof(TActual) == typeof(string))
			{
				realConstraint = new EmptyStringConstraint();
			}
			else
			{
				if (actual == null)
				{
					throw new ArgumentException("The actual value must be a string or a non-null IEnumerable or DirectoryInfo", "actual");
				}
				if (actual is DirectoryInfo)
				{
					realConstraint = new EmptyDirectoryConstraint();
				}
				else
				{
					realConstraint = new EmptyCollectionConstraint();
				}
			}
			return realConstraint.ApplyTo(actual);
		}
	}
}
