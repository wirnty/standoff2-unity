using System;
using UnityEngine;

namespace Tastybits.NativeGallery
{
	public class ImagePicker
	{
		private static Action<Texture2D, ExifOrientation> _callback;

		public static void OpenGallery(Action<Texture2D, ExifOrientation> callback, bool rotateImportedImage, ImagePickerType iOSImagePickerType)
		{
			if (NativeGalleryController.Verbose)
			{
				Debug.Log("Opening Image Picker");
			}
			_callback = callback;
			if (NativeGalleryController.Verbose)
			{
				Debug.Log("Opening Image Picker - Android");
			}
			AndroidGallery.OpenGallery(delegate(Texture2D tex)
			{
				_callback(tex, AndroidGallery.GetLastImportedOrientation());
			}, rotateImportedImage);
		}
	}
}
