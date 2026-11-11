using UnityEngine;

namespace Axlebolt.Standoff.Player.Mecanim
{
	public class MecanimController : BaseController<MecanimSnapshot, MecanimControllerCmd>
	{
		public delegate void AnimatorUpdateCall();

		[HideInInspector]
		private PlayerController _controlModule;

		public Animator animator;

		public RuntimeAnimatorController runtimeAnimatorController;

		public PlayerMecanimConfig config;

		public MecanimSync mecanimSync;

		private MecanimExecutionType executionType = MecanimExecutionType.GameLoop;

		private IMecanimEvents[] _mecanimEventsImpl;

		private MecanimSnapshot syncParameters = new MecanimSnapshot();

		public MecanimExecutionType ExecutionType
		{
			get
			{
				return executionType;
			}
			set
			{
				executionType = value;
				if (executionType == MecanimExecutionType.OnDemand)
				{
					animator.enabled = false;
				}
				if (executionType == MecanimExecutionType.GameLoop)
				{
					animator.enabled = true;
				}
			}
		}

		public event AnimatorUpdateCall onAnimatorUpdated = delegate
		{
		};

		public override void PreInitialize()
		{
			_controlModule = GetComponent<PlayerController>();
			_mecanimEventsImpl = GetComponents<IMecanimEvents>();
			_controlModule.CharacterSkinSetEvent += CharacterSkinSet;
			animator = GetAnimator(_controlModule.BipedMap);
			mecanimSync = new MecanimSync();
			mecanimSync.PreInitialize(animator, config);
		}

		public override void Initialize()
		{
			animator.runtimeAnimatorController = config.animatorController;
			animator.Update(0f);
		}

		public override void ExecuteCommands(MecanimControllerCmd commands, float duration, float time)
		{
			mecanimSync.EvaluateTimePoints(time);
			if (executionType == MecanimExecutionType.OnDemand)
			{
				animator.Update(duration);
				this.onAnimatorUpdated();
			}
		}

		public override MecanimSnapshot GetSnapshot()
		{
			syncParameters.mecanimSyncData = mecanimSync.GetAnimatorSnapshot(true);
			return syncParameters;
		}

		public override void SetSnapshot(MecanimSnapshot parameters)
		{
			syncParameters = parameters;
			mecanimSync.SetAnimatorSnapshot(parameters.mecanimSyncData, true);
		}

		private void LateUpdate()
		{
			if (executionType == MecanimExecutionType.GameLoop)
			{
				this.onAnimatorUpdated();
			}
		}

		public void CharacterSkinSet(BipedMap bipedMap)
		{
			MecanimSyncData animatorSnapshot = mecanimSync.GetAnimatorSnapshot(false);
			animator = GetAnimator(bipedMap);
			mecanimSync.Initialize(animator);
			mecanimSync.SetAnimatorSnapshot(animatorSnapshot, false);
			animator.Update(0f);
		}

		private Animator GetAnimator(BipedMap bipedMap)
		{
			Animator animator = bipedMap.GetComponent<Animator>();
			if (animator == null)
			{
				animator = bipedMap.gameObject.AddComponent<Animator>();
				animator.avatar = config.avatar;
				animator.runtimeAnimatorController = config.animatorController;
			}
			return animator;
		}

		public void ResetAnimatorController()
		{
			OnAnimatorResetEvent();
		}

		public void SwitchWeapon(int weaponId)
		{
			animator.SetInteger("WeaponNO", weaponId);
			animator.SetTrigger("TakeWeapon");
		}

		public void SetWeaponIdleNO(int weaponId)
		{
			animator.SetFloat("WeaponIdleNO", weaponId);
		}

		public void SetWeaponNO(int weaponId)
		{
			animator.SetInteger("WeaponNO", weaponId);
		}

		public void SetReloading()
		{
			animator.SetTrigger("Reload");
		}

		public void SetShooting()
		{
			animator.SetTrigger("Shoot");
		}

		public void SetMoveDir(Vector2 dir)
		{
			animator.SetFloat("Horizontal", dir.x);
			animator.SetFloat("Vertical", dir.y);
			animator.SetFloat("Magnitude", dir.magnitude);
		}

		public void SetTargetDirectionMagnitude(float magn)
		{
			animator.SetFloat("TargetDirMagn", magn);
		}

		public void SetJumpCoeff(float jumpCoeff)
		{
			animator.SetFloat("JumpCoeff", jumpCoeff);
		}

		public void SetStandCoeff(float coeff)
		{
			animator.SetFloat("StandType", coeff);
		}

		public void SetArmsOffsetCoeff(float armsOffsetCoeff)
		{
			animator.SetFloat("ArmsOffset", armsOffsetCoeff);
		}

		public void SetCrouchIdle(bool state)
		{
			animator.SetBool("ToCrouchIdle", state);
		}

		public void SetRotating(float ang)
		{
			animator.SetTrigger("ToRotate");
			animator.SetFloat("RotAngle", ang);
		}

		public void SetFPSViewMode()
		{
			animator.SetFloat("ViewType", 0f);
			animator.Update(0f);
		}

		public void SetTPSViewMode()
		{
			animator.SetFloat("ViewType", 1f);
			animator.Update(0f);
		}

		public bool IsSwitchingWeapon(string weaponName)
		{
			if (!animator.IsInTransition(1))
			{
				return animator.GetCurrentAnimatorStateInfo(1).IsName("Take_" + weaponName);
			}
			return animator.GetNextAnimatorStateInfo(1).IsName("Take_" + weaponName);
		}

		public bool IsReloadingWeapon(string weaponName)
		{
			if (!animator.IsInTransition(1))
			{
				return animator.GetCurrentAnimatorStateInfo(1).IsName("Reload_" + weaponName);
			}
			return animator.GetNextAnimatorStateInfo(1).IsName("Reload_" + weaponName);
		}

		public bool IsStartingWeaponSwitch(string weaponName)
		{
			if (animator.IsInTransition(1) && animator.GetNextAnimatorStateInfo(1).IsName("Take_" + weaponName))
			{
				return true;
			}
			return false;
		}

		public bool IsHoldingWeapon(string weaponName)
		{
			AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(1);
			if (currentAnimatorStateInfo.IsName("Idle_" + weaponName) || currentAnimatorStateInfo.IsName("Reload_" + weaponName) || currentAnimatorStateInfo.IsName("Shoot_" + weaponName) || currentAnimatorStateInfo.IsName("Take_" + weaponName))
			{
				return true;
			}
			return false;
		}

		public bool IsIdlingWeapon(string weaponName)
		{
			AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(1);
			if (!animator.IsInTransition(1) && currentAnimatorStateInfo.IsName("Idle_" + weaponName))
			{
				return true;
			}
			return false;
		}

		public AnimatorStateInfo GetAnimatorStateInfo(int layer)
		{
			return animator.GetCurrentAnimatorStateInfo(layer);
		}

		public float GetWalkAnimationProgress()
		{
			return animator.GetFloat("IsRU");
		}

		public float GetLFootParameter()
		{
			return animator.GetFloat("LFoot");
		}

		private void OnAnimatorResetEvent()
		{
			IMecanimEvents[] mecanimEventsImpl = _mecanimEventsImpl;
			foreach (IMecanimEvents mecanimEvents in mecanimEventsImpl)
			{
				if (mecanimEvents != null)
				{
					mecanimEvents.OnAnimatorReset();
				}
			}
		}
	}
}
