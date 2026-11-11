namespace NUnit.Framework.Interfaces
{
	public class TestOutput
	{
		public string Text { get; private set; }

		public string Stream { get; private set; }

		public string TestName { get; private set; }

		public TestOutput(string text, string stream, string testName)
		{
			Text = text;
			Stream = stream;
			TestName = testName;
		}

		public override string ToString()
		{
			return Stream + ": " + Text;
		}

		public string ToXml()
		{
			if (TestName == null)
			{
				return string.Format("<test-output stream='{0}'>{1}</test-output>", Stream, Text);
			}
			return string.Format("<test-output stream='{0}' testname='{1}'>{2}</test-output>", Stream, TestName, Text);
		}
	}
}
