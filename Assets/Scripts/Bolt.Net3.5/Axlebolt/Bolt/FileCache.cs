using System;
using System.IO;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt
{
	public class FileCache
	{
		public string CachePath { get; set; }

		public FileCache()
		{
			CachePath = Path.GetTempPath();
		}

		internal byte[] Get(string file)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			try
			{
				string path = Path.Combine(CachePath, file);
				return (!File.Exists(path)) ? null : File.ReadAllBytes(path);
			}
			catch (Exception arg)
			{
				Logger.Error(string.Format("Can't get {0} from cache, {1}", file, arg));
				return null;
			}
		}

		internal void Set(string file, byte[] data)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			try
			{
				string path = Path.Combine(CachePath, file);
				if (data != null)
				{
					File.WriteAllBytes(path, data);
				}
				else
				{
					File.Delete(path);
				}
			}
			catch (Exception arg)
			{
				Logger.Error(string.Format("Can't add {0} to cache, {1}", file, arg));
			}
		}
	}
}
