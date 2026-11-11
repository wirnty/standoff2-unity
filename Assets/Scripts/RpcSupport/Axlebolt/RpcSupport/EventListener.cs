using System;

namespace Axlebolt.RpcSupport
{
	[AttributeUsage(AttributeTargets.Interface)]
	public class EventListener : Attribute
	{
		public readonly string name;

		public EventListener(string name)
		{
			this.name = name;
		}
	}
}
