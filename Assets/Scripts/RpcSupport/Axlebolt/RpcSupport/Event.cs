using System;

namespace Axlebolt.RpcSupport
{
	[AttributeUsage(AttributeTargets.Method)]
	public class Event : Attribute
	{
		public readonly string name;

		public Event(string name)
		{
			this.name = name;
		}
	}
}
