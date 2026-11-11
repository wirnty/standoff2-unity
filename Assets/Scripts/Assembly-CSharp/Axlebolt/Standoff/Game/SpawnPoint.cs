using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public class SpawnPoint
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly Vector3 _003CPosition_003Ek__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly Quaternion _003CRotation_003Ek__BackingField;

		public Vector3 Position
		{
			[CompilerGenerated]
			get
			{
				return _003CPosition_003Ek__BackingField;
			}
		}

		public Quaternion Rotation
		{
			[CompilerGenerated]
			get
			{
				return _003CRotation_003Ek__BackingField;
			}
		}

		public SpawnPoint(Vector3 position, Quaternion rotation)
		{
			_003CPosition_003Ek__BackingField = position;
			_003CRotation_003Ek__BackingField = rotation;
		}
	}
}
