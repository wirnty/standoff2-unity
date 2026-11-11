using System;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player.Inventory;
using Axlebolt.Standoff.Player.Mecanim;
using Axlebolt.Standoff.Player.Movement.States;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Movement
{
	[RequireComponent(typeof(PlayerController))]
	[RequireComponent(typeof(MecanimController))]
	public class MovementController : BaseController<MovementSnapshot, MovementControllerCmd>, IInventoryEvents
	{
		public delegate void MoveEvent(MovementController moveModule);

		public delegate void MoveLandEvent(MovementController moveModule, float fallDuration);

		private PlayerController _playerController;

		private MovementControllerCmd _cmdParameters = new MovementControllerCmd();

		private float _deltaTime = 1f;

		private TranslationStatesMap _statesMap;

		public PlayerTranslationParameters translationParameters;

		public PlayerTranslationData translationData = new PlayerTranslationData();

		private MovementSnapshot _syncTranslationData = new MovementSnapshot();

		[HideInInspector]
		public Transform characterTransform;

		public Transform MovementDirector { get; private set; }

		public MovementControllerCmd CmdParameters
		{
			get
			{
				return _cmdParameters;
			}
		}

		public float DeltaTime
		{
			get
			{
				return _deltaTime;
			}
		}

		public float LocalTime { get; private set; }

		public CharacterController CharacterController { get; private set; }

		public Trigger CharacterControllerTrigger { get; private set; }

		public IMovementEvents[] MovementEventsImpls { get; private set; }

		public MecanimController MecanimController { get; private set; }

		public Vector3 Velocity
		{
			get
			{
				if (translationData == null)
				{
					Debug.LogError("MovementController Not Initialized");
				}
				return translationData.velocity;
			}
		}

		public event MoveEvent OnMovementInit = delegate
		{
		};

		public event MoveEvent OnJump = delegate
		{
		};

		public event MoveLandEvent OnLand = delegate
		{
		};

		public event MoveEvent OnCrouch = delegate
		{
		};

		public event MoveEvent OnStand = delegate
		{
		};

		public MovementController()
		{
			LocalTime = 0f;
		}

		private void InitializeParameters()
		{
			CurvesContainer.RegisterCurve(translationParameters.crouchParameters.standToCrouchCurve);
			CurvesContainer.RegisterCurve(translationParameters.jumpParameters.jumpCurve);
			CurvesContainer.RegisterCurve(translationParameters.jumpParameters.landCurve);
			CurvesContainer.RegisterCurve(translationParameters.jumpParameters.minSpeedCurve);
			CurvesContainer.RegisterCurve(translationParameters.generalCurveTypes.curveLinear01);
			CurvesContainer.RegisterCurve(translationParameters.generalCurveTypes.curveLinear11);
			translationData.jumpTypeCoeff.actual = 0f;
			translationData.standTypeCoeff.actual = 1f;
		}

		public override void PreInitialize()
		{
			InitializeParameters();
			_playerController = GetComponent<PlayerController>();
			_playerController.CharacterSkinSetEvent += CharacterSkinSet;
			MecanimController = GetComponent<MecanimController>();
			MovementDirector = base.transform;
			MovementDirector.position = base.transform.position;
			MovementDirector.rotation = base.transform.rotation;
			CharacterController = GetComponent<CharacterController>();
			CharacterController.center = GetComponent<CharacterController>().center;
			CharacterController.radius = GetComponent<CharacterController>().radius;
			CharacterController.height = GetComponent<CharacterController>().height;
			CharacterControllerTrigger = new GameObject().AddComponent<Trigger>();
			CharacterControllerTrigger.transform.SetParent(MovementDirector);
			CharacterControllerTrigger.transform.localPosition = Vector3.zero;
			CharacterControllerTrigger.transform.localScale = new Vector3(1f, 1f, 1f);
			CharacterControllerTrigger.transform.localEulerAngles = Vector3.zero;
			CharacterControllerTrigger.gameObject.name = CharacterControllerTrigger.ToString();
			CapsuleCollider capsuleCollider = CharacterControllerTrigger.gameObject.AddComponent<CapsuleCollider>();
			capsuleCollider.height = CharacterController.height;
			capsuleCollider.radius = CharacterController.radius;
			capsuleCollider.center = CharacterController.center + Vector3.down * 0.2f;
			capsuleCollider.isTrigger = true;
			characterTransform = _playerController.CharacterGO.transform;
			MovementEventsImpls = GetComponents<IMovementEvents>();
			translationData.prevPosition = (translationData.currentPosition = CharacterController.transform.position);
			IMovementEvents[] movementEventsImpls = MovementEventsImpls;
			foreach (IMovementEvents movementEvents in movementEventsImpls)
			{
				OnCrouch += movementEvents.OnCrouch;
				OnJump += movementEvents.OnJump;
				OnStand += movementEvents.OnStand;
				OnLand += movementEvents.OnLand;
				OnMovementInit += movementEvents.OnMovementInit;
			}
			this.OnMovementInit(this);
			this.OnStand(this);
			_statesMap = new TranslationStatesMap(this);
		}

		public override void Initialize()
		{
			translationData.Reset();
			translationData.prevPosition = (translationData.currentPosition = CharacterController.transform.position);
			_statesMap.Initialize();
			_statesMap.SetState(TranslationStatesEnum.Walk);
		}

		public void CharacterSkinSet(BipedMap bipedMap)
		{
			characterTransform = bipedMap.transform;
		}

		private void MovementTranslationControl()
		{
			float horizontal = CmdParameters.horizontal;
			float vertical = CmdParameters.vertical;
			Vector2 targetRelativeDirection = new Vector2(horizontal, vertical);
			if (targetRelativeDirection.magnitude < 0.3f)
			{
			}
			if (targetRelativeDirection.magnitude > 1f)
			{
				targetRelativeDirection.Normalize();
			}
			translationData.targetRelativeDirection = targetRelativeDirection;
			_statesMap.UpdateState();
			CharacterController.Move(translationData.targetDeltaPosition);
			MecanimController.SetMoveDir(new Vector2(translationData.currentRelativeDirectionMecanim.x, translationData.currentRelativeDirectionMecanim.y));
			MecanimController.SetJumpCoeff(translationData.jumpTypeCoeff.actual);
			MecanimController.SetStandCoeff(translationData.standTypeCoeff.actual);
			MecanimController.SetTargetDirectionMagnitude(translationData.targetMecanimDirectionMagnitude);
			translationData.prevDeltaTime = DeltaTime;
			translationData.prevPosition = translationData.currentPosition;
			translationData.currentPosition = CharacterController.transform.position;
			translationData.prevDeltaPosition = translationData.targetDeltaPosition;
		}

		public override void SetSnapshot(MovementSnapshot param)
		{
			_syncTranslationData = param;
			translationData.currentPosition = param.currentPosition;
			characterTransform.localEulerAngles = param.characterRotation;
			CharacterController.transform.position = translationData.currentPosition;
			translationData.velocity = param.velocity;
			translationData.IsVelocitySynchronized = true;
		}

		public override MovementSnapshot GetSnapshot()
		{
			_syncTranslationData.currentPosition = translationData.currentPosition;
			_syncTranslationData.characterRotation = characterTransform.localEulerAngles;
			_syncTranslationData.velocity = translationData.velocity;
			return _syncTranslationData;
		}

		public override void ExecuteCommands(MovementControllerCmd commands, float duration, float time)
		{
			_cmdParameters = commands;
			_deltaTime = duration;
			LocalTime = time;
			MovementTranslationControl();
			_statesMap.LateUpdateState();
			translationData.IsVelocitySynchronized = false;
		}

		public void OnCharacterSkinChanged(BipedMap bipedMap)
		{
			throw new NotImplementedException();
		}

		public void OnViewModeChanged(ViewMode viewMode)
		{
		}

		public void OnWeaponSet(WeaponController weapon)
		{
			translationData.WeaponSpeedMult = (float)weapon.WeaponParameters.MovementRate / 250f;
		}

		public void OnNewWeaponSet(WeaponController weapon)
		{
			throw new NotImplementedException();
		}

		public void OnDropWeapon(WeaponController weapon)
		{
			throw new NotImplementedException();
		}
	}
}
