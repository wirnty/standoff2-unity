using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	public class ComboboxField : View, IPointerDownHandler, IEventSystemHandler
	{
		[SerializeField]
		[NotNull]
		private Text _text;

		private ComboboxPopup _popup;

		public string Text
		{
			set
			{
				_text.text = value;
			}
		}

		public ComboboxPopup Popup
		{
			set
			{
				_popup = value;
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left)
			{
				if (_popup.IsVisible)
				{
					_popup.Hide();
					return;
				}
				_popup.Show();
				eventData.selectedObject = _popup.gameObject;
			}
		}
	}
}
