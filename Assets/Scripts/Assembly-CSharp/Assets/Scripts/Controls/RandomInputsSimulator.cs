using Axlebolt.Standoff.Player;
using UnityEngine;

namespace Assets.Scripts.Controls
{
	public class RandomInputsSimulator : MonoBehaviour
	{
		[SerializeField]
		private float _aimAngleChangeDuration;

		[SerializeField]
		private float _aimAngleLerpSpeed;

		[SerializeField]
		private float _moveDirectionChangeDuration;

		[SerializeField]
		private float _moveDirectionLerpSpeed;

		private float _aimAngleChangedTime;

		private float _moveDiractionChangedTime;

		private Vector3 _targetAimAngles;

		private Vector2 _targetMoveDirection;

		private Vector3 _currentAimAngles;

		private Vector3 _prevAimAngles;

		private Vector3 _currentMoveDirection;

		private void Start()
		{
			_aimAngleChangedTime = Time.time;
			_moveDiractionChangedTime = Time.time;
		}

		private void SimulateAimAngles()
		{
			_prevAimAngles = _currentAimAngles;
			_currentAimAngles = Vector3.Lerp(_currentAimAngles, _targetAimAngles, Time.deltaTime * 10f);
		}

		private void SimulateMoveDirection()
		{
			_currentMoveDirection = Vector2.Lerp(_currentMoveDirection, _targetMoveDirection, Time.deltaTime * 5f);
		}

		private Vector2 GetAimAxis()
		{
			return new Vector2(_currentAimAngles.x - _prevAimAngles.x, _currentAimAngles.y - _prevAimAngles.y);
		}

		private Vector2 GetMoveDirection()
		{
			return _currentMoveDirection.normalized;
		}

		public PlayerInputs GetPlayerInputs()
		{
			PlayerInputs playerInputs = new PlayerInputs();
			playerInputs.DeltaAimAngles.x = GetAimAxis().x;
			playerInputs.DeltaAimAngles.y = GetAimAxis().y;
			playerInputs.Horizontal = GetMoveDirection().x;
			playerInputs.Vertical = GetMoveDirection().y;
			return playerInputs;
		}

		private void Update()
		{
			SimulateAimAngles();
			if (Time.time - _aimAngleChangedTime > _aimAngleChangeDuration)
			{
				_aimAngleChangedTime = Time.time;
				_targetAimAngles = new Vector3(Random.Range(-30, 30), Random.Range(0, 360), 0f);
			}
			SimulateMoveDirection();
			if (Time.time - _moveDiractionChangedTime > _moveDirectionChangeDuration)
			{
				_moveDiractionChangedTime = Time.time;
				_targetMoveDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
			}
		}
	}
}
