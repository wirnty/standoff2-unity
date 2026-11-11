namespace Axlebolt.RpcSupport
{
	public class UnauthorizedException : RpcException
	{
		public UnauthorizedException()
			: base(401)
		{
		}
	}
}
