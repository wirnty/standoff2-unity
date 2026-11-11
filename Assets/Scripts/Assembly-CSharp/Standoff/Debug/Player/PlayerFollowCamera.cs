using UnityEngine;

namespace StandOff.Debug.Player
{
	public class PlayerFollowCamera : MonoBehaviour
	{
		public enum RotationType
		{
			FixedRotation = 1,
			FreeRotation = 2,
			ThirdPersonControl = 3
		}

		public enum FollowType
		{
			Fixed = 0,
			Lerped = 1
		}

		public RotationType rotationType = RotationType.FixedRotation;

		public FollowType followType = FollowType.Lerped;

		public float followSpeed_l;

		public float distance;

		public float borderRadius;

		public float sensitivityX = 1f;

		public float sensitivityY = 1f;

		private Vector3 _deltaOffsetPos = Vector3.zero;

		private Transform _playerTransform = null;

		private Transform _cameraTransform = null;

		private void Awake()
		{
			_cameraTransform = base.transform;
		}

		public void SetFollowType(int type)
		{
		}

		public void SetCamera(GameObject cameraGO)
		{
			_cameraTransform = cameraGO.transform;
		}

		public void SetPlayerTransform(Transform plTransform)
		{
			_playerTransform = plTransform;
			base.enabled = true;
		}

		private void Start()
		{
		}

		private void Update()
		{
			if (rotationType == RotationType.FixedRotation)
			{
			}
			if (rotationType == RotationType.FreeRotation)
			{
				float num = _cameraTransform.eulerAngles.y + Input.GetAxis("Mouse X") * sensitivityY;
				float num2 = 0f - _cameraTransform.eulerAngles.x;
				if (num < -180f)
				{
					num += 360f;
				}
				num2 += Input.GetAxis("Mouse Y") * sensitivityX;
				if (num2 < -180f)
				{
					num2 += 360f;
				}
				_cameraTransform.eulerAngles = new Vector3(0f - num2, num, 0f);
			}
			if (followType == FollowType.Fixed)
			{
			}
			if (followType == FollowType.Lerped)
			{
				_deltaOffsetPos = Vector3.Lerp(_deltaOffsetPos, _playerTransform.position, Time.deltaTime * followSpeed_l);
				if ((_deltaOffsetPos - _playerTransform.position).magnitude > borderRadius)
				{
					Vector3 vector = _deltaOffsetPos - _playerTransform.position;
					_deltaOffsetPos = _playerTransform.position + vector.normalized * borderRadius;
				}
				Vector3 vector2 = _cameraTransform.forward * distance;
				_cameraTransform.position = _deltaOffsetPos - vector2 + Vector3.up * 2f;
			}
		}
	}
}
