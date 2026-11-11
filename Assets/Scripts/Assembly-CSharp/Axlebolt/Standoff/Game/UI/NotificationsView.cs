using System.Collections;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI
{
	public class NotificationsView : View
	{
		[SerializeField]
		[NotNull]
		private NotificationView _information;

		[SerializeField]
		[NotNull]
		private NotificationView _warning;

		private IEnumerator _informationCoroutine;

		private IEnumerator _warningCoroutine;

		public NotificationView Info(string title, string text, float duration = -1f)
		{
			_information.Title = title;
			_information.Description = text;
			_information.Show();
			if (_informationCoroutine != null)
			{
				StopCoroutine(_informationCoroutine);
				_informationCoroutine = null;
			}
			if (duration > 0f)
			{
				_informationCoroutine = HideInformationAfter(duration);
				StartCoroutine(_informationCoroutine);
			}
			return _information;
		}

		private IEnumerator HideInformationAfter(float duration)
		{
			yield return new WaitForSeconds(duration);
			HideInfo();
		}

		public void HideInfo()
		{
			_information.Hide();
		}

		public void Warn(string title, string text, float duration = -1f)
		{
			_warning.Title = title;
			_warning.Description = text;
			_warning.Show();
			if (_warningCoroutine != null)
			{
				StopCoroutine(_warningCoroutine);
				_warningCoroutine = null;
			}
			if (duration > 0f)
			{
				_warningCoroutine = HideWarningAfter(duration);
				StartCoroutine(_warningCoroutine);
			}
		}

		public void HideWarning()
		{
			_warning.Hide();
		}

		private IEnumerator HideWarningAfter(float duration)
		{
			yield return new WaitForSeconds(duration);
			HideWarning();
		}

		public override void Hide()
		{
			_information.Hide();
			_warning.Hide();
		}
	}
}
