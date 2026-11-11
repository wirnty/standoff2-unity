using System.Runtime.CompilerServices;
using Axlebolt.Standoff.UI;
using I2.Loc;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Settings
{
	public class ControlSettingsController : SettingsTabController
	{
		[SerializeField]
		[NotNull]
		private ExtendedSlider _sensitivitySlider;

		[SerializeField]
		[NotNull]
		private ExtendedSlider _accelerationSlider;

		private ControlSettings _model;

		public override string Title
		{
			[CompilerGenerated]
			get
			{
				return ScriptLocalization.ControlSettings.Title;
			}
		}

		public override void InitValues()
		{
			_model = ControlSettingsManager.Instance.Model;
			_sensitivitySlider.Slider.minValue = 0.5f;
			_sensitivitySlider.Slider.maxValue = 2f;
			_sensitivitySlider.Value = _model.Sensitivity;
			_sensitivitySlider.ValueChangedHandler = delegate(float value)
			{
				_model.Sensitivity = value;
				base.ChangedHandler();
			};
			_accelerationSlider.Slider.minValue = 1f;
			_accelerationSlider.Slider.maxValue = 3f;
			_accelerationSlider.Value = _model.Acceleration;
			_accelerationSlider.ValueChangedHandler = delegate(float value)
			{
				_model.Acceleration = value;
				base.ChangedHandler();
			};
		}

		public override void Apply()
		{
			ControlSettingsManager.Instance.Save(_model);
		}

		public override void RestoreDefaults()
		{
			ControlSettings controlSettings = ControlSettingsManager.DefaultValue();
			_sensitivitySlider.Value = controlSettings.Sensitivity;
			_accelerationSlider.Value = controlSettings.Acceleration;
		}
	}
}
