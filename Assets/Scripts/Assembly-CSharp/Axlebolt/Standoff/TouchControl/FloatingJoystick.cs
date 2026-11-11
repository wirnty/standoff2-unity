using UnityEngine;

namespace Axlebolt.Standoff.TouchControl
{
	public class FloatingJoystick : InteractableZone
	{
		public enum TouchState
		{
			TouchFixed = 0,
			NotFixed = 1
		}

		public enum Type
		{
			Fixed = 0,
			Floating = 1,
			FreeTouch = 2
		}

		public Type type = Type.FreeTouch;

		public RectTransform touchTriggerArea;

		public RectTransform joystickCurcleRect;

		public RectTransform joystickPointerRect;

		private float radius;

		private Vector3 triggerAreaCenter;

		private float triggerAreaRadius;

		private bool isInitialized = false;

		private TouchState touchState = TouchState.NotFixed;

		private Vector3 curJoystickPos;

		private Vector3 startJoystickPos;

		private Vector3 curTouchPosition;

		public Vector3 joystickAxis
		{
			get
			{
				if (radius <= 1E-05f)
				{
					return Vector3.zero;
				}
				Vector3 vector = curTouchPosition - curJoystickPos;
				float num = Mathf.Clamp01(vector.magnitude / radius);
				vector.Normalize();
				if (num < 0.3f)
				{
					num = 0f;
				}
				return vector * num;
			}
		}

		public void Initialize()
		{
			float num = joystickCurcleRect.rect.yMax - joystickCurcleRect.rect.yMin;
			float num2 = joystickCurcleRect.rect.xMax - joystickCurcleRect.rect.xMin;
			radius = Mathf.Min(num, num2) * 0.5f;
			Vector3[] array = new Vector3[4];
			touchTriggerArea.GetWorldCorners(array);
			triggerAreaCenter = interactiveArea.InverseTransformPoint((array[0] + array[2]) * 0.5f);
			triggerAreaRadius = Mathf.Min(touchTriggerArea.rect.xMax - touchTriggerArea.rect.xMin, touchTriggerArea.rect.yMax - touchTriggerArea.rect.yMin) * 0.5f;
			if (type == Type.Fixed)
			{
				Vector3 vector = new Vector3(num2 * joystickCurcleRect.pivot.x, num * joystickCurcleRect.pivot.y, 0f);
				Vector3 position = joystickCurcleRect.TransformPoint(new Vector3(num2 * 0.5f, num * 0.5f, 0f) - vector);
				startJoystickPos = interactiveArea.InverseTransformPoint(position);
			}
			isInitialized = true;
		}

		private void SetJoystickCurclePosition(Vector3 position)
		{
			Vector3 localPosition = joystickCurcleRect.parent.InverseTransformPoint(interactiveArea.TransformPoint(position));
			joystickCurcleRect.localPosition = localPosition;
		}

		private void SetJoystickPointerPosition(Vector3 position)
		{
			Vector3 localPosition = joystickPointerRect.parent.InverseTransformPoint(interactiveArea.TransformPoint(position));
			joystickPointerRect.localPosition = localPosition;
		}

		private bool IsInTriggerArea(Vector3 point)
		{
			Vector3[] array = new Vector3[4];
			touchTriggerArea.GetWorldCorners(array);
			Vector3 vector = interactiveArea.InverseTransformPoint(array[0]);
			Vector3 vector2 = interactiveArea.InverseTransformPoint(array[2]);
			return vector.x <= point.x && point.x <= vector2.x && vector.y <= point.y && point.y <= vector2.y;
		}

		public override void OnTouchDown(TouchData touchData)
		{
			if (!isInitialized)
			{
				Initialize();
			}
			Vector3 positionConverted = touchData.positionConverted;
			if (IsInTriggerArea(positionConverted))
			{
				if (type == Type.Floating || type == Type.FreeTouch)
				{
					curJoystickPos = positionConverted;
				}
				if (type == Type.Fixed)
				{
					curJoystickPos = startJoystickPos;
				}
				touchState = TouchState.TouchFixed;
				joystickCurcleRect.gameObject.SetActive(true);
				joystickPointerRect.gameObject.SetActive(true);
				OnTouchStayActive(touchData);
			}
		}

		public override void OnTouchStayActive(TouchData touchData)
		{
			if (touchState != 0)
			{
				return;
			}
			Vector3 positionConverted = touchData.positionConverted;
			Vector3 vector = positionConverted - curJoystickPos;
			Vector3 vector2 = triggerAreaCenter - positionConverted;
			if (type == Type.Floating)
			{
				if (vector2.magnitude < triggerAreaRadius + radius)
				{
					if (vector.magnitude > radius)
					{
						curJoystickPos = positionConverted - vector.normalized * radius;
						Vector3 vector3 = curJoystickPos - triggerAreaCenter;
						if (vector3.magnitude > triggerAreaRadius)
						{
							curJoystickPos = triggerAreaCenter + vector3.normalized * triggerAreaRadius;
						}
					}
				}
				else
				{
					curJoystickPos = triggerAreaCenter - vector2.normalized * triggerAreaRadius;
				}
			}
			if (type == Type.FreeTouch)
			{
			}
			if (type == Type.Fixed)
			{
				curJoystickPos = startJoystickPos;
			}
			curTouchPosition = positionConverted;
			SetJoystickPointerPosition(joystickAxis * radius + curJoystickPos);
			SetJoystickCurclePosition(curJoystickPos);
		}

		public override void OnTouchEnd(TouchData touchData)
		{
			if (touchState == TouchState.TouchFixed)
			{
				touchState = TouchState.NotFixed;
				joystickCurcleRect.gameObject.SetActive(false);
				joystickPointerRect.gameObject.SetActive(false);
				curTouchPosition = (curJoystickPos = Vector3.zero);
			}
		}

		public void SetJouystickType(int type)
		{
			this.type = (Type)type;
		}
	}
}
