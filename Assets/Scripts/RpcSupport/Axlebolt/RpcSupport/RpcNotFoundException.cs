namespace Axlebolt.RpcSupport
{
	public class RpcNotFoundException : RpcException
	{
		public RpcNotFoundException()
			: base(404)
		{
		}
	}
}
