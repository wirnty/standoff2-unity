using System;
using System.Collections.Generic;
using System.Linq;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Inventory.HitHandling;
using UnityEngine;

namespace Axlebolt.Standoff.Effects
{
	public class SurfaceImpactsEmitter : SimpleEffectsEmitter<SurfaceImpactsEmitter, SurfaceType>
	{
		private class Pair
		{
			public readonly int Min;

			public readonly int Max;

			public Pair(int min, int max)
			{
				Min = min;
				Max = max;
			}
		}

		private SurfaceImpactParams _params;

		private readonly Dictionary<SurfaceType, SurfaceImpactParam> _paramByType = new Dictionary<SurfaceType, SurfaceImpactParam>();

		private readonly Dictionary<SurfaceType, Pair> _decalIndexes = new Dictionary<SurfaceType, Pair>();

		private DecalsDrawer _decalsDrawer;

		public void Init()
		{
			_params = ResourcesUtility.Load<SurfaceImpactParams>("Effects/SurfaceImpactParams");
			if (_params.Params == null || _params.Params.Length == 0)
			{
				SimpleEffectsEmitter<SurfaceImpactsEmitter, SurfaceType>.Log.Error("SurfaceImpactParams.Params are empty");
				return;
			}
			SurfaceImpactParam[] @params = _params.Params;
			foreach (SurfaceImpactParam surfaceImpactParam in @params)
			{
				SurfaceType[] surfaceTypes = surfaceImpactParam.SurfaceTypes;
				foreach (SurfaceType surfaceType in surfaceTypes)
				{
					if (_paramByType.ContainsKey(surfaceType))
					{
						SimpleEffectsEmitter<SurfaceImpactsEmitter, SurfaceType>.Log.Error(string.Format("{0} already defined", surfaceType));
						return;
					}
					_paramByType[surfaceType] = surfaceImpactParam;
				}
			}
			if (ValidateParams())
			{
				Init(_params.EffectDetails);
			}
		}

		private bool ValidateParams()
		{
			IEnumerable<SurfaceType> enumerable = Enum.GetValues(typeof(SurfaceType)).Cast<SurfaceType>();
			bool result = true;
			foreach (SurfaceType item in enumerable)
			{
				if (item != SurfaceType.Character && item != 0)
				{
					if (!_paramByType.ContainsKey(item))
					{
						SimpleEffectsEmitter<SurfaceImpactsEmitter, SurfaceType>.Log.Error(string.Format("SurfaceImpact not found for SurfaceType {0}", item));
						result = false;
					}
					else if (_paramByType[item].Particles == null)
					{
						SimpleEffectsEmitter<SurfaceImpactsEmitter, SurfaceType>.Log.Error(string.Format("SurfaceImpact.Particles for SurfaceType {0} is null", item));
						result = false;
					}
				}
			}
			return result;
		}

		protected override SurfaceType[] GetTypes()
		{
			return _paramByType.Keys.ToArray();
		}

		protected override ParticleSystem GetParticles(SurfaceType type)
		{
			return _paramByType[type].Particles;
		}

		protected override void InitPool(int poolSize)
		{
			base.InitPool(poolSize);
			InitDecals();
		}

		private void InitDecals()
		{
			List<Sprite> list = new List<Sprite>();
			int num = 0;
			SurfaceImpactParam[] @params = _params.Params;
			foreach (SurfaceImpactParam surfaceImpactParam in @params)
			{
				SurfaceType[] surfaceTypes = surfaceImpactParam.SurfaceTypes;
				foreach (SurfaceType key in surfaceTypes)
				{
					_decalIndexes[key] = new Pair(num, num + surfaceImpactParam.Decals.Length);
					list.AddRange(surfaceImpactParam.Decals);
					num += surfaceImpactParam.Decals.Length;
				}
			}
			_decalsDrawer = SimpleDecalsDrawer.Create(_params.BulletHoleMaterial, list.ToArray());
		}

		protected override void ClearPool()
		{
			base.ClearPool();
			_decalIndexes.Clear();
			if (_decalsDrawer != null)
			{
				UnityEngine.Object.Destroy(_decalsDrawer.gameObject);
			}
		}

		public void Emit(RaycastHit raycastHit, SurfaceType surfaceType, bool isLocal)
		{
			if (surfaceType != SurfaceType.Character && surfaceType != 0 && PoolSize != 0)
			{
				Emit(surfaceType, raycastHit.point, raycastHit.normal, isLocal);
				DrawBulletHole(surfaceType, raycastHit);
			}
		}

		private void DrawBulletHole(SurfaceType surfaceType, RaycastHit raycastHit)
		{
			SurfaceImpactParam surfaceImpactParam = _paramByType[surfaceType];
			Pair pair = _decalIndexes[surfaceType];
			int decalIndex = UnityEngine.Random.Range(pair.Min, pair.Max);
			_decalsDrawer.DrawDecal(raycastHit, surfaceImpactParam.Size, decalIndex);
		}
	}
}
