namespace Axlebolt.RpcSupport
{
	public interface ILogStream
	{
		void Debug(object msg);

		void Error(object msg);
	}
}
