using System.Collections.Generic;
using Axlebolt.Standoff.Settings;
using UnityEngine;

namespace Axlebolt.Standoff.TouchControl
{
	public class AimingArea : InteractableZone
	{
		public AnimationCurve accelerationCurve;

		public float accelerationMultiplier;

		public float accelerationBase;

		public float heightToWidthAccelerationRatio;

		public float referenceSensitivity;

		private Vector3 prevTouchPos = Vector3.zero;

		private Vector3 curTouchPos = Vector3.zero;

		private readonly List<float> _bufferedTime = new List<float>();

		private readonly List<float> _bufferedLinearSpeedHor = new List<float>();

		private readonly List<float> _bufferedLinearSpeedVer = new List<float>();

		private float _linearSpeedHor;

		private float _linearSpeedVer;

		private Vector2 _aimingAxis = Vector2.zero;

		public Vector2 AimingAxis
		{
			get
			{
				if (interactiveArea == null)
				{
					return Vector2.zero;
				}
				float num = interactiveArea.rect.yMax - interactiveArea.rect.yMin;
				Vector3 vector = (curTouchPos - prevTouchPos) / num;
				if (_bufferedTime.Count > 2)
				{
					_bufferedTime.RemoveAt(0);
					_bufferedLinearSpeedHor.RemoveAt(0);
					_bufferedLinearSpeedVer.RemoveAt(0);
				}
				_bufferedTime.Add(Time.deltaTime);
				_bufferedLinearSpeedHor.Add(Mathf.Abs(vector.x));
				_bufferedLinearSpeedVer.Add(Mathf.Abs(vector.y));
				float num2 = 0f;
				_linearSpeedHor = 0f;
				_linearSpeedVer = 0f;
				for (int i = 0; i < _bufferedTime.Count; i++)
				{
					num2 += _bufferedTime[i];
					_linearSpeedHor += _bufferedLinearSpeedHor[i];
					_linearSpeedVer += _bufferedLinearSpeedVer[i];
				}
				_linearSpeedHor = Mathf.Lerp(_linearSpeedHor, Mathf.Abs(vector.x) / Time.deltaTime, 1f);
				_linearSpeedVer = Mathf.Lerp(_linearSpeedVer, Mathf.Abs(vector.y) / Time.deltaTime, 1f);
				ControlSettings model = ControlSettingsManager.Instance.Model;
				float acceleration = model.Acceleration;
				float sensitivity = model.Sensitivity;
				float x = vector.x * referenceSensitivity * (accelerationBase + accelerationCurve.Evaluate(_linearSpeedHor) * accelerationMultiplier * acceleration) * heightToWidthAccelerationRatio * sensitivity;
				float y = vector.y * referenceSensitivity * (accelerationBase + accelerationCurve.Evaluate(_linearSpeedVer) * accelerationMultiplier * acceleration) * (1f - heightToWidthAccelerationRatio) * sensitivity;
				_aimingAxis = Vector2.Lerp(_aimingAxis, new Vector2(x, y), Time.deltaTime * 20f);
				return _aimingAxis;
			}
		}

		public override void OnTouchDown(TouchData touchData)
		{
			Vector3 positionConverted = touchData.positionConverted;
			prevTouchPos = (curTouchPos = positionConverted);
			_bufferedTime.Clear();
			_bufferedLinearSpeedVer.Clear();
			_bufferedLinearSpeedHor.Clear();
			_linearSpeedHor = (_linearSpeedVer = 0f);
		}

		public override void OnTouchStayActive(TouchData touchData)
		{
			prevTouchPos = curTouchPos;
			curTouchPos = touchData.positionConverted;
		}

		public override void OnTouchEnd(TouchData touchData)
		{
			prevTouchPos = (curTouchPos = Vector3.zero);
		}
	}
}
