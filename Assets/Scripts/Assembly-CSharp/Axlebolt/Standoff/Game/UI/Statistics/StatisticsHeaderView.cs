using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Statistics
{
	public class StatisticsHeaderView : View
	{
		[SerializeField]
		[NotNull]
		private Text _timeText;

		[SerializeField]
		[NotNull]
		private Text _descriptionText;

		[SerializeField]
		[NotNull]
		private Text _gameModeText;

		[SerializeField]
		[NotNull]
		private Text _levelText;

		public double Time
		{
			set
			{
				_timeText.text = StringUtils.FormatTwoDigit(value);
			}
		}

		public string Description
		{
			set
			{
				_descriptionText.text = value;
			}
		}

		public string GameMode
		{
			set
			{
				_gameModeText.text = value;
			}
		}

		public string Level
		{
			set
			{
				_levelText.text = value;
			}
		}

		private void Awake()
		{
			_timeText.text = "";
			_descriptionText.text = "";
			_gameModeText.text = "";
			_levelText.text = "";
		}
	}
}
