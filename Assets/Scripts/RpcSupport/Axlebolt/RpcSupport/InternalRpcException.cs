namespace Axlebolt.RpcSupport
{
	public class InternalRpcException : RpcException
	{
		public InternalRpcException()
			: base(500)
		{
		}
	}
}
