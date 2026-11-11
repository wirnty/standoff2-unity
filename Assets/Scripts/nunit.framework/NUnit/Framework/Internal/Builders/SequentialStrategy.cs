using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;

namespace NUnit.Framework.Internal.Builders
{
	public class SequentialStrategy : ICombiningStrategy
	{
		public IEnumerable<ITestCaseData> GetTestCases(IEnumerable[] sources)
		{
			List<ITestCaseData> list = new List<ITestCaseData>();
			IEnumerator[] array = new IEnumerator[sources.Length];
			for (int i = 0; i < sources.Length; i++)
			{
				array[i] = sources[i].GetEnumerator();
			}
			while (true)
			{
				bool flag = false;
				object[] array2 = new object[sources.Length];
				for (int j = 0; j < sources.Length; j++)
				{
					if (array[j].MoveNext())
					{
						array2[j] = array[j].Current;
						flag = true;
					}
					else
					{
						array2[j] = null;
					}
				}
				if (!flag)
				{
					break;
				}
				TestCaseParameters item = new TestCaseParameters(array2);
				list.Add(item);
			}
			return list;
		}
	}
}
