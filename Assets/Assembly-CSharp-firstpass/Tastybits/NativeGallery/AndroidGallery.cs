using System;
using System.Collections;
using UnityEngine;

namespace Tastybits.NativeGallery
{
	public class AndroidGallery
	{
		public static NativeGalleryController.LoadImageMethods LoadImageMethod = NativeGalleryController.LoadImageMethods.NativeLoading;

		private static AndroidJavaClass _AndroidCls = null;

		private static AndroidJavaClass AndroidCls
		{
			get
			{
				if (_AndroidCls == null)
				{
					_AndroidCls = new AndroidJavaClass("com.NativeGallery.AndroidGallery");
				}
				return _AndroidCls;
			}
		}

		public static void OpenGallery(Action<Texture2D> callback, bool rotateImportedImage = true)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				Debug.LogError("Cannot do this on a non Android platform");
			}
			string name = NativeIOSDelegate.CreateNativeIOSDelegate(delegate(Hashtable args)
			{
				Debug.Log("NativeCallback returned");
				bool flag = Convert.ToBoolean(args["succeeded"]);
				bool flag2 = Convert.ToBoolean(args["cancelled"]);
				string text = Convert.ToString(args["path"]);
				Debug.Log("AndroidGallery returned with succeeded = " + flag + " cancelled = " + flag2 + " path = " + text);
				if (flag && !flag2)
				{
					LoadAssetFromAssetLibrary(text, delegate(Texture2D tex)
					{
						callback(tex);
					});
				}
				else
				{
					NativeGalleryController.LastPickedImagePath = "";
					callback(null);
				}
			}).name;
			AndroidCls.CallStatic("OpenGallery", name, rotateImportedImage);
		}

		public static ExifOrientation GetLastImportedOrientation()
		{
			return (ExifOrientation)AndroidCls.CallStatic<int>("getLastImportedOrientation", new object[0]);
		}

		private static byte[] LoadPngBytesFromPath(string assetUrl, int maxTextureSize)
		{
			if (assetUrl.StartsWith("file://"))
			{
				assetUrl = assetUrl.Replace("file://", "");
			}
			AndroidJavaObject androidJavaObject = AndroidCls.CallStatic<AndroidJavaObject>("LoadPngBytesFromPath", new object[2] { assetUrl, maxTextureSize });
			bool flag = true;
			if (androidJavaObject == null)
			{
				Debug.LogError("Error the returned object was null");
				flag = false;
			}
			else if (androidJavaObject.GetRawObject() == IntPtr.Zero)
			{
				Debug.LogError("Error the raw object returned was null");
				flag = false;
			}
			else if (androidJavaObject.GetRawObject().ToInt32() == 0)
			{
				Debug.LogError("Error cannot convert raw object to Int32");
				flag = false;
			}
			if (flag)
			{
				Debug.Log("Converting raw object to byte array");
				byte[] array = AndroidJNIHelper.ConvertFromJNIArray<byte[]>(androidJavaObject.GetRawObject());
				if (array == null || array.Length == 0)
				{
					Debug.LogError("Error readig bytes from the asset library (1)");
				}
				AndroidCls.CallStatic("ClearPngByteBuffer");
				return array;
			}
			Debug.LogError("Error reading bytes from the asset library (2)");
			return null;
		}

		private static void LoadAssetFromAssetLibrary(string assetUrl, Action<Texture2D> callback)
		{
			if (LoadImageMethod == NativeGalleryController.LoadImageMethods.NativeLoading)
			{
				byte[] data = LoadPngBytesFromPath(assetUrl, GetMaxTextureSize());
				Texture2D texture2D = new Texture2D(2, 2);
				texture2D.LoadImage(data);
				callback(texture2D);
				return;
			}
			WWW www = new WWW(assetUrl);
			NativeGalleryController.LastPickedImagePath = assetUrl;
			WWWUtil.Wait(www, delegate(WWW w, bool www_ok)
			{
				if (www_ok && w.texture == null)
				{
					www_ok = false;
				}
				callback(w.texture);
			});
		}

		private static int GetMaxTextureSize(int cap = 2048)
		{
			try
			{
				using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.opengl.GLES20"))
				{
					using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("java.nio.IntBuffer"))
					{
						AndroidJavaObject androidJavaObject = androidJavaClass.CallStatic<AndroidJavaObject>("allocate", new object[1] { 1 });
						int @static = androidJavaClass2.GetStatic<int>("GL_MAX_TEXTURE_SIZE");
						androidJavaClass2.CallStatic("glGetIntegerv", @static, androidJavaObject);
						int num = androidJavaObject.Call<int>("get", new object[1] { 0 });
						if (num >= cap)
						{
							num = cap;
						}
						return num;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Error getting maximum texture size : " + ex.ToString());
				return 1024;
			}
		}
	}
}
