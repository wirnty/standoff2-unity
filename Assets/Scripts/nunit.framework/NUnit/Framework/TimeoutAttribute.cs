using System;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Framework
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class TimeoutAttribute : PropertyAttribute, IApplyToContext
	{
		private int _timeout;

		public TimeoutAttribute(int timeout)
			: base(timeout)
		{
			_timeout = timeout;
		}

		void IApplyToContext.ApplyToContext(TestExecutionContext context)
		{
			context.TestCaseTimeout = _timeout;
		}
	}
}
