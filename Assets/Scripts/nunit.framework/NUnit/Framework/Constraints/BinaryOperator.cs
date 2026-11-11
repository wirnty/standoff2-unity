namespace NUnit.Framework.Constraints
{
	public abstract class BinaryOperator : ConstraintOperator
	{
		public override int LeftPrecedence
		{
			get
			{
				if (!(base.RightContext is CollectionOperator))
				{
					return base.LeftPrecedence;
				}
				return base.LeftPrecedence + 10;
			}
		}

		public override int RightPrecedence
		{
			get
			{
				if (!(base.RightContext is CollectionOperator))
				{
					return base.RightPrecedence;
				}
				return base.RightPrecedence + 10;
			}
		}

		public override void Reduce(ConstraintBuilder.ConstraintStack stack)
		{
			IConstraint right = stack.Pop();
			IConstraint left = stack.Pop();
			stack.Push(ApplyOperator(left, right));
		}

		public abstract IConstraint ApplyOperator(IConstraint left, IConstraint right);
	}
}
