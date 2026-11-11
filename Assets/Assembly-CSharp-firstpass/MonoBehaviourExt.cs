using UnityEngine;

public static class MonoBehaviourExt
{
	public static T GetComponentInChildren2<T>(this GameObject self, bool alsoInactive = false) where T : MonoBehaviour
	{
		return Iterate<T>(self, alsoInactive);
	}

	private static T Iterate<T>(GameObject go, bool alsoInactive) where T : MonoBehaviour
	{
		if (!go.activeInHierarchy && !alsoInactive)
		{
			return (T)null;
		}
		MonoBehaviour[] components = go.GetComponents<MonoBehaviour>();
		foreach (MonoBehaviour monoBehaviour in components)
		{
			if (monoBehaviour.GetType() == typeof(T))
			{
				return (T)monoBehaviour;
			}
		}
		for (int j = 0; j < go.transform.childCount; j++)
		{
			Transform child = go.transform.GetChild(j);
			T val = Iterate<T>(child.gameObject, alsoInactive);
			if ((Object)val != (Object)null)
			{
				return val;
			}
		}
		return (T)null;
	}
}
