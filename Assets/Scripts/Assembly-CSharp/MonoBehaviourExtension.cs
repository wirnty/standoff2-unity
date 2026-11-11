using System;
using UnityEngine;

public static class MonoBehaviourExtension
{
	public class RequireComponentMissingException : InvalidOperationException
	{
		public RequireComponentMissingException(UnityEngine.Object component, Type componentType)
			: base(string.Concat(component, " not define require component ", componentType))
		{
		}
	}

	public static T GetRequireComponent<T>(this Component component)
	{
		T component2 = component.GetComponent<T>();
		if (component2 == null)
		{
			throw new RequireComponentMissingException(component, typeof(T));
		}
		return component2;
	}

	public static T GetRequireComponent<T>(this GameObject gameObject)
	{
		T component = gameObject.GetComponent<T>();
		if (component == null)
		{
			throw new RequireComponentMissingException(gameObject, typeof(T));
		}
		return component;
	}
}
