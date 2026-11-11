namespace Axlebolt.Standoff.Settings
{
	public interface ISettingsStore
	{
		T Load<T>(string key, T defaultValue = default(T));

		void Save<T>(string key, T value);
	}
}
