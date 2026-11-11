using System;
using Axlebolt.Bolt;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Profile
{
	public class ShortProfileView : View
	{
		[NotNull]
		[SerializeField]
		private RawImage _avatarImage;

		[SerializeField]
		[NotNull]
		private Text _idText;

		[SerializeField]
		[NotNull]
		private Text _nameText;

		private Texture _defaultAvatar;

		public Action NameChangeHandler { get; set; }

		public Action AvatarChangeEvent { get; set; }

		private bool Editable { get; set; }

		private void Awake()
		{
			Editable = false;
			_defaultAvatar = _avatarImage.texture;
			_avatarImage.GetRequireComponent<Button>().onClick.AddListener(delegate
			{
				if (Editable)
				{
					Action avatarChangeEvent = AvatarChangeEvent;
					if (avatarChangeEvent != null)
					{
						avatarChangeEvent();
					}
				}
			});
			_nameText.GetRequireComponent<Button>().onClick.AddListener(delegate
			{
				if (Editable)
				{
					Action nameChangeHandler = NameChangeHandler;
					if (nameChangeHandler != null)
					{
						nameChangeHandler();
					}
				}
			});
		}

		public void SetPlayer(Axlebolt.Bolt.Player player, bool editable)
		{
			Editable = editable;
			_idText.text = player.Uid;
			_nameText.text = player.Name;
			_avatarImage.texture = ((player.Avatar == null) ? _defaultAvatar : TextureUtility.FromBytes(player.Avatar));
		}
	}
}
