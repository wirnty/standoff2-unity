using System;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Framework
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class SingleThreadedAttribute : NUnitAttribute, IApplyToContext
	{
		public void ApplyToContext(TestExecutionContext context)
		{
			context.IsSingleThreaded = true;
		}
	}
}
