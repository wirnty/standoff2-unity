using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tastybits.NativeGallery
{
	public class EditorGalleryUI : MonoBehaviour
	{
		private Action<Texture2D, ExifOrientation> callback;

		public void OpenGallery(Action<Texture2D, ExifOrientation> callback)
		{
			this.callback = callback;
			base.transform.SetAsLastSibling();
			base.gameObject.SetActive(true);
		}

		public void OnGalleryItemClicked(GameObject go)
		{
			if (callback != null)
			{
				RawImage component = go.transform.Find("Image").GetComponent<RawImage>();
				callback((Texture2D)component.texture, ExifOrientation.ORIENTATION_NORMAL);
				base.gameObject.SetActive(false);
			}
		}

		private void Update()
		{
			for (int i = 0; i < base.gameObject.transform.childCount; i++)
			{
				GameObject gameObject = base.gameObject.transform.GetChild(i).gameObject;
				gameObject.SetActive(false);
				gameObject.SetActive(true);
			}
		}
	}
}
