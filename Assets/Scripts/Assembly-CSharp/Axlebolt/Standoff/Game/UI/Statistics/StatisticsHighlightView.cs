using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Statistics
{
	public class StatisticsHighlightView : View
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
