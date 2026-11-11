using System;

namespace Axlebolt.RpcSupport
{
	[AttributeUsage(AttributeTargets.Method)]
	public class Rpc : Attribute
	{
		public readonly string Name;

		public Rpc(string name)
		{
			Name = name;
		}
	}
}
