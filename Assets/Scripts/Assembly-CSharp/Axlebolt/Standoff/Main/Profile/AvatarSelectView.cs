using System;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using JetBrains.Annotations;
using Tastybits.NativeGallery;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Profile
{
	public class AvatarSelectView : View
	{
		private static readonly Log Log = Log.Create(typeof(AvatarSelectView));

		[NotNull]
		[SerializeField]
		private AvatarSelectDialog _avatarSelectDialog;

		[SerializeField]
		[NotNull]
		private AvatarSelectLibraryView _libraryView;

		[SerializeField]
		[NotNull]
		private Transform _parentTransform;

		private Action<Texture2D> _callback;

		private Texture2D _avatar;

		private void Awake()
		{
			_avatarSelectDialog.Hide();
			_libraryView.Hide();
			_avatarSelectDialog.LoadFromGalleryHandler = LoadFromGallery;
			_avatarSelectDialog.ChooseFromLibraryHandler = ChooseFromLibrary;
			base.transform.SetParent(_parentTransform);
		}

		public void Show([NotNull] Action<Texture2D> callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			_callback = callback;
			Show();
			_avatarSelectDialog.Show();
		}

		private void LoadFromGallery()
		{
			if (Application.isEditor)
			{
				Result(null);
				return;
			}
			Log.Debug("NativeGalleryController call");
			NativeGalleryController.OpenGallery(delegate(Texture2D avatar, ExifOrientation orientation)
			{
				Log.Debug("NativeGalleryController successfully");
				Result(avatar);
			});
		}

		private void ChooseFromLibrary()
		{
			_libraryView.Show(delegate(Sprite avatar)
			{
				if (avatar == null)
				{
					Result(null);
				}
				else
				{
					Texture2D texture2D = new Texture2D((int)avatar.rect.width, (int)avatar.rect.height);
					Color[] pixels = avatar.texture.GetPixels((int)avatar.textureRect.x, (int)avatar.textureRect.y, (int)avatar.textureRect.width, (int)avatar.textureRect.height);
					texture2D.SetPixels(pixels);
					texture2D.Apply();
					Result(texture2D);
				}
			});
		}

		private void Result(Texture2D avatar)
		{
			if (avatar == null)
			{
				Log.Debug("Avatar selection canceled");
				Hide();
				return;
			}
			Log.Debug("Avatar successfully selected");
			TextureUtility.Bilinear(avatar, 192, 192);
			_avatar = avatar;
			Hide();
		}

		public override void Hide()
		{
			if (_callback != null)
			{
				_callback(_avatar);
			}
			_callback = null;
			_avatar = null;
			base.Hide();
		}
	}
}
