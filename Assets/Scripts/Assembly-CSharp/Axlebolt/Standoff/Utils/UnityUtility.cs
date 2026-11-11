using UnityEngine;

namespace Axlebolt.Standoff.Utils
{
	public class UnityUtility
	{
		public static Transform FindDeepChild(Transform aParent, string aName)
		{
			Transform transform = aParent.Find(aName);
			if (transform != null)
			{
				return transform;
			}
			foreach (Transform item in aParent)
			{
				transform = FindDeepChild(item, aName);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		public static void SetLayerRecursively(GameObject go, int layer)
		{
			go.layer = layer;
			Transform[] componentsInChildren = go.GetComponentsInChildren<Transform>(true);
			foreach (Transform transform in componentsInChildren)
			{
				transform.gameObject.layer = layer;
			}
		}
	}
}
