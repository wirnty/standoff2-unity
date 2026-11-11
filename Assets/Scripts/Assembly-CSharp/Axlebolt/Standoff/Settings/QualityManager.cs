using System;
using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Core;
using UnityEngine;

namespace Axlebolt.Standoff.Settings
{
	public class QualityManager
	{
		public delegate void EventHandler();

		private static readonly Log Log = Log.Create(typeof(QualityManager));

		private const string SettingsKey = "QualitySettings";

		private static QualityManager _instance;

		private QualityData _qualityData;

		public static QualityManager Instance
		{
			[CompilerGenerated]
			get
			{
				return _instance ?? (_instance = new QualityManager());
			}
		}

		public QualityLvl ModelDetail
		{
			[CompilerGenerated]
			get
			{
				return _qualityData.ModelDetail;
			}
		}

		public QualityLvl ShaderDetail
		{
			[CompilerGenerated]
			get
			{
				return _qualityData.ShaderDetail;
			}
		}

		public QualityLvl TextureDetail
		{
			[CompilerGenerated]
			get
			{
				return _qualityData.TextureDetail;
			}
		}

		public QualityLvl EffectDetail
		{
			[CompilerGenerated]
			get
			{
				return _qualityData.EffectDetail;
			}
		}

		public QualityLvl ShadowDetail
		{
			[CompilerGenerated]
			get
			{
				return _qualityData.ShadowDetail;
			}
		}

		public int[] AntiAliasingValues
		{
			[CompilerGenerated]
			get
			{
				return new int[4] { 0, 2, 4, 8 };
			}
		}

		public int AntiAliasing
		{
			[CompilerGenerated]
			get
			{
				return _qualityData.AntiAliasing;
			}
		}

		public event EventHandler ModelDetailChanged;

		public event EventHandler ShaderDetailChanged;

		public event EventHandler EffectDetailChanged;

		private QualityManager()
		{
			_qualityData = default(QualityData);
		}

		public QualityData DefaultConfiguration()
		{
			QualityLvl shaderDetail = QualityLvl.High;
			if (SystemInfo.systemMemorySize < 1024)
			{
				shaderDetail = QualityLvl.Medium;
			}
			QualityData result = default(QualityData);
			result.ShaderDetail = shaderDetail;
			result.ModelDetail = QualityLvl.High;
			result.TextureDetail = QualityLvl.Medium;
			result.EffectDetail = QualityLvl.Medium;
			result.ShadowDetail = QualityLvl.Disabled;
			result.AntiAliasing = 0;
			return result;
		}

		public QualityData Get()
		{
			return _qualityData;
		}

		public void Apply()
		{
			_qualityData = SettingsStore.Instance.Load("QualitySettings", DefaultConfiguration());
			Apply(_qualityData);
		}

		public void Save(QualityData data)
		{
			Apply(data);
			SettingsStore.Instance.Save("QualitySettings", _qualityData);
			Resources.UnloadUnusedAssets();
		}

		private void Apply(QualityData data)
		{
			SetModelDetail(data.ModelDetail);
			SetShaderDetail(data.ShaderDetail);
			SetTextureDetail(data.TextureDetail);
			SetEffectDetail(data.EffectDetail);
			SetShadowDetail(data.ShadowDetail);
			SetAntiAliasing(data.AntiAliasing);
		}

		private void SetModelDetail(QualityLvl value)
		{
			if (_qualityData.ModelDetail != value)
			{
				switch (value)
				{
				case QualityLvl.High:
				case QualityLvl.VeryHigh:
					QualitySettings.maximumLODLevel = 0;
					break;
				case QualityLvl.Medium:
					QualitySettings.maximumLODLevel = 1;
					break;
				case QualityLvl.Low:
					QualitySettings.maximumLODLevel = 2;
					break;
				case QualityLvl.VeryLow:
					QualitySettings.maximumLODLevel = 3;
					break;
				default:
					throw new ArgumentOutOfRangeException("value", value, null);
				}
				_qualityData.ModelDetail = value;
				if (this.ModelDetailChanged != null)
				{
					this.ModelDetailChanged();
				}
			}
		}

		private void SetShaderDetail(QualityLvl value)
		{
			if (_qualityData.ShaderDetail != value)
			{
				_qualityData.ShaderDetail = value;
				if (this.ShaderDetailChanged != null)
				{
					this.ShaderDetailChanged();
				}
			}
		}

		private void SetTextureDetail(QualityLvl value)
		{
			int num;
			switch (value)
			{
			case QualityLvl.High:
			case QualityLvl.VeryHigh:
				num = 0;
				break;
			case QualityLvl.Medium:
				num = 1;
				break;
			case QualityLvl.Low:
				num = 2;
				break;
			case QualityLvl.Disabled:
			case QualityLvl.VeryLow:
				num = 3;
				break;
			default:
				throw new ArgumentOutOfRangeException("value", value, null);
			}
			if (QualitySettings.masterTextureLimit != num)
			{
				QualitySettings.masterTextureLimit = num;
				Log.Debug(string.Format("TextureDetailChanged to {0}", value));
			}
			_qualityData.TextureDetail = value;
		}

		private void SetEffectDetail(QualityLvl value)
		{
			if (_qualityData.EffectDetail != value)
			{
				_qualityData.EffectDetail = value;
				if (this.EffectDetailChanged != null)
				{
					this.EffectDetailChanged();
				}
			}
		}

		private void SetShadowDetail(QualityLvl value)
		{
			if (_qualityData.ShadowDetail != value)
			{
				switch (value)
				{
				case QualityLvl.VeryHigh:
					QualitySettings.shadows = ShadowQuality.All;
					QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
					QualitySettings.shadowDistance = 50f;
					break;
				case QualityLvl.High:
					QualitySettings.shadows = ShadowQuality.All;
					QualitySettings.shadowResolution = ShadowResolution.High;
					QualitySettings.shadowDistance = 50f;
					break;
				case QualityLvl.Medium:
					QualitySettings.shadows = ShadowQuality.All;
					QualitySettings.shadowResolution = ShadowResolution.Medium;
					QualitySettings.shadowDistance = 50f;
					break;
				case QualityLvl.Low:
					QualitySettings.shadows = ShadowQuality.All;
					QualitySettings.shadowResolution = ShadowResolution.Low;
					QualitySettings.shadowDistance = 50f;
					break;
				case QualityLvl.VeryLow:
					QualitySettings.shadows = ShadowQuality.All;
					QualitySettings.shadowResolution = ShadowResolution.Low;
					QualitySettings.shadowDistance = 30f;
					break;
				case QualityLvl.Disabled:
					QualitySettings.shadows = ShadowQuality.Disable;
					break;
				}
				_qualityData.ShadowDetail = value;
			}
		}

		private void SetAntiAliasing(int antiAliasing)
		{
			if (_qualityData.AntiAliasing != antiAliasing)
			{
				if (!AntiAliasingValues.Contains(antiAliasing))
				{
					Log.Error(string.Format("Invalid anti-aliasing value {0}", antiAliasing));
				}
				else
				{
					QualitySettings.antiAliasing = antiAliasing;
				}
			}
		}
	}
}
