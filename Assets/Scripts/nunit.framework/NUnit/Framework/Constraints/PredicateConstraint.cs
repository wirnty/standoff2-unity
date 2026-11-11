using System;

namespace NUnit.Framework.Constraints
{
	public class PredicateConstraint<T> : Constraint
	{
		private readonly Predicate<T> predicate;

		public override string Description
		{
			get
			{
				string name = predicate.Method.Name;
				if (!name.StartsWith("<"))
				{
					return "value matching " + name;
				}
				return "value matching lambda expression";
			}
		}

		public PredicateConstraint(Predicate<T> predicate)
		{
			this.predicate = predicate;
		}

		public override ConstraintResult ApplyTo<TActual>(TActual actual)
		{
			if (!(actual is T))
			{
				throw new ArgumentException("The actual value is not of type " + typeof(T).Name, "actual");
			}
			return new ConstraintResult(this, actual, predicate((T)(object)actual));
		}
	}
}
