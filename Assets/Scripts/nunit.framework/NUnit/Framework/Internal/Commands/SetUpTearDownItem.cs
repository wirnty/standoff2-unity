using System;
using System.Collections.Generic;
using System.Reflection;

namespace NUnit.Framework.Internal.Commands
{
	public class SetUpTearDownItem
	{
		private IList<MethodInfo> _setUpMethods;

		private IList<MethodInfo> _tearDownMethods;

		private bool _setUpWasRun;

		public bool HasMethods
		{
			get
			{
				if (_setUpMethods.Count <= 0)
				{
					return _tearDownMethods.Count > 0;
				}
				return true;
			}
		}

		public SetUpTearDownItem(IList<MethodInfo> setUpMethods, IList<MethodInfo> tearDownMethods)
		{
			_setUpMethods = setUpMethods;
			_tearDownMethods = tearDownMethods;
		}

		public void RunSetUp(TestExecutionContext context)
		{
			_setUpWasRun = true;
			foreach (MethodInfo setUpMethod in _setUpMethods)
			{
				RunSetUpOrTearDownMethod(context, setUpMethod);
			}
		}

		public void RunTearDown(TestExecutionContext context)
		{
			if (!_setUpWasRun)
			{
				return;
			}
			try
			{
				int num = _tearDownMethods.Count;
				while (--num >= 0)
				{
					RunSetUpOrTearDownMethod(context, _tearDownMethods[num]);
				}
			}
			catch (Exception ex)
			{
				context.CurrentResult.RecordTearDownException(ex);
			}
		}

		private void RunSetUpOrTearDownMethod(TestExecutionContext context, MethodInfo method)
		{
			RunNonAsyncMethod(method, context);
		}

		private object RunNonAsyncMethod(MethodInfo method, TestExecutionContext context)
		{
			return Reflect.InvokeMethod(method, method.IsStatic ? null : context.TestObject);
		}
	}
}
