using System;
using System.Collections.Generic;
using System.Threading;

namespace NUnit.Framework.Internal.Commands
{
	public class TestActionCommand : DelegatingTestCommand
	{
		private IList<TestActionItem> _actions = new List<TestActionItem>();

		public TestActionCommand(TestCommand innerCommand)
			: base(innerCommand)
		{
			Guard.ArgumentValid(innerCommand.Test is TestMethod, "TestActionCommand may only apply to a TestMethod", "innerCommand");
		}

		public override TestResult Execute(TestExecutionContext context)
		{
			if (base.Test.Fixture == null)
			{
				base.Test.Fixture = context.TestObject;
			}
			foreach (ITestAction upstreamAction in context.UpstreamActions)
			{
				_actions.Add(new TestActionItem(upstreamAction));
			}
			ITestAction[] actionsFromAttributeProvider = ActionsHelper.GetActionsFromAttributeProvider(((TestMethod)base.Test).Method.MethodInfo);
			foreach (ITestAction testAction in actionsFromAttributeProvider)
			{
				if (testAction.Targets == ActionTargets.Default || (testAction.Targets & ActionTargets.Test) == ActionTargets.Test)
				{
					_actions.Add(new TestActionItem(testAction));
				}
			}
			try
			{
				for (int j = 0; j < _actions.Count; j++)
				{
					_actions[j].BeforeTest(base.Test);
				}
				context.CurrentResult = innerCommand.Execute(context);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException)
				{
					Thread.ResetAbort();
				}
				context.CurrentResult.RecordException(ex);
			}
			finally
			{
				if (context.ExecutionStatus != TestExecutionStatus.AbortRequested)
				{
					for (int num = _actions.Count - 1; num >= 0; num--)
					{
						_actions[num].AfterTest(base.Test);
					}
				}
			}
			return context.CurrentResult;
		}
	}
}
