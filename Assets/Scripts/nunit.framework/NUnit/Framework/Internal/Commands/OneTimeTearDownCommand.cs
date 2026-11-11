using System;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;

namespace NUnit.Framework.Internal.Commands
{
	public class OneTimeTearDownCommand : TestCommand
	{
		private List<SetUpTearDownItem> _setUpTearDownItems;

		private List<TestActionItem> _actions;

		public OneTimeTearDownCommand(TestSuite suite, List<SetUpTearDownItem> setUpTearDownItems, List<TestActionItem> actions)
			: base(suite)
		{
			_setUpTearDownItems = setUpTearDownItems;
			_actions = actions;
		}

		public override TestResult Execute(TestExecutionContext context)
		{
			TestResult currentResult = context.CurrentResult;
			try
			{
				int num = _actions.Count;
				while (num > 0)
				{
					_actions[--num].AfterTest(base.Test);
				}
				if (_setUpTearDownItems != null)
				{
					foreach (SetUpTearDownItem setUpTearDownItem in _setUpTearDownItems)
					{
						setUpTearDownItem.RunTearDown(context);
					}
				}
				IDisposable disposable = context.TestObject as IDisposable;
				if (disposable != null && base.Test is IDisposableFixture)
				{
					disposable.Dispose();
				}
			}
			catch (Exception ex)
			{
				currentResult.RecordTearDownException(ex);
			}
			return currentResult;
		}
	}
}
