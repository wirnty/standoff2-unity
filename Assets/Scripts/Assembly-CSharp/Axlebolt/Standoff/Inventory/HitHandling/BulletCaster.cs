using System.Collections.Generic;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Effects;
using Axlebolt.Standoff.Player;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.HitHandling
{
	public class BulletCaster
	{
		public enum CasterState
		{
			InitialCast = 0,
			PostCast = 1,
			ReverseCast = 2
		}

		private const float CastDistance = 3000f;

		private static CasterState _casterState = CasterState.InitialCast;

		private static readonly List<PlayerHitController> TriggeredPlayerList = new List<PlayerHitController>();

		private const float MinImpulse = 50f;

		public static Vector3 CastBullet(Vector3 startPosition, Vector3 direction, BulletParameters parameters, bool isLocal)
		{
			float num = parameters.PenetrationPower;
			TriggeredPlayerList.Clear();
			int mask = LayerMask.GetMask("PlayerHitbox", "BulletLayer");
			direction.Normalize();
			_casterState = CasterState.InitialCast;
			RaycastHit hit = default(RaycastHit);
			Ray ray = default(Ray);
			ray.direction = direction;
			ray.origin = startPosition;
			Ray ray2 = ray;
			float castDistance = 3000f;
			while (true)
			{
				RaycastHit hit2;
				bool flag = Cast(ray2, out hit2, castDistance, mask);
				if (_casterState == CasterState.InitialCast)
				{
					if (!flag)
					{
						DisableTriggeredPlayers();
						return startPosition + direction.normalized * 3000f;
					}
					SurfaceType surfaceType = SurfaceTypeUtility.FromTag(hit2.collider.tag);
					ray2.origin = hit2.point + direction * 0.0001f;
					ray2.direction = direction;
					if (surfaceType == SurfaceType.Character)
					{
						PlayerHitbox component = hit2.collider.gameObject.GetComponent<PlayerHitbox>();
						component.PlayerHitController.DisableBoneHit();
						TriggeredPlayerList.Remove(component.PlayerHitController);
						if (isLocal)
						{
							float lossCoeff = num / parameters.PenetrationPower;
							component.Hit(CreateHitData(component, hit2, direction, parameters, lossCoeff));
						}
					}
					castDistance = 3000f - Vector3.Distance(startPosition, hit2.point);
					if (HitSurface.IsThinMaterial(surfaceType))
					{
						_casterState = CasterState.InitialCast;
						num -= HitSurface.GetPenetrationLoss(surfaceType, 0f);
					}
					else
					{
						_casterState = CasterState.PostCast;
					}
					hit = hit2;
					DrawHit(hit, surfaceType, isLocal);
					if (num <= 0f)
					{
						DisableTriggeredPlayers();
						return hit2.point;
					}
				}
				else if (_casterState == CasterState.PostCast)
				{
					Vector3 vector = ((!flag) ? (startPosition + direction.normalized * 3000f) : hit2.point);
					_casterState = CasterState.ReverseCast;
					ray2.direction = -direction;
					ray2.origin = vector - direction * 0.0001f;
					castDistance = Vector3.Distance(hit.point, vector);
				}
				else if (_casterState == CasterState.ReverseCast)
				{
					SurfaceType surfaceType2 = SurfaceTypeUtility.FromTag(hit.collider.tag);
					float thickness = ((!flag) ? 0.01f : ((!(hit.collider.gameObject == hit2.collider.gameObject)) ? 0.01f : Vector3.Distance(hit.point, hit2.point)));
					float penetrationLoss = HitSurface.GetPenetrationLoss(surfaceType2, thickness);
					num -= penetrationLoss;
					if (!(num > 0f))
					{
						break;
					}
					if (flag && hit.collider.gameObject == hit2.collider.gameObject)
					{
						DrawHit(hit2, surfaceType2, isLocal);
					}
					_casterState = CasterState.InitialCast;
					ray2.direction = direction;
					ray2.origin = hit.point + direction * 0.0001f;
					castDistance = 3000f - Vector3.Distance(startPosition, hit.point);
				}
			}
			DisableTriggeredPlayers();
			return hit.point;
		}

		private static HitData CreateHitData(PlayerHitbox hitBox, RaycastHit hit, Vector3 direction, BulletParameters parameters, float lossCoeff)
		{
			float num = (float)parameters.GetDamage(hitBox.Bone) * lossCoeff;
			float num2 = num * (100f - parameters.ArmorPenetration) / 100f;
			num -= num2;
			float num3 = parameters.Impulse * lossCoeff;
			if (num3 < 50f)
			{
				num3 = 50f;
			}
			bool penetrated = lossCoeff < 0.99999f;
			HitData hitData = new HitData();
			hitData.Point = hit.point;
			hitData.Direction = direction;
			hitData.WeaponId = parameters.WeaponId;
			hitData.Impulse = num3;
			hitData.Damage = (int)num;
			hitData.ArmorDamage = (int)num2;
			hitData.Bone = hitBox.Bone;
			hitData.Penetrated = penetrated;
			return hitData;
		}

		private static void DisableTriggeredPlayers()
		{
			foreach (PlayerHitController triggeredPlayer in TriggeredPlayerList)
			{
				triggeredPlayer.DisableBoneHit();
			}
			TriggeredPlayerList.Clear();
		}

		private static bool Cast(Ray ray, out RaycastHit hit, float castDistance, int layerMask)
		{
			bool flag = Physics.Raycast(ray, out hit, castDistance, layerMask);
			while (flag && _casterState == CasterState.InitialCast && hit.collider.CompareTag(Tags.PlayerHitTrigger))
			{
				PlayerHitController component = hit.collider.gameObject.transform.parent.GetComponent<PlayerHitController>();
				component.EnableBoneHit();
				flag = Physics.Raycast(ray, out hit, castDistance, layerMask);
				TriggeredPlayerList.Add(component);
			}
			return flag;
		}

		private static void DrawHit(RaycastHit hit, SurfaceType surfaceType, bool isLocal)
		{
			Singleton<SurfaceImpactsEmitter>.Instance.Emit(hit, surfaceType, isLocal);
		}
	}
}
