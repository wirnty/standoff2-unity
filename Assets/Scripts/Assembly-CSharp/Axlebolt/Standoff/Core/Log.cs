using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Core
{
	public class Log
	{
		private readonly string tag;

		private readonly ILogger logger;

		public bool DebugEnabled
		{
			[CompilerGenerated]
			get
			{
				return true;
			}
		}

		public Log(string tag)
		{
			this.tag = tag;
			logger = UnityEngine.Debug.unityLogger;
			logger.logEnabled = true;
		}

		public static Log Create(Type type)
		{
			return new Log(type.Name);
		}

		public void Debug(object message)
		{
			if (DebugEnabled)
			{
				logger.Log(tag, message);
			}
		}

		public void Debug(object message, UnityEngine.Object context)
		{
			if (DebugEnabled)
			{
				logger.Log(tag, message, context);
			}
		}

		public void Warning(string message, UnityEngine.Object context, params object[] args)
		{
			if (DebugEnabled)
			{
				logger.LogWarning(tag, FormatMessage(message, args), context);
			}
		}

		public void Warning(string message, params object[] args)
		{
			if (DebugEnabled)
			{
				logger.LogWarning(tag, FormatMessage(message, args));
			}
		}

		public void Error(object message, UnityEngine.Object context)
		{
			if (DebugEnabled)
			{
				logger.LogError(tag, message, context);
			}
		}

		public void Error(object message)
		{
			if (DebugEnabled)
			{
				logger.LogError(tag, message);
			}
		}

		private static string FormatMessage(string message, params object[] args)
		{
			if (args.Length > 0)
			{
				message = string.Format(message, args);
			}
			return message;
		}
	}
}
