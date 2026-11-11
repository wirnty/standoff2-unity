using System.Collections;
using System.Threading.Tasks;

namespace Axlebolt.Standoff.Test
{
	public abstract class BaseTest
	{
		protected IEnumerator Async(Task task)
		{
			while (!task.IsCompleted)
			{
				yield return null;
			}
			if (task.IsFaulted)
			{
				throw task.Exception;
			}
		}
	}
}
