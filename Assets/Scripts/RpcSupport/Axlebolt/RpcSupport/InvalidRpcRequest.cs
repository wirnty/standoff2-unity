namespace Axlebolt.RpcSupport
{
	public class InvalidRpcRequest : RpcException
	{
		public InvalidRpcRequest()
			: base(400)
		{
		}
	}
}
