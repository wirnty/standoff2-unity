using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI.Winners
{
	public class WinnersView : View
	{
		[SerializeField]
		[NotNull]
		private WinPlayersView _winPlayers;

		public void Show(WinPlayers winPlayers)
		{
			if (winPlayers.FirstPlace != null)
			{
				_winPlayers.SetFirstPlace(GetAvatar(winPlayers.FirstPlace), winPlayers.FirstPlace.NickName);
				if (winPlayers.SecondPlace != null)
				{
					_winPlayers.SetSecondPlace(GetAvatar(winPlayers.SecondPlace), winPlayers.SecondPlace.NickName);
					if (winPlayers.ThirdPlace != null)
					{
						_winPlayers.SetThirdPlace(GetAvatar(winPlayers.ThirdPlace), winPlayers.ThirdPlace.NickName);
					}
				}
			}
			_winPlayers.Show();
			Show();
		}

		public override void Hide()
		{
			_winPlayers.Hide();
			base.Hide();
		}

		private Sprite GetAvatar(PhotonPlayer photonPlayer)
		{
			return AvatarSupport.GetAvatar(photonPlayer);
		}
	}
}
