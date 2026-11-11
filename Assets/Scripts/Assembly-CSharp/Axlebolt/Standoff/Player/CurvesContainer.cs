using System.Collections.Generic;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Player.State;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class CurvesContainer
	{
		private static readonly Log Log = Log.Create(typeof(CurvesContainer));

		private static readonly Dictionary<int, CurvedValue> CurvesMap = new Dictionary<int, CurvedValue>();

		public static void RegisterCurve(CurvedValue curve)
		{
			if (curve.id < 0)
			{
				Debug.LogError("Curve Id can not be less than 0");
			}
			else if (!CurvesMap.ContainsKey(curve.id))
			{
				CurvesMap.Add(curve.id, curve);
			}
		}

		public static CurvedValue GetCurve(int id)
		{
			if (id < 0)
			{
				Log.Error(string.Format("Curve Id {0} can not be less than 0", id));
				return null;
			}
			CurvedValue value;
			if (CurvesMap.TryGetValue(id, out value))
			{
				return value;
			}
			Log.Error(string.Format("Curve Not Found. Id: {0}", id));
			return null;
		}
	}
}
