using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Settings
{
	public class SettingsManager
	{
		private static SettingsManager _instance;

		public static SettingsManager Instance
		{
			[CompilerGenerated]
			get
			{
				return _instance ?? (_instance = new SettingsManager());
			}
		}

		public void Apply()
		{
			QualityManager.Instance.Apply();
			ControlSettingsManager.Instance.Apply();
		}
	}
}
