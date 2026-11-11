using System;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Movement.States
{
	public class GroundMovementState : PlayerTranslationState
	{
		public GroundMovementState(TranslationStatesMap stateMap, MovementController movementController)
			: base(stateMap, movementController)
		{
		}

		protected void GroundTranslationControl()
		{
			Vector2 vector = _translationData.targetRelativeDirection - _translationData.currentRelativeDirection.actual;
			float magnitude = vector.magnitude;
			vector.Normalize();
			Vector2 vector2 = vector * _translationParameters.moveDirectionChangeSpeed * base.DeltaTime;
			if (vector2.magnitude > magnitude)
			{
				_translationData.currentRelativeDirection.actual = _translationData.targetRelativeDirection;
			}
			else
			{
				_translationData.currentRelativeDirection.actual += vector2;
			}
			_translationData.targetSpeed = _translationParameters.speedDefault * _translationData.currentSpeedMultiplier;
			_translationData.moveDirection = _movementController.MovementDirector.TransformDirection(_translationData.currentRelativeDirection3.actual);
			_translationData.targetDeltaPosition += _translationData.moveDirection * _translationData.targetSpeed * base.DeltaTime;
			float num = Mathf.Min(_translationData.currentSpeedY, 0f);
			num -= 10f * base.DeltaTime;
			_translationData.targetDeltaPosition.y += num * base.DeltaTime;
			_translationData.targetDeltaPosition *= _translationData.WeaponSpeedMult;
			if (!_characterController.isGrounded && !_movementController.CharacterControllerTrigger.collides)
			{
				Vector3 startPos = _movementController.MovementDirector.TransformPoint(_characterController.center) + Vector3.down * _characterController.height * 0.3f;
				RaycastHit hit;
				if (!Raycaster.Raycast(startPos, Vector3.down, 1.3f, out hit, null))
				{
					_statesMap.SetState(TranslationStatesEnum.Jump);
					((JumpState)_statesMap.GetState(TranslationStatesEnum.Jump)).SetSubState(JumpState.JumpSubState.Landing);
					return;
				}
			}
			if (base.cmdParameters.isToJump)
			{
				_statesMap.SetState(TranslationStatesEnum.Jump);
				((JumpState)_statesMap.GetState(TranslationStatesEnum.Jump)).SetSubState(JumpState.JumpSubState.Starting);
			}
		}

		protected void MecanimMovementControl()
		{
			Vector3 forward = _movementController.characterTransform.forward;
			Vector3 velocity = _translationData.velocity;
			Vector3 b = new Vector3(velocity.x, 0f, velocity.z);
			float num = 0f - VectorAngle.AngleDirected(forward, b, _movementController.transform.up);
			float f = num / 180f * (float)Math.PI;
			Vector2 vector = new Vector2(Mathf.Sin(f), Mathf.Cos(f));
			float num2 = b.magnitude / _translationParameters.speedDefault;
			vector *= num2;
			Vector2 vector2 = vector - _translationData.currentRelativeDirectionMecanim;
			_translationData.currentRelativeDirectionMecanim = Vector2.Lerp(_translationData.currentRelativeDirectionMecanim, vector, base.DeltaTime * 10f);
			vector2.Normalize();
		}

		public virtual void RotationAppliying()
		{
		}
	}
}
