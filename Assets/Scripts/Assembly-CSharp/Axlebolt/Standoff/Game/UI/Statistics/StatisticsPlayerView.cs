using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Statistics
{
	[RequireComponent(typeof(Image))]
	public class StatisticsPlayerView : View
	{
		[SerializeField]
		[NotNull]
		private Image _avatarImage;

		[SerializeField]
		[NotNull]
		private Text _pingText;

		[SerializeField]
		[NotNull]
		private Image _avatarBackgroundImage;

		[NotNull]
		[SerializeField]
		private Text _nameText;

		[SerializeField]
		[NotNull]
		private Text _killsText;

		[SerializeField]
		[NotNull]
		private Text _assistsText;

		[SerializeField]
		[NotNull]
		private Text _deathText;

		[NotNull]
		[SerializeField]
		private Text _scoreText;

		private Image _image;

		private void Awake()
		{
			_image = GetComponent<Image>();
		}

		public void Refresh(PhotonPlayer player, Color color, Color textColor, bool currentPlayer)
		{
			_avatarBackgroundImage.gameObject.SetActive(currentPlayer);
			_avatarImage.sprite = AvatarSupport.GetAvatar(player);
			_nameText.text = player.NickName;
			_nameText.color = textColor;
			_pingText.text = player.GetPing().ToString();
			_pingText.color = textColor;
			_killsText.text = player.GetKills().ToString();
			_killsText.color = textColor;
			_assistsText.text = player.GetAssists().ToString();
			_assistsText.color = textColor;
			_deathText.text = player.GetDeath().ToString();
			_deathText.color = textColor;
			_scoreText.text = player.GetScore().ToString();
			_scoreText.color = textColor;
			_image.color = color;
		}
	}
}
