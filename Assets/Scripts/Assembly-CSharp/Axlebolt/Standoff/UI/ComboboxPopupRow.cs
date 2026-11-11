using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	public class ComboboxPopupRow : View
	{
		[NotNull]
		[SerializeField]
		private Text _text;

		public string Text
		{
			set
			{
				_text.text = value;
			}
		}
	}
}
