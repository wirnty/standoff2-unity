using System.Runtime.CompilerServices;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI.GameStats
{
	public class PersonalGameStatsView : View, IPlayerPropSensitiveView
	{
		private static readonly string[] PlayerProps = new string[3] { "team", "score", "isDeath" };

		[NotNull]
		[SerializeField]
		private TextView _timeText;

		[NotNull]
		[SerializeField]
		private PersonalLeaderBoardView _personalLeaderBoardView;

		public double Time
		{
			set
			{
				_timeText.Text = StringUtils.FormatTwoDigit(value);
			}
		}

		public bool ScoreVisible { get; set; }

		public bool TimeVisible
		{
			set
			{
				if (value)
				{
					_timeText.Show();
				}
				else
				{
					_timeText.Hide();
				}
			}
		}

		public string[] SensitivePlayerProperties
		{
			[CompilerGenerated]
			get
			{
				return PlayerProps;
			}
		}

		private void Awake()
		{
			_timeText.Text = "";
		}

		public override void Show()
		{
			base.Show();
			Refresh();
		}

		public void Refresh()
		{
			_personalLeaderBoardView.Refresh(ScoreVisible);
		}
	}
}
