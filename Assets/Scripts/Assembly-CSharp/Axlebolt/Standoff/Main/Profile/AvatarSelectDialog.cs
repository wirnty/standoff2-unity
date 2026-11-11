using System;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Profile
{
	public class AvatarSelectDialog : View
	{
		[SerializeField]
		[NotNull]
		private Button _loadFromGallery;

		[SerializeField]
		[NotNull]
		private Button _chooseFromLibrary;

		public Action LoadFromGalleryHandler { get; set; }

		public Action ChooseFromLibraryHandler { get; set; }

		private void Awake()
		{
			_loadFromGallery.onClick.AddListener(delegate
			{
				Action loadFromGalleryHandler = LoadFromGalleryHandler;
				if (loadFromGalleryHandler != null)
				{
					loadFromGalleryHandler();
				}
				Hide();
			});
			_chooseFromLibrary.onClick.AddListener(delegate
			{
				Action chooseFromLibraryHandler = ChooseFromLibraryHandler;
				if (chooseFromLibraryHandler != null)
				{
					chooseFromLibraryHandler();
				}
				Hide();
			});
		}
	}
}
