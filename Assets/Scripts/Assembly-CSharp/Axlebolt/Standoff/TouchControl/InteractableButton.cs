using UnityEngine;

namespace Axlebolt.Standoff.TouchControl
{
	public class InteractableButton : InteractableZone
	{
		public delegate void OnButtonClickAction();

		public float TriggerFixTime;

		public float MaxTriggerFixDeviation;

		private float _touchDownTime;

		private Vector3 _touchPosition;

		private bool _isTriggerFixed = false;

		private bool _buttonUpTriggered = false;

		private bool _isClicked = false;

		public bool IsButtonDown
		{
			get
			{
				if (_buttonUpTriggered)
				{
					_buttonUpTriggered = false;
					return true;
				}
				return _isTriggerFixed;
			}
		}

		public bool IsClicked
		{
			get
			{
				return _isClicked;
			}
			set
			{
				_isClicked = value;
			}
		}

		public event OnButtonClickAction onButtonDownEvent = delegate
		{
		};

		public event OnButtonClickAction onButtonStayActiveEvent = delegate
		{
		};

		public event OnButtonClickAction onButtonUpEvent = delegate
		{
		};

		public override void OnTouchDown(TouchData touchData)
		{
			_isClicked = false;
			_touchDownTime = Time.time;
			_touchPosition = touchData.positionConverted;
			_isTriggerFixed = (_buttonUpTriggered = false);
		}

		public override void OnTouchStayActive(TouchData touchData)
		{
			_isClicked = false;
			if (!_isTriggerFixed && Time.time - _touchDownTime > TriggerFixTime)
			{
				float num = Vector3.Distance(touchData.positionConverted, _touchPosition);
				if (num < MaxTriggerFixDeviation)
				{
					_isTriggerFixed = true;
					_isClicked = true;
					this.onButtonDownEvent();
				}
			}
			if (_isTriggerFixed)
			{
				this.onButtonStayActiveEvent();
			}
		}

		public override void OnTouchEnd(TouchData touchData)
		{
			if (!_isTriggerFixed && Time.time - _touchDownTime < 0.1f)
			{
				float num = Vector3.Distance(touchData.positionConverted, _touchPosition);
				if (num < MaxTriggerFixDeviation)
				{
					_buttonUpTriggered = true;
				}
			}
			if (_isTriggerFixed)
			{
				this.onButtonUpEvent();
			}
			_isTriggerFixed = false;
		}
	}
}
