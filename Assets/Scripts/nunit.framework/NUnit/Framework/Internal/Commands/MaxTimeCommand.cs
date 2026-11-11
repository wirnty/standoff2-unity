using System.Diagnostics;
using NUnit.Framework.Interfaces;

namespace NUnit.Framework.Internal.Commands
{
	public class MaxTimeCommand : DelegatingTestCommand
	{
		private int maxTime;

		public MaxTimeCommand(TestCommand innerCommand, int maxTime)
			: base(innerCommand)
		{
			this.maxTime = maxTime;
		}

		public override TestResult Execute(TestExecutionContext context)
		{
			long timestamp = Stopwatch.GetTimestamp();
			TestResult testResult = innerCommand.Execute(context);
			double duration = (double)(Stopwatch.GetTimestamp() - timestamp) / (double)Stopwatch.Frequency;
			testResult.Duration = duration;
			if (testResult.ResultState == ResultState.Success)
			{
				double num = testResult.Duration * 1000.0;
				if (num > (double)maxTime)
				{
					testResult.SetResult(ResultState.Failure, string.Format("Elapsed time of {0}ms exceeds maximum of {1}ms", num, maxTime));
				}
			}
			return testResult;
		}
	}
}
