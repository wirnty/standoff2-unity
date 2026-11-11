using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Tastybits.NativeGallery
{
	public class Documents
	{
		private static string _documentDirectoryString = null;

		public static string DocumentsPath
		{
			get
			{
				return GetDocumentsPath();
			}
		}

		private static string GetDocumentsDirectoryOniOS()
		{
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				return Application.persistentDataPath;
			}
			if (string.IsNullOrEmpty(_documentDirectoryString))
			{
				_documentDirectoryString = ImagePicker_GetDocumentDirectory();
			}
			return _documentDirectoryString;
		}

		[DllImport("__Internal")]
		private static extern string ImagePicker_GetDocumentDirectory();

		public static bool LoadTextureFromDocuments(string fname, out Texture2D retval)
		{
			string text = Path.Combine(DocumentsPath, fname);
			if (!File.Exists(text))
			{
				Debug.Log("Documents: TryLoadFromDocuments file at address does not exist : " + text);
				retval = null;
				return false;
			}
			byte[] array = null;
			array = File.ReadAllBytes(text);
			if (array == null)
			{
				Debug.LogError("Documents: Error calling ReadAllBytes at Path : " + text);
				retval = null;
				return false;
			}
			Texture2D texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(array);
			retval = texture2D;
			return true;
		}

		public static void SaveTextureToDocuments(string fname, Texture2D texture)
		{
			byte[] bytes = texture.EncodeToPNG();
			string path = Path.Combine(DocumentsPath, fname);
			File.WriteAllBytes(path, bytes);
		}

		private static string GetDocumentsPath()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				string documentsDirectoryOniOS = GetDocumentsDirectoryOniOS();
				if (!string.IsNullOrEmpty(documentsDirectoryOniOS))
				{
					return documentsDirectoryOniOS;
				}
				Debug.LogError("Error getting documents path via native method.. retrying with Unity method");
				if (Application.dataPath.EndsWith("/Data"))
				{
					string text = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
					text = text.Substring(0, text.LastIndexOf('/'));
					return text + "/Documents";
				}
				Debug.LogError("Error datapath did not end with  /Data : " + Application.dataPath);
				Debug.LogError("Error generating document path ");
				return "";
			}
			if (Application.isEditor)
			{
				if (!Application.dataPath.EndsWith("/Assets"))
				{
					Debug.LogError("Error datapath does not end with Assets");
				}
				string text2 = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
				string text3 = text2 + "/Documents";
				if (!Directory.Exists(text3))
				{
					Directory.CreateDirectory(text3);
				}
				return text3;
			}
			return Application.persistentDataPath;
		}
	}
}
