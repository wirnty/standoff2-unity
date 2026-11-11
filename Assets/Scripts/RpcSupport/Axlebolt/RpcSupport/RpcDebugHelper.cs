namespace Axlebolt.RpcSupport
{
	public class RpcDebugHelper
	{
		public static bool DebugDisconnect { get; private set; }

		public static void Disconnect()
		{
			DebugDisconnect = true;
		}

		public static void Reset()
		{
			DebugDisconnect = false;
		}
	}
}
