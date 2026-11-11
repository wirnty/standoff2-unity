using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using NUnit.Compatibility;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace NUnit.Framework
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	public class TestCaseAttribute : NUnitAttribute, ITestBuilder, ITestCaseData, ITestData, IImplyFixture
	{
		private object _expectedResult;

		private Type _testOf;

		public string TestName { get; set; }

		public RunState RunState { get; private set; }

		public object[] Arguments { get; private set; }

		public IPropertyBag Properties { get; private set; }

		public object ExpectedResult
		{
			get
			{
				return _expectedResult;
			}
			set
			{
				_expectedResult = value;
				HasExpectedResult = true;
			}
		}

		public bool HasExpectedResult { get; private set; }

		public string Description
		{
			get
			{
				return Properties.Get("Description") as string;
			}
			set
			{
				Properties.Set("Description", value);
			}
		}

		public string Author
		{
			get
			{
				return Properties.Get("Author") as string;
			}
			set
			{
				Properties.Set("Author", value);
			}
		}

		public Type TestOf
		{
			get
			{
				return _testOf;
			}
			set
			{
				_testOf = value;
				Properties.Set("TestOf", value.FullName);
			}
		}

		public string Ignore
		{
			get
			{
				return IgnoreReason;
			}
			set
			{
				IgnoreReason = value;
			}
		}

		public bool Explicit
		{
			get
			{
				return RunState == RunState.Explicit;
			}
			set
			{
				RunState = ((!value) ? RunState.Runnable : RunState.Explicit);
			}
		}

		public string Reason
		{
			get
			{
				return Properties.Get("_SKIPREASON") as string;
			}
			set
			{
				Properties.Set("_SKIPREASON", value);
			}
		}

		public string IgnoreReason
		{
			get
			{
				return Reason;
			}
			set
			{
				RunState = RunState.Ignored;
				Reason = value;
			}
		}

		public string IncludePlatform { get; set; }

		public string ExcludePlatform { get; set; }

		public string Category
		{
			get
			{
				return Properties.Get("Category") as string;
			}
			set
			{
				string[] array = value.Split(',');
				foreach (string value2 in array)
				{
					Properties.Add("Category", value2);
				}
			}
		}

		public TestCaseAttribute(params object[] arguments)
		{
			RunState = RunState.Runnable;
			if (arguments == null)
			{
				Arguments = new object[1];
			}
			else
			{
				Arguments = arguments;
			}
			Properties = new PropertyBag();
		}

		public TestCaseAttribute(object arg)
		{
			RunState = RunState.Runnable;
			Arguments = new object[1] { arg };
			Properties = new PropertyBag();
		}

		public TestCaseAttribute(object arg1, object arg2)
		{
			RunState = RunState.Runnable;
			Arguments = new object[2] { arg1, arg2 };
			Properties = new PropertyBag();
		}

		public TestCaseAttribute(object arg1, object arg2, object arg3)
		{
			RunState = RunState.Runnable;
			Arguments = new object[3] { arg1, arg2, arg3 };
			Properties = new PropertyBag();
		}

		private TestCaseParameters GetParametersForTestCase(IMethodInfo method)
		{
			TestCaseParameters testCaseParameters;
			try
			{
				IParameterInfo[] parameters = method.GetParameters();
				int num = parameters.Length;
				int num2 = Arguments.Length;
				testCaseParameters = new TestCaseParameters(this);
				if (num > 0 && num2 >= num - 1)
				{
					IParameterInfo parameterInfo = parameters[num - 1];
					Type parameterType = parameterInfo.ParameterType;
					Type elementType = parameterType.GetElementType();
					if (parameterType.IsArray && parameterInfo.IsDefined<ParamArrayAttribute>(false))
					{
						if (num2 == num)
						{
							Type type = testCaseParameters.Arguments[num2 - 1].GetType();
							if (!parameterType.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
							{
								Array array = Array.CreateInstance(elementType, 1);
								array.SetValue(testCaseParameters.Arguments[num2 - 1], 0);
								testCaseParameters.Arguments[num2 - 1] = array;
							}
						}
						else
						{
							object[] array2 = new object[num];
							for (int i = 0; i < num && i < num2; i++)
							{
								array2[i] = testCaseParameters.Arguments[i];
							}
							int num3 = num2 - num + 1;
							Array array3 = Array.CreateInstance(elementType, num3);
							for (int j = 0; j < num3; j++)
							{
								array3.SetValue(testCaseParameters.Arguments[num + j - 1], j);
							}
							array2[num - 1] = array3;
							testCaseParameters.Arguments = array2;
							num2 = num;
						}
					}
				}
				if (testCaseParameters.Arguments.Length < num)
				{
					object[] array4 = new object[parameters.Length];
					Array.Copy(testCaseParameters.Arguments, array4, testCaseParameters.Arguments.Length);
					for (int k = testCaseParameters.Arguments.Length; k < parameters.Length; k++)
					{
						if (parameters[k].IsOptional)
						{
							array4[k] = Type.Missing;
							continue;
						}
						if (k < testCaseParameters.Arguments.Length)
						{
							array4[k] = testCaseParameters.Arguments[k];
							continue;
						}
						throw new TargetParameterCountException("Incorrect number of parameters specified for TestCase");
					}
					testCaseParameters.Arguments = array4;
				}
				if (num == 1 && method.GetParameters()[0].ParameterType == typeof(object[]) && (num2 > 1 || (num2 == 1 && testCaseParameters.Arguments[0].GetType() != typeof(object[]))))
				{
					testCaseParameters.Arguments = new object[1] { testCaseParameters.Arguments };
				}
				if (num2 == num)
				{
					PerformSpecialConversions(testCaseParameters.Arguments, parameters);
				}
			}
			catch (Exception exception)
			{
				testCaseParameters = new TestCaseParameters(exception);
			}
			return testCaseParameters;
		}

		private static void PerformSpecialConversions(object[] arglist, IParameterInfo[] parameters)
		{
			for (int i = 0; i < arglist.Length; i++)
			{
				object obj = arglist[i];
				Type parameterType = parameters[i].ParameterType;
				if (obj == null)
				{
					continue;
				}
				if (obj is SpecialValue && (SpecialValue)obj == SpecialValue.Null)
				{
					arglist[i] = null;
				}
				else
				{
					if (parameterType.IsAssignableFrom(obj.GetType()))
					{
						continue;
					}
					if (obj is DBNull)
					{
						arglist[i] = null;
						continue;
					}
					bool flag = false;
					if (parameterType == typeof(short) || parameterType == typeof(byte) || parameterType == typeof(sbyte) || parameterType == typeof(short?) || parameterType == typeof(byte?) || parameterType == typeof(sbyte?) || parameterType == typeof(double?))
					{
						flag = obj is int;
					}
					else if (parameterType == typeof(decimal) || parameterType == typeof(decimal?))
					{
						flag = obj is double || obj is string || obj is int;
					}
					else if (parameterType == typeof(DateTime) || parameterType == typeof(DateTime?))
					{
						flag = obj is string;
					}
					if (flag)
					{
						Type conversionType = ((parameterType.GetTypeInfo().IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? parameterType.GetGenericArguments()[0] : parameterType);
						arglist[i] = Convert.ChangeType(obj, conversionType, CultureInfo.InvariantCulture);
					}
					else if ((parameterType == typeof(TimeSpan) || parameterType == typeof(TimeSpan?)) && obj is string)
					{
						arglist[i] = TimeSpan.Parse((string)obj);
					}
				}
			}
		}

		public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
		{
			TestMethod testMethod = new NUnitTestCaseBuilder().BuildTestMethod(method, suite, GetParametersForTestCase(method));
			if (testMethod.RunState != 0 && testMethod.RunState != RunState.Ignored)
			{
				PlatformHelper platformHelper = new PlatformHelper();
				if (!platformHelper.IsPlatformSupported(this))
				{
					testMethod.RunState = RunState.Skipped;
					testMethod.Properties.Add("_SKIPREASON", platformHelper.Reason);
				}
			}
			yield return testMethod;
		}
	}
}
