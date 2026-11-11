using System;
using System.Linq;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI
{
	public class WhoKillView : View
	{
		[SerializeField]
		[NotNull]
		private Image _weaponPreview;

		[SerializeField]
		[NotNull]
		private Text _weaponText;

		[SerializeField]
		[NotNull]
		private Text _nameText;

		[SerializeField]
		[NotNull]
		private Image _iconImage;

		[SerializeField]
		[NotNull]
		private Image _borderImage;

		[SerializeField]
		[NotNull]
		private Text _damageTakenText;

		[SerializeField]
		[NotNull]
		private Text _damageGivenText;

		[SerializeField]
		private Color _trColor;

		[SerializeField]
		private Color _ctColor;

		public void Show([NotNull] WeaponParameters weapon, [NotNull] PhotonPlayer killer, HitLogs takenHits, HitLogs givenHits)
		{
			if (weapon == null)
			{
				throw new ArgumentNullException("weapon");
			}
			if (killer == null)
			{
				throw new ArgumentNullException("killer");
			}
			_weaponPreview.sprite = weapon.Sprites.Preview;
			_weaponText.text = weapon.DisplayName;
			_nameText.text = killer.NickName;
			_iconImage.sprite = AvatarSupport.GetAvatar(killer);
			_borderImage.color = ((killer.GetTeam() != Team.Ct) ? _trColor : _ctColor);
			_damageTakenText.text = takenHits.Damages.Sum().ToString();
			_damageGivenText.text = givenHits.Damages.Sum().ToString();
			Show();
		}
	}
}
