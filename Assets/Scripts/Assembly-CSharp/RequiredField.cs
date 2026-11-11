using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

public static class RequiredField
{
	[Conditional("UNITY_EDITOR")]
	public static void CheckRequiredFields(Component a_Component)
	{
		Type type = a_Component.GetType();
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (FieldInfo fieldInfo in fields)
		{
			object[] customAttributes = fieldInfo.GetCustomAttributes(true);
			for (int j = 0; j < customAttributes.Length; j++)
			{
				if (customAttributes[j].GetType() == typeof(RequiredFieldAttribute) && fieldInfo.GetValue(a_Component) == null)
				{
					string text = ToEditorFormat(fieldInfo.Name);
					string objectType = GetObjectType(a_Component);
					string gameObjectName = GetGameObjectName(a_Component);
					UnityEngine.Debug.LogError(gameObjectName + " : The Required field " + text + " in component " + objectType + " is empty. Please, fill it with the appropriate component.", a_Component);
					UnityEngine.Debug.Break();
				}
			}
		}
	}

	private static string Highlight(string a_text)
	{
		return "<b><color=white>" + a_text + "</color></b>";
	}

	public static string ToEditorFormat(string a_text)
	{
		string text = a_text;
		if (text[0] == '_')
		{
			text = text.Remove(0, 1);
		}
		else if (text.StartsWith("m_"))
		{
			text = text.Remove(0, 2);
		}
		text = text[0].ToString().ToUpper() + text.Remove(0, 1);
		return Highlight(text);
	}

	private static string GetGameObjectName(Component a_component)
	{
		return Highlight(a_component.gameObject.name);
	}

	private static string GetObjectType(UnityEngine.Object a_object)
	{
		return Highlight(a_object.GetType().Name);
	}
}
