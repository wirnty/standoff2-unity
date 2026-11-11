using System;

namespace Axlebolt.Standoff.Common
{
	public class ResourceNotFoundException : Exception
	{
		public ResourceNotFoundException(string path)
			: base(string.Format("Resource {0} not found", path))
		{
		}
	}
}
