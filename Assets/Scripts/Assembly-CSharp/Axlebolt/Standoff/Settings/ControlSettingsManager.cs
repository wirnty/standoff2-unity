using System.Runtime.CompilerServices;

namespace Axlebolt.Standoff.Settings
{
	public class ControlSettingsManager
	{
		private const string StoreKey = "ControlSettings";

		private static ControlSettingsManager _instance;

		public static ControlSettingsManager Instance
		{
			[CompilerGenerated]
			get
			{
				return _instance ?? (_instance = new ControlSettingsManager());
			}
		}

		public ControlSettings Model { get; private set; }

		private ControlSettingsManager()
		{
			Model = SettingsStore.Instance.Load("ControlSettings", DefaultValue());
		}

		public static ControlSettings DefaultValue()
		{
			ControlSettings controlSettings = new ControlSettings();
			controlSettings.Sensitivity = 1f;
			controlSettings.Acceleration = 1f;
			return controlSettings;
		}

		public void Save(ControlSettings model)
		{
			Model = model;
			SettingsStore.Instance.Save("ControlSettings", Model);
		}

		public void Apply()
		{
		}
	}
}
