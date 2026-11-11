using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI
{
	public class NotificationView : View
	{
		[SerializeField]
		[NotNull]
		private Text _titleText;

		[SerializeField]
		[NotNull]
		private Text _descriptionText;

		public string Title
		{
			set
			{
				_titleText.text = value;
			}
		}

		public string Description
		{
			set
			{
				_descriptionText.text = value;
			}
		}
	}
}
