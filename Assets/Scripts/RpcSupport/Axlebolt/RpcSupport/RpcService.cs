using System;

namespace Axlebolt.RpcSupport
{
	[AttributeUsage(AttributeTargets.Interface, Inherited = true)]
	public class RpcService : Attribute
	{
		public readonly string name;

		public RpcService(string name)
		{
			this.name = name;
		}
	}
}
