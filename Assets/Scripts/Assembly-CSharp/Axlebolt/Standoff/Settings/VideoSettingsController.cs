using System.Runtime.CompilerServices;
using Axlebolt.Standoff.UI;
using I2.Loc;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Settings
{
	public class VideoSettingsController : SettingsTabController
	{
		[NotNull]
		[SerializeField]
		private QualityLvlCombobox _shaderDetailField;

		[SerializeField]
		[NotNull]
		private QualityLvlCombobox _modelDetailField;

		[SerializeField]
		[NotNull]
		private QualityLvlCombobox _textureDetailField;

		[SerializeField]
		[NotNull]
		private QualityLvlCombobox _effectDetailField;

		[SerializeField]
		[NotNull]
		private QualityLvlCombobox _shadowDetailField;

		[SerializeField]
		[NotNull]
		private IntegerCombobox _antiAliasingField;

		private QualityData _model;

		public override string Title
		{
			[CompilerGenerated]
			get
			{
				return ScriptLocalization.VideoSettings.Title;
			}
		}

		public override void InitValues()
		{
			_model = QualityManager.Instance.Get();
			_shaderDetailField.SetInterval(QualityLvl.VeryLow, QualityLvl.VeryHigh);
			_shaderDetailField.Value = _model.ShaderDetail;
			_shaderDetailField.ValueChangedHandler = delegate(QualityLvl value)
			{
				_model.ShaderDetail = value;
				base.ChangedHandler();
			};
			_textureDetailField.SetInterval(QualityLvl.VeryLow, QualityLvl.High);
			_textureDetailField.Value = _model.TextureDetail;
			_textureDetailField.ValueChangedHandler = delegate(QualityLvl value)
			{
				_model.TextureDetail = value;
				base.ChangedHandler();
			};
			_modelDetailField.SetInterval(QualityLvl.VeryLow, QualityLvl.High);
			_modelDetailField.Value = _model.ModelDetail;
			_modelDetailField.ValueChangedHandler = delegate(QualityLvl value)
			{
				_model.ModelDetail = value;
				base.ChangedHandler();
			};
			_effectDetailField.SetInterval(QualityLvl.Disabled, QualityLvl.High);
			_effectDetailField.Value = _model.EffectDetail;
			_effectDetailField.ValueChangedHandler = delegate(QualityLvl value)
			{
				_model.EffectDetail = value;
				base.ChangedHandler();
			};
		}

		private static string AntiAliasingFormatter(int value)
		{
			if (value == 0)
			{
				return ScriptLocalization.Settings.Disabled;
			}
			return value + "x MSAA";
		}

		public override void Apply()
		{
			QualityManager.Instance.Save(_model);
		}

		public override void RestoreDefaults()
		{
			QualityData qualityData = QualityManager.Instance.DefaultConfiguration();
			_shaderDetailField.Value = qualityData.ShaderDetail;
			_textureDetailField.Value = qualityData.TextureDetail;
			_modelDetailField.Value = qualityData.ModelDetail;
			_effectDetailField.Value = qualityData.EffectDetail;
			_shadowDetailField.Value = qualityData.ShadowDetail;
			_antiAliasingField.Value = qualityData.AntiAliasing;
		}
	}
}
