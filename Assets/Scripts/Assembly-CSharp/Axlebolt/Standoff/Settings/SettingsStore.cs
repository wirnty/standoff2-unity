using UnityEngine;

namespace Axlebolt.Standoff.Settings
{
	public class SettingsStore : ISettingsStore
	{
		public static ISettingsStore Instance = new SettingsStore();

		public T Load<T>(string key, T defaultValue = default(T))
		{
			string @string = PlayerPrefs.GetString(key, null);
			return (!string.IsNullOrEmpty(@string)) ? JsonUtility.FromJson<T>(@string) : defaultValue;
		}

		public void Save<T>(string key, T value)
		{
			string value2 = JsonUtility.ToJson(value);
			PlayerPrefs.SetString(key, value2);
			PlayerPrefs.Save();
		}
	}
}
