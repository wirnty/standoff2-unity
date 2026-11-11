using System.Collections.Generic;
using NUnit.Framework.Interfaces;

namespace NUnit.Framework.Internal.Builders
{
	public class DefaultTestCaseBuilder : ITestCaseBuilder
	{
		private NUnitTestCaseBuilder _nunitTestCaseBuilder = new NUnitTestCaseBuilder();

		public bool CanBuildFrom(IMethodInfo method)
		{
			if (!method.IsDefined<ITestBuilder>(false))
			{
				return method.IsDefined<ISimpleTestBuilder>(false);
			}
			return true;
		}

		public Test BuildFrom(IMethodInfo method)
		{
			return BuildFrom(method, null);
		}

		public bool CanBuildFrom(IMethodInfo method, Test parentSuite)
		{
			return CanBuildFrom(method);
		}

		public Test BuildFrom(IMethodInfo method, Test parentSuite)
		{
			List<TestMethod> list = new List<TestMethod>();
			List<ITestBuilder> list2 = new List<ITestBuilder>(method.GetCustomAttributes<ITestBuilder>(false));
			bool flag = true;
			foreach (ITestBuilder item in list2)
			{
				if (item is CombiningStrategyAttribute)
				{
					flag = false;
				}
			}
			if (flag)
			{
				list2.Add(new CombinatorialAttribute());
			}
			foreach (ITestBuilder item2 in list2)
			{
				foreach (TestMethod item3 in item2.BuildFrom(method, parentSuite))
				{
					list.Add(item3);
				}
			}
			if (list.Count <= 0)
			{
				return BuildSingleTestMethod(method, parentSuite);
			}
			return BuildParameterizedMethodSuite(method, list);
		}

		private Test BuildParameterizedMethodSuite(IMethodInfo method, IEnumerable<TestMethod> tests)
		{
			ParameterizedMethodSuite parameterizedMethodSuite = new ParameterizedMethodSuite(method);
			parameterizedMethodSuite.ApplyAttributesToTest(method.MethodInfo);
			foreach (TestMethod test in tests)
			{
				parameterizedMethodSuite.Add(test);
			}
			return parameterizedMethodSuite;
		}

		private Test BuildSingleTestMethod(IMethodInfo method, Test suite)
		{
			ISimpleTestBuilder[] customAttributes = method.GetCustomAttributes<ISimpleTestBuilder>(false);
			if (customAttributes.Length == 0)
			{
				return _nunitTestCaseBuilder.BuildTestMethod(method, suite, null);
			}
			return customAttributes[0].BuildFrom(method, suite);
		}
	}
}
