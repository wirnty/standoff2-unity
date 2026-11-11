using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.GameStats
{
	public class PersonalLeaderBoardPlayerView : View
	{
		[SerializeField]
		[NotNull]
		private Image _iconImage;

		[SerializeField]
		[NotNull]
		private Image _borderImage;

		[NotNull]
		[SerializeField]
		private Image _healthImage;

		[SerializeField]
		[NotNull]
		private TextView _scoreText;

		public void Refresh(Sprite icon, Color color, int score, bool scoreVisible)
		{
			_iconImage.sprite = icon;
			_borderImage.color = color;
			_healthImage.color = color;
			if (scoreVisible)
			{
				_scoreText.Text = score.ToString();
				_scoreText.Show();
			}
			else
			{
				_scoreText.Hide();
			}
		}
	}
}
