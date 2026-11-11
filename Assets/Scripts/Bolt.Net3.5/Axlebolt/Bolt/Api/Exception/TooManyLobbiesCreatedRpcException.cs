using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api.Exception
{
	public class TooManyLobbiesCreatedRpcException : RpcException
	{
		public TooManyLobbiesCreatedRpcException()
			: base(5007)
		{
		}
	}
}
