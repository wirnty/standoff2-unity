using System;
using System.Collections.Generic;
using System.Linq;
using Axlebolt.Standoff.Core;

namespace Axlebolt.Standoff.Inventory.HitHandling
{
	public class SurfaceTypeUtility
	{
		private static readonly Log Log;

		public const string TagPrefix = "SurfaceType/";

		private static readonly Dictionary<string, SurfaceType> EnumByTag;

		static SurfaceTypeUtility()
		{
			Log = Log.Create(typeof(BulletCaster));
			EnumByTag = new Dictionary<string, SurfaceType>();
			foreach (SurfaceType item in Enum.GetValues(typeof(SurfaceType)).Cast<SurfaceType>())
			{
				EnumByTag[GetTag(item)] = item;
			}
		}

		public static string GetTag(SurfaceType surfaceType)
		{
			return "SurfaceType/" + surfaceType;
		}

		public static SurfaceType FromTag(string surfaceType)
		{
			SurfaceType value;
			if (EnumByTag.TryGetValue(surfaceType, out value))
			{
				return value;
			}
			Log.Error(string.Format("Unsupported SurfaceType {0}", surfaceType));
			return SurfaceType.Unknown;
		}
	}
}
