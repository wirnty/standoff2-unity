using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.HUD
{
	public class HitIndicatorView : View
	{
		[SerializeField]
		[NotNull]
		private Image _front;

		[SerializeField]
		[NotNull]
		private Image _right;

		[NotNull]
		[SerializeField]
		private Image _back;

		[NotNull]
		[SerializeField]
		private Image _left;

		private PlayerController _playerController;

		public PlayerController PlayerController
		{
			set
			{
				if (_playerController != null)
				{
					_playerController.HitController.OnHit = null;
				}
				_playerController = value;
				if (_playerController != null)
				{
					_playerController.HitController.OnHit = OnHit;
				}
			}
		}

		private void OnEnable()
		{
			_front.gameObject.SetActive(false);
			_right.gameObject.SetActive(false);
			_back.gameObject.SetActive(false);
			_left.gameObject.SetActive(false);
		}

		private void OnHit(HitData hitData)
		{
			Image indicator = GetIndicator(hitData);
			indicator.gameObject.SetActive(false);
			indicator.gameObject.SetActive(true);
		}

		private Image GetIndicator(HitData hitData)
		{
			Vector3 forward = _playerController.transform.forward;
			forward = new Vector3(forward.x, 0f, forward.z);
			Vector3 vector = -hitData.Direction;
			vector = new Vector3(vector.x, 0f, vector.z);
			float num = VectorAngle.AngleDirected(vector, forward, Vector3.up);
			if (num > -45f && num <= 45f)
			{
				return _front;
			}
			if (num > 45f && num <= 135f)
			{
				return _right;
			}
			if (num > 135f || num < -135f)
			{
				return _back;
			}
			return _left;
		}
	}
}
