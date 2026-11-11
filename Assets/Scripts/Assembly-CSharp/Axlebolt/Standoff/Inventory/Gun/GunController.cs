using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Axlebolt.Common.States;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Effects;
using Axlebolt.Standoff.Inventory.Animation;
using Axlebolt.Standoff.Inventory.HitHandling;
using Axlebolt.Standoff.Inventory.Weapon;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Player.Inventory;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Gun
{
	[RequireComponent(typeof(WeaponMap))]
	public class GunController : WeaponController
	{
		public enum StateType
		{
			Reloading = 0,
			PreReload = 1,
			Switching = 2,
			TryToSwitch = 3,
			Ready = 4
		}

		public enum AimingMode
		{
			Aiming = 0,
			AimingReload = 1,
			NotAiming = 2
		}

		internal class AudioData
		{
			public List<AudioSource> AudioSources = new List<AudioSource>();

			public AudioClip ShootClip;
		}

		private static readonly Log Log = Log.Create(typeof(GunController));

		private float _fireInterval;

		private float _timeFired;

		private float _lastFiredTime;

		private int _audioSourceIndex;

		private short _curMagazineAmmo;

		private short _restAmmoAmount;

		private RecoilData _recoilData;

		private AudioData _audioData;

		private RecoilControl _recoilControl;

		private GunParameters _gunParameters;

		private float _lastHandledShotTime;

		private GunSnapshot _syncData = new GunSnapshot();

		private readonly StateSimple<StateType> _state = new StateSimple<StateType>();

		private readonly StateSimple<AimingMode> _aimingMode = new StateSimple<AimingMode>();

		private readonly List<ShotData> _shotBuffer = new List<ShotData>();

		private Transform _cameraTransform;

		private Transform _muzzlePoint;

		private readonly float _switchAnimConfirmDuration = 0.5f;

		private readonly float _reloadAnimConfirmDuration = 0.5f;

		private float _accuracyAdditive = 1f;

		private AccuracyData _accuracyData = new AccuracyData();

		public short CurMagazineAmmo
		{
			[CompilerGenerated]
			get
			{
				return _curMagazineAmmo;
			}
		}

		public short RestAmmoAmount
		{
			[CompilerGenerated]
			get
			{
				return _restAmmoAmount;
			}
		}

		public GunParameters GunParameters
		{
			[CompilerGenerated]
			get
			{
				return _gunParameters;
			}
		}

		public AimingMode CurrentAimingMode
		{
			[CompilerGenerated]
			get
			{
				return _aimingMode.curState;
			}
		}

		public override byte SlotIndex
		{
			[CompilerGenerated]
			get
			{
				return (byte)((_gunParameters.GunType == GunType.Pistol) ? 1u : 2u);
			}
		}

		internal override void PreInitialize(WeaponParameters weaponParameters, WeaponAnimationParameters animationParameters)
		{
			if (!(weaponParameters is GunParameters))
			{
				throw new ArgumentException("Invalid weaponParameters, expected Weapon actual is " + weaponParameters);
			}
			base.PreInitialize(weaponParameters, animationParameters);
			_gunParameters = (GunParameters)weaponParameters;
			_fireInterval = 60f / (float)_gunParameters.FireRate;
			_curMagazineAmmo = _gunParameters.Ammunition.MagazineCapacity;
			_restAmmoAmount = _gunParameters.Ammunition.Capacity;
			_timeFired = 0f;
			_recoilControl = new RecoilControl(_gunParameters);
			_audioData = new AudioData
			{
				ShootClip = AnimationParameters.shotSound
			};
			for (int i = 0; i < AnimationParameters.audioSourcesCount; i++)
			{
				AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
				audioSource.playOnAwake = false;
				_audioData.AudioSources.Add(audioSource);
			}
			AnimationController.PreInitialize(this);
			ConfigureMuzzlePoint();
		}

		internal override void Initialize()
		{
			base.Initialize();
			_curMagazineAmmo = _gunParameters.Ammunition.MagazineCapacity;
			_restAmmoAmount = _gunParameters.Ammunition.Capacity;
			_recoilControl = new RecoilControl(_gunParameters);
			_timeFired = 0f;
			_shotBuffer.Clear();
			_aimingMode.SetState(AimingMode.NotAiming, base.LocalTime);
		}

		private void ConfigureMuzzlePoint()
		{
			if (base.WeaponMap.MuzzlePoint == null)
			{
				Log.Error(string.Format("Weapon {0} is not valid, {1} not found", base.gameObject.name, WeaponMap.WeaponPart.MuzzlePoint));
			}
			else
			{
				_muzzlePoint = base.WeaponMap.MuzzlePoint.transform;
			}
		}

		public override void ExecuteCommands(InventoryControllerCmd commands, float duration, float time)
		{
			base.ExecuteCommands(commands, duration, time);
			LocalUpdate();
			if (commands.toFire && _state.curState == StateType.Ready)
			{
				Fire();
			}
			if (commands.toReload && _state.curState == StateType.Ready && _restAmmoAmount > 0 && _curMagazineAmmo < _gunParameters.Ammunition.MagazineCapacity)
			{
				_state.SetState(StateType.PreReload, base.LocalTime);
			}
			LocalPostUpdate();
		}

		public override WeaponSnapshot GetSnapshot()
		{
			_syncData.magazineAmmo = _curMagazineAmmo;
			_syncData.restAmmo = _restAmmoAmount;
			_syncData.shootDataList.Clear();
			_syncData.shootDataList.AddRange(_shotBuffer);
			_shotBuffer.Clear();
			return _syncData;
		}

		public override void SetSnapshot(WeaponSnapshot parameters)
		{
			_syncData = (GunSnapshot)parameters;
			_curMagazineAmmo = _syncData.magazineAmmo;
			_restAmmoAmount = _syncData.restAmmo;
			List<ShotData> unhandledShots = GetUnhandledShots(_syncData.shootDataList, _syncData.time);
			if (unhandledShots.Count <= 0)
			{
				return;
			}
			foreach (ShotData item in unhandledShots)
			{
				Shoot(item.startPos, item.direction, false);
			}
			_lastHandledShotTime = unhandledShots[unhandledShots.Count - 1].shootTime;
		}

		private List<ShotData> GetUnhandledShots(List<ShotData> shootList, float time)
		{
			List<ShotData> list = new List<ShotData>();
			for (int i = 0; i < shootList.Count; i++)
			{
				if (_lastHandledShotTime < shootList[i].shootTime && shootList[i].shootTime < time)
				{
					list.Add(shootList[i]);
				}
			}
			return list;
		}

		private void Reload()
		{
			MecanimController.SetReloading();
		}

		private void PlayShotSound()
		{
			_audioSourceIndex %= _audioData.AudioSources.Count;
			_lastFiredTime = base.LocalTime;
			_audioData.AudioSources[_audioSourceIndex].clip = _audioData.ShootClip;
			_audioData.AudioSources[_audioSourceIndex].Play();
			_audioSourceIndex++;
		}

		private void Fire()
		{
			float num = base.LocalTime - _timeFired;
			if (!(num < _fireInterval) && _curMagazineAmmo > 0)
			{
				_curMagazineAmmo--;
				_recoilData = _recoilControl.GetNextShot(false, base.LocalTime);
				GetRelativeDispertion(ref _recoilData, _accuracyAdditive);
				_timeFired = base.LocalTime;
				MecanimController.SetShooting();
				Vector3 forward = _cameraTransform.forward;
				forward = Quaternion.AngleAxis(_recoilData.YDeviation, _cameraTransform.up) * forward;
				forward = Quaternion.AngleAxis(0f - _recoilData.XDeviation, _cameraTransform.right) * forward;
				Vector3 position = _cameraTransform.position;
				Shoot(position, forward, true);
				ShotData shotData = new ShotData();
				shotData.direction = forward;
				shotData.shootTime = _lastFiredTime;
				shotData.startPos = position;
				_shotBuffer.Add(shotData);
			}
		}

		private void Shoot(Vector3 startPosition, Vector3 direction, bool isLocal)
		{
			PlayShotSound();
			CastBullet(startPosition, direction, isLocal);
			EmitMuzzleFlash(isLocal);
			if (isLocal)
			{
				Singleton<WeaponManager>.Instance.OnShootEvent(this);
			}
		}

		private void CastBullet(Vector3 startPosition, Vector3 direction, bool isLocal)
		{
			BulletParameters parameters = BulletParameters.Create(_gunParameters.Id, _gunParameters.PenetrationPower, _gunParameters.ArmorPenetration, _gunParameters.Impulse, _gunParameters.Damage);
			BulletCaster.CastBullet(startPosition, direction, parameters, isLocal);
		}

		private void EmitMuzzleFlash(bool isLocal)
		{
			Singleton<MuzzleFlashEmitter>.Instance.Emit(_gunParameters.MuzzleFlashType, _muzzlePoint.position, _muzzlePoint.forward, isLocal);
		}

		public override void SetAsDefault(float time)
		{
			base.SetAsDefault(time);
			_state.SetState(StateType.TryToSwitch, base.LocalTime);
			_aimingMode.SetState(AimingMode.NotAiming, base.LocalTime);
		}

		public override void SetPlayer(PlayerController playerController)
		{
			base.SetPlayer(playerController);
			base.transform.SetParent(playerController.BipedMap.RightHand);
			SetViewMode(playerController.AimController.ViewMode);
		}

		private bool IsFiring()
		{
			return base.LocalTime - _timeFired < _fireInterval;
		}

		private void SetAiming()
		{
			PlayerController.FpsCameraHolder.SetActive(false);
		}

		private void SetNotAiming()
		{
			PlayerController.FpsCameraHolder.SetActive(true);
		}

		private void AimingControl()
		{
			if (_gunParameters.SightType != SightType.SniperScope)
			{
				return;
			}
			if (_aimingMode.curState == AimingMode.Aiming)
			{
				if (Commands.toAim)
				{
					_aimingMode.SetState(AimingMode.NotAiming, base.LocalTime);
					SetNotAiming();
				}
				if (IsFiring() || _state.curState != StateType.Ready)
				{
					_aimingMode.SetState(AimingMode.AimingReload, base.LocalTime);
					SetNotAiming();
				}
			}
			else if (_aimingMode.curState == AimingMode.AimingReload)
			{
				if (!IsFiring() && _state.curState == StateType.Ready)
				{
					_aimingMode.SetState(AimingMode.Aiming, base.LocalTime);
					SetAiming();
				}
			}
			else if (_aimingMode.curState == AimingMode.NotAiming && _state.curState == StateType.Ready && !IsFiring() && Commands.toAim)
			{
				_aimingMode.SetState(AimingMode.Aiming, base.LocalTime);
				SetAiming();
			}
		}

		private void AdditiveAccuracyControl()
		{
			float actual = PlayerController.MovementController.translationData.standTypeCoeff.actual;
			float magnitude = PlayerController.MovementController.translationData.velocity.magnitude;
			if (actual > 0.5f)
			{
				_accuracyAdditive = Mathf.Lerp(_accuracyAdditive, _gunParameters.AccuracyAdditiveCurve.Evaluate(magnitude), base.DeltaTime * 20f);
			}
			else
			{
				_accuracyAdditive = Mathf.Lerp(_accuracyAdditive, _gunParameters.AccuracyAdditiveCurve.Evaluate(magnitude) * (actual + 1f) * 0.5f, base.DeltaTime * 20f);
			}
			if (_gunParameters.SightType == SightType.SniperScope && _aimingMode.curState == AimingMode.NotAiming)
			{
				_accuracyAdditive = _gunParameters.AdditiveNoAimDispertionCurve.Evaluate(magnitude);
			}
		}

		private void LocalUpdate()
		{
			AdditiveAccuracyControl();
			if (_state.curState == StateType.PreReload && base.LocalTime - _timeFired > _gunParameters.PreReloadTime)
			{
				Reload();
				_state.SetState(StateType.Reloading, base.LocalTime);
			}
			if (_state.curState == StateType.Reloading)
			{
				if (base.LocalTime - _state.timeSwitched > _gunParameters.ReloadDuration)
				{
					_state.SetState(StateType.Ready, base.LocalTime);
				}
				else if (base.LocalTime - _state.timeSwitched < _reloadAnimConfirmDuration && !MecanimController.IsReloadingWeapon(base.WeaponName))
				{
					MecanimController.SetWeaponNO(base.WeaponId);
					MecanimController.SetReloading();
				}
				if (base.LocalTime - _state.timeSwitched > _gunParameters.MagazineInsertTime)
				{
					_restAmmoAmount += _curMagazineAmmo;
					_curMagazineAmmo = 0;
					if (_restAmmoAmount <= _gunParameters.Ammunition.MagazineCapacity)
					{
						_curMagazineAmmo = _restAmmoAmount;
						_restAmmoAmount = 0;
					}
					else
					{
						_curMagazineAmmo = _gunParameters.Ammunition.MagazineCapacity;
						_restAmmoAmount -= _gunParameters.Ammunition.MagazineCapacity;
					}
				}
				return;
			}
			if (_state.curState == StateType.TryToSwitch)
			{
				_state.SetState(StateType.Switching, base.LocalTime);
				return;
			}
			if (_state.curState == StateType.Switching)
			{
				if (base.LocalTime - _state.timeSwitched > _gunParameters.TakeDuration)
				{
					_state.SetState(StateType.Ready, base.LocalTime);
				}
				else if (base.LocalTime - _state.timeSwitched < _switchAnimConfirmDuration && !MecanimController.IsSwitchingWeapon(base.WeaponName))
				{
					MecanimController.SwitchWeapon(base.WeaponId);
				}
			}
			if (_state.curState == StateType.Ready && _curMagazineAmmo <= 0 && _restAmmoAmount > 0)
			{
				_state.SetState(StateType.PreReload, base.LocalTime);
			}
			if (_gunParameters.SightType == SightType.SniperScope || _gunParameters.SightType == SightType.CollimatorSight)
			{
				AimingControl();
			}
		}

		private void GetRelativeDispertion(ref RecoilData recoilData, float accuracyAdditive)
		{
			float max = 1f;
			float angle = UnityEngine.Random.Range(0f, 360f);
			Vector3 vector = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
			vector *= UnityEngine.Random.Range(0f, max);
			vector *= recoilData.AngleDispertion + accuracyAdditive;
			recoilData.XDeviation += vector.x;
			recoilData.YDeviation += vector.y;
		}

		public override AccuracyData GetAccuracyData()
		{
			return _accuracyData;
		}

		private void LocalPostUpdate()
		{
			_cameraTransform = PlayerController.MainCameraHolder.transform;
			_cameraTransform.localPosition = Vector3.zero;
			_cameraTransform.localRotation = Quaternion.identity;
			RecoilData currentDeviation = _recoilControl.GetCurrentDeviation(base.LocalTime);
			_accuracyData.RecoilAngle = Mathf.Max(currentDeviation.XDeviation, currentDeviation.YDeviation);
			_accuracyData.AccuracyAngle = currentDeviation.AngleDispertion + _accuracyAdditive;
			_cameraTransform.localRotation *= Quaternion.AngleAxis(currentDeviation.YDeviation * _gunParameters.RecoilParameters.cameraDeviationCoeff, Vector3.up);
			_cameraTransform.localRotation *= Quaternion.AngleAxis((0f - currentDeviation.XDeviation) * _gunParameters.RecoilParameters.cameraDeviationCoeff, Vector3.right);
			_cameraTransform.localEulerAngles = new Vector3(_cameraTransform.localEulerAngles.x, _cameraTransform.localEulerAngles.y, 0f);
		}

		private void MeshCombineSplitTracing()
		{
			if (MecanimController.IsIdlingWeapon(base.WeaponName))
			{
				base.LodGroup.CombineMesh();
				if (!base.LodGroup.Combined)
				{
					base.LodGroup.CombineMesh();
				}
			}
			else
			{
				base.LodGroup.SplitMesh();
				if (base.LodGroup.Combined)
				{
					base.LodGroup.SplitMesh();
				}
			}
		}

		internal override void SetViewMode(ViewMode viewMode)
		{
			base.SetViewMode(viewMode);
			if (viewMode == ViewMode.FPS)
			{
				foreach (AudioSource audioSource in _audioData.AudioSources)
				{
					audioSource.spatialBlend = 0f;
				}
				AnimationAudioSource.spatialBlend = 0f;
			}
			if (viewMode != ViewMode.TPS)
			{
				return;
			}
			foreach (AudioSource audioSource2 in _audioData.AudioSources)
			{
				audioSource2.spatialBlend = 1f;
			}
			AnimationAudioSource.spatialBlend = 1f;
		}

		public override void OnAnimatorUpdated()
		{
			AnimatorStateInfo currentCharacterWeaponAnimationState = GetCurrentCharacterWeaponAnimationState();
			AnimationController.Play(currentCharacterWeaponAnimationState.shortNameHash, currentCharacterWeaponAnimationState.normalizedTime, Time.deltaTime);
			MeshCombineSplitTracing();
		}
	}
}
