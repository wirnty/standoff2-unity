using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	[CreateAssetMenu(fileName = "SurfaceImpactParams", menuName = "Standoff/Effects/Surface", order = 1)]
	public class SurfaceImpactParams : ScriptableObject
	{
		[SerializeField]
		private EffectDetails _effectDetails;

		[SerializeField]
		private Material _bulletHoleMaterial;

		[SerializeField]
		private SurfaceImpactParam[] _params;

		public EffectDetails EffectDetails
		{
			[CompilerGenerated]
			get
			{
				return _effectDetails;
			}
		}

		public Material BulletHoleMaterial
		{
			[CompilerGenerated]
			get
			{
				return _bulletHoleMaterial;
			}
		}

		public SurfaceImpactParam[] Params
		{
			[CompilerGenerated]
			get
			{
				return _params;
			}
		}
	}
}
