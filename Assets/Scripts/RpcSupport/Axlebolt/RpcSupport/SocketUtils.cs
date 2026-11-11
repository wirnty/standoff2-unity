using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Axlebolt.RpcSupport
{
	public static class SocketUtils
	{
		public static void KeepAlive(this Socket socket, int pollSeconds)
		{
			socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
			SetIOControlKeepAlive(socket, (uint)(pollSeconds * 1000), 1u);
		}

		private static void SetIOControlKeepAlive(Socket socket, uint time, uint interval)
		{
			int num = Marshal.SizeOf(time);
			byte[] array = new byte[num * 3];
			BitConverter.GetBytes((uint)((time != 0) ? 1 : 0)).CopyTo(array, 0);
			BitConverter.GetBytes(time).CopyTo(array, num);
			BitConverter.GetBytes(interval).CopyTo(array, num * 2);
			socket.IOControl(IOControlCode.KeepAliveValues, array, null);
		}
	}
}
