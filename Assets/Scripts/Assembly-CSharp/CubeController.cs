using Axlebolt.Standoff.TouchControl;
using UnityEngine;

public class CubeController : MonoBehaviour
{
	public FloatingJoystick joystick;

	public AimingArea aimingArea;

	public float speed = 5f;

	public Transform camTransform;

	private Vector2 aimingAxis = Vector2.zero;

	private void Start()
	{
	}

	private void Update()
	{
		Vector2 vector = joystick.joystickAxis;
		Vector3 vector2 = base.transform.right * vector.x;
		Vector3 vector3 = base.transform.forward * vector.y;
		base.transform.position += (vector2 + vector3) * speed * Time.deltaTime;
		Vector3 eulerAngles = base.transform.eulerAngles;
		eulerAngles.x = camTransform.localEulerAngles.x;
		eulerAngles.x += 0f - aimingAxis.y;
		eulerAngles.y += aimingAxis.x;
		if (Mathf.Abs(eulerAngles.x) > 180f)
		{
			eulerAngles.x += 360f * (0f - Mathf.Sign(eulerAngles.x));
		}
		eulerAngles.x = Mathf.Clamp(eulerAngles.x, -80f, 80f);
		camTransform.localEulerAngles = new Vector3(eulerAngles.x, 0f, 0f);
		base.transform.eulerAngles = new Vector3(0f, eulerAngles.y, 0f);
	}
}
