using System;

namespace Axlebolt.RpcSupport
{
	public class ConnectionFailedException : Exception
	{
		public ConnectionFailedException()
		{
		}

		public ConnectionFailedException(string message)
			: base(message)
		{
		}

		public ConnectionFailedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
