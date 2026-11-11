using System;
using Axlebolt.Standoff.Core;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Common
{
	public class ResourcesUtility
	{
		private static readonly Log Log = Log.Create(typeof(ResourcesUtility));

		public static T Load<T>([NotNull] string path) where T : UnityEngine.Object
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("Loading resource {0}", path));
			}
			T val = Resources.Load<T>(path);
			if ((UnityEngine.Object)val == (UnityEngine.Object)null)
			{
				throw new ResourceNotFoundException(path);
			}
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("Resource {0} successfully loaded", path));
			}
			return val;
		}

		public static T[] LoadAll<T>(string path) where T : UnityEngine.Object
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("Loading resources {0}", path));
			}
			T[] result = Resources.LoadAll<T>(path);
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("Resources {0} successfully loaded", path));
			}
			return result;
		}

		public static void Unload([NotNull] UnityEngine.Object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("Unloading resource {0}", o));
			}
			Resources.UnloadAsset(o);
		}
	}
}
