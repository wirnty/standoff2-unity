using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.TouchControl
{
	public class ClingingButton : MonoBehaviour
	{
		private bool _isClicked;

		[SerializeField]
		private Image _clickedImage;

		[SerializeField]
		private Image _unclickedImage;

		public bool IsClicked
		{
			get
			{
				return _isClicked;
			}
			set
			{
				_isClicked = false;
				SetClicked(_isClicked);
			}
		}

		private void SetClicked(bool isClicked)
		{
			if (isClicked)
			{
				_clickedImage.enabled = true;
				_unclickedImage.enabled = false;
			}
			else
			{
				_clickedImage.enabled = false;
				_unclickedImage.enabled = true;
			}
		}

		private void Awake()
		{
			GetComponent<InteractableButton>().onButtonDownEvent += OnButtonClick;
			SetClicked(false);
		}

		private void OnButtonClick()
		{
			_isClicked = !_isClicked;
			SetClicked(_isClicked);
		}
	}
}
