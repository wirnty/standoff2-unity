using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public class ISpawnPoint
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly Vector3 _003CPosition_003Ek__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly Quaternion _003CRotation_003Ek__BackingField;

		private Vector3 Position
		{
			[CompilerGenerated]
			get
			{
				return _003CPosition_003Ek__BackingField;
			}
		}

		private Quaternion Rotation
		{
			[CompilerGenerated]
			get
			{
				return _003CRotation_003Ek__BackingField;
			}
		}
	}
}
