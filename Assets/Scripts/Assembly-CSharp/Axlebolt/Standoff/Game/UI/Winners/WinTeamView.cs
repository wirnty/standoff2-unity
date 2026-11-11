using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Winners
{
	public class WinTeamView : View
	{
		[SerializeField]
		[NotNull]
		private Text _titleText;

		[SerializeField]
		[NotNull]
		private Image _mvpAvatar;

		[NotNull]
		[SerializeField]
		private Text _mvpText;

		[SerializeField]
		[NotNull]
		private Image _teamEmblemImage;

		[SerializeField]
		private Color _trColor;

		[SerializeField]
		private Color _ctColor;

		[SerializeField]
		[NotNull]
		private Sprite _trEmblem;

		[NotNull]
		[SerializeField]
		private Sprite _ctEmblem;

		public void Show(WinTeam winTeam)
		{
		}
	}
}
