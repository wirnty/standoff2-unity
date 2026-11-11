using System;

namespace Axlebolt.RpcSupport
{
	public class Logger
	{
		public class ConsoleStream : ILogStream
		{
			public void Debug(object msg)
			{
				Console.Write(msg);
			}

			public void Error(object msg)
			{
				Console.Error.Write(msg);
			}
		}

		public static bool LogDebug = false;

		public static ILogStream Stream { get; set; } = new ConsoleStream();


		public static void Debug(object msg)
		{
			if (LogDebug)
			{
				Stream.Debug(msg);
			}
		}

		public static void Error(object msg)
		{
			Stream.Error(msg);
		}
	}
}
