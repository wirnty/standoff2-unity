using System;

namespace Axlebolt.Standoff.Settings
{
	[Serializable]
	public struct QualityData
	{
		public QualityLvl ShaderDetail;

		public QualityLvl ModelDetail;

		public QualityLvl TextureDetail;

		public QualityLvl EffectDetail;

		public QualityLvl ShadowDetail;

		public int AntiAliasing;
	}
}
