using System;

namespace Axlebolt.Standoff.Settings
{
	[Serializable]
	public class ControlSettings
	{
		public const float SensitivityMin = 0.5f;

		public const float SensitivityMax = 2f;

		public const float AccelerationMin = 1f;

		public const float AccelerationMax = 3f;

		public float Sensitivity;

		public float Acceleration;
	}
}
