using System.Collections;
using Axlebolt.Networking;
using Axlebolt.Standoff.Player.Networking;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	[RequireComponent(typeof(PlayerController))]
	public class CharacterPlayer : ObjectPlayer
	{
		private readonly PlayerInterpolator _playerInterpolator = new PlayerInterpolator();

		private PlayerController _playerController;

		private bool _isCharacterVisible;

		public override void PreInitialize()
		{
			base.PreInitialize();
			_playerController = this.GetRequireComponent<PlayerController>();
		}

		public override void Clear()
		{
			base.Clear();
			_isCharacterVisible = false;
		}

		public override Interpolator GetInterpolator()
		{
			return _playerInterpolator;
		}

		protected override void SetSnapshot(ObjectSnapshot snapshot)
		{
			_playerController.SetSnapsot((PlayerSnapshot)snapshot);
			if (!_isCharacterVisible)
			{
				_isCharacterVisible = true;
				StartCoroutine(EnableCharacter());
			}
		}

		private IEnumerator EnableCharacter()
		{
			yield return null;
			_playerController.SetCharacterVisible(true);
		}
	}
}
