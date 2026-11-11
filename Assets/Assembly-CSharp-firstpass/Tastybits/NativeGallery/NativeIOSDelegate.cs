using System;
using System.Collections;
using UnityEngine;

namespace Tastybits.NativeGallery
{
	public class NativeIOSDelegate : MonoBehaviour
	{
		public string randomId;

		private bool invoked = false;

		private float timeout = -1337f;

		private Action<Hashtable> deleg;

		public static NativeIOSDelegate CreateNativeIOSDelegate(Action<Hashtable> methodToCall)
		{
			int num = UnityEngine.Random.Range(100000, 999999);
			int num2 = 0;
			while (GameObject.Find("NativeDelegate_" + num) != null && num2++ < 100)
			{
				num = UnityEngine.Random.Range(100000, 999999);
			}
			if (GameObject.Find("NativeDelegate_" + num) != null)
			{
				Debug.LogError("Error there is allready a callback existing");
				return null;
			}
			GameObject gameObject = GameObject.Find("NativeDelegates");
			if (gameObject == null)
			{
				gameObject = new GameObject("NativeDelegates");
			}
			GameObject gameObject2 = new GameObject("NativeDelegate_" + num);
			gameObject2.transform.parent = gameObject.transform;
			NativeIOSDelegate nativeIOSDelegate = gameObject2.AddComponent<NativeIOSDelegate>();
			nativeIOSDelegate.deleg = methodToCall;
			return nativeIOSDelegate;
		}

		private void Update()
		{
			if (!(timeout <= -1337f) && timeout > 0f)
			{
				timeout -= Time.fixedTime;
				if (timeout <= 0f)
				{
					OnTimeout();
				}
			}
		}

		private void OnTimeout()
		{
		}

		public void CallDelegateFromNative(string strData)
		{
			if (invoked)
			{
				Debug.LogError("Error the delegate is allready invoked");
				return;
			}
			Hashtable hashtable = JSON.Deserialize(strData) as Hashtable;
			if (hashtable == null)
			{
				Debug.LogError("Error parsing ret");
				return;
			}
			Debug.Log(string.Concat("Str= ", hashtable, " str data = ", strData));
			deleg(hashtable);
			base.enabled = false;
			base.gameObject.SetActive(false);
			UnityEngine.Object.DestroyObject(base.gameObject);
		}
	}
}
