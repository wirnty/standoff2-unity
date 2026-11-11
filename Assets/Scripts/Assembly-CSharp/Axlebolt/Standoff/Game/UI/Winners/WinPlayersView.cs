using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Winners
{
	public class WinPlayersView : View
	{
		[NotNull]
		[SerializeField]
		private Text _firstPlaceText;

		[NotNull]
		[SerializeField]
		private Image _firstPlaceAvatar;

		[NotNull]
		[SerializeField]
		private GameObject _secondPlaceTitle;

		[NotNull]
		[SerializeField]
		private Text _secondPlaceText;

		[NotNull]
		[SerializeField]
		private Image _secondPlaceAvatar;

		[NotNull]
		[SerializeField]
		private GameObject _thirdPlaceTitle;

		[NotNull]
		[SerializeField]
		private Text _thirdPlaceText;

		[NotNull]
		[SerializeField]
		private Image _thirdPlaceAvatar;

		private void Awake()
		{
			HideAll();
		}

		public void SetFirstPlace(Sprite avatar, string playerName)
		{
			_firstPlaceAvatar.sprite = avatar;
			_firstPlaceAvatar.gameObject.SetActive(true);
			_firstPlaceText.text = playerName;
			_firstPlaceText.gameObject.SetActive(true);
		}

		public void SetSecondPlace(Sprite avatar, string playerName)
		{
			_secondPlaceTitle.SetActive(true);
			_secondPlaceAvatar.sprite = avatar;
			_secondPlaceAvatar.gameObject.SetActive(true);
			_secondPlaceText.text = playerName;
			_secondPlaceText.gameObject.SetActive(true);
		}

		public void SetThirdPlace(Sprite avatar, string playerName)
		{
			_thirdPlaceTitle.SetActive(true);
			_thirdPlaceAvatar.sprite = avatar;
			_thirdPlaceAvatar.gameObject.SetActive(true);
			_thirdPlaceText.text = playerName;
			_thirdPlaceText.gameObject.SetActive(true);
		}

		public override void Hide()
		{
			HideAll();
		}

		private void HideAll()
		{
			_firstPlaceText.gameObject.SetActive(false);
			_firstPlaceAvatar.gameObject.SetActive(false);
			_secondPlaceTitle.SetActive(false);
			_secondPlaceText.gameObject.SetActive(false);
			_secondPlaceAvatar.gameObject.SetActive(false);
			_thirdPlaceTitle.SetActive(false);
			_thirdPlaceText.gameObject.SetActive(false);
			_thirdPlaceAvatar.gameObject.SetActive(false);
		}
	}
}
