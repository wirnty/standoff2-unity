using System;
using UnityEngine;

namespace Tastybits.NativeGallery
{
	[RequireComponent(typeof(RectTransform))]
	public class NativeGalleryController : MonoBehaviour
	{
		public enum LoadImageMethods
		{
			WWWLoading = 1,
			NativeLoading = 2
		}

		private static NativeGalleryController _instance;

		[HideInInspector]
		[SerializeField]
		private bool iAmSingleton = false;

		[Tooltip("Contains a referance to the Editor Only Prefab for the UI")]
		public GameObject editorUIPrefab;

		public bool verbose = false;

		[Tooltip("Editor gallery simulator UI referance")]
		public GameObject editorGallery;

		[Tooltip("On iOS you can make the native gallery select images from diffirent types of Galleries. Photos,Saved Photos and Camera")]
		public ImagePickerType iOSImagePickerType = ImagePickerType.UIImagePickerControllerSourceTypeSavedPhotosAlbum;

		[Tooltip("Rotate imported image is used to counter rotate the imported image so that landscape images are looking right when imported.")]
		public bool rotateImportedImage = true;

		[Tooltip("On Android the Unity Android Player has a difficult time loading using the WWW class use Native Loading if you're experencing this")]
		public LoadImageMethods LoadImageMethod = LoadImageMethods.NativeLoading;

		public static string LastPickedImagePath = "";

		public static bool Verbose
		{
			get
			{
				if (_instance == null)
				{
					return false;
				}
				return _instance.verbose;
			}
		}

		public static NativeGalleryController instance
		{
			get
			{
				if (_instance == null)
				{
					GameObject[] array = UnityEngine.Object.FindObjectsOfType<GameObject>();
					GameObject[] array2 = array;
					foreach (GameObject self in array2)
					{
						NativeGalleryController componentInChildren = self.GetComponentInChildren2<NativeGalleryController>(true);
						if (componentInChildren != null)
						{
							_instance = componentInChildren;
							break;
						}
					}
				}
				return _instance;
			}
		}

		private void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Debug.LogWarning("The gameobject : " + base.name + " was destroyed since there is already an insteance of the Native Gallery Component loaded");
				UnityEngine.Object.DestroyImmediate(base.gameObject);
				return;
			}
			if (_instance == null)
			{
				_instance = this;
			}
			if (Time.frameCount < 3)
			{
				instance.gameObject.SetActive(false);
			}
			iAmSingleton = true;
		}

		private void OnDestroy()
		{
			if (_instance == this)
			{
				_instance = null;
			}
		}

		public static void OpenGallery(Action<Texture2D, ExifOrientation> callback)
		{
			if (instance == null)
			{
				UnityEngine.Object original = Resources.Load("NativeGallery", typeof(GameObject));
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(original);
				gameObject.name = "NativeGallery";
				if (instance == null)
				{
					Debug.LogError("Cannot open Test gallery in editor mode since no instance of NativeGalleryController was found");
					callback(null, ExifOrientation.ORIENTATION_UNDEFINED);
					return;
				}
				Debug.Log("NativeGallery: We have autocreted the instance of the NativeGalleryp prefab in the scene");
			}
			instance.gameObject.SetActive(true);
			if (!Application.isEditor && instance.editorGallery != null)
			{
				instance.editorGallery.SetActive(false);
			}
			AndroidGallery.LoadImageMethod = instance.LoadImageMethod;
			ImagePicker.OpenGallery(delegate(Texture2D tx, ExifOrientation orient)
			{
				instance.gameObject.SetActive(false);
				callback(tx, orient);
			}, instance.rotateImportedImage, instance.iOSImagePickerType);
		}

		public static void OpenEditorGallery(Action<Texture2D, ExifOrientation> callback)
		{
			if (instance == null)
			{
				Debug.LogError("Cannot open Test gallery in editor mode since no instance of NativeGalleryController was found");
				callback(null, ExifOrientation.ORIENTATION_UNDEFINED);
				return;
			}
			if (instance.editorGallery == null)
			{
				Canvas canvas = CanvasHelper.GetCanvas();
				if (canvas != null && instance.editorUIPrefab != null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(instance.editorUIPrefab);
					instance.editorGallery = gameObject;
					RectTransform component = gameObject.GetComponent<RectTransform>();
					component.SetParent(canvas.GetComponent<RectTransform>(), false);
					gameObject.name = "NativeGalleryUI";
				}
			}
			if (instance.editorGallery == null || instance.editorGallery.GetComponent<EditorGalleryUI>() == null)
			{
				Debug.LogError("Cannot open Test gallery in editor mode since no referance to an EditorGalleryUI object was found");
				return;
			}
			instance.gameObject.transform.SetAsLastSibling();
			EditorGalleryUI component2 = instance.editorGallery.GetComponent<EditorGalleryUI>();
			component2.OpenGallery(callback);
		}
	}
}
