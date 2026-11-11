using System;
using Axlebolt.Standoff.UI;

namespace Axlebolt.Standoff.Settings
{
	public abstract class SettingsTabController : View
	{
		public Action ChangedHandler { get; set; }

		public abstract string Title { get; }

		public abstract void InitValues();

		public abstract void Apply();

		public abstract void RestoreDefaults();
	}
}
