using System;
using System.Collections;
using UnityEngine;

namespace Tastybits.NativeGallery
{
	public class WWWUtil : MonoBehaviour
	{
		private static WWWUtil instance = null;

		public WWW target;

		private Action<WWW, bool> callback;

		private bool running = false;

		public WWWUtil()
		{
			instance = null;
		}

		public static void Wait(WWW www, Action<WWW, bool> _callback)
		{
			if (instance == null)
			{
				GameObject gameObject = GameObject.Find("Job");
				if (gameObject == null)
				{
					gameObject = new GameObject("Job");
				}
				WWWUtil wWWUtil = gameObject.GetComponent<WWWUtil>();
				if (wWWUtil == null)
				{
					wWWUtil = gameObject.AddComponent<WWWUtil>();
				}
				instance = wWWUtil;
			}
			if (instance.running)
			{
				_callback(null, false);
				return;
			}
			if (www == null)
			{
				Debug.LogError("Error WWW is null");
			}
			instance.running = true;
			instance.target = www;
			instance.callback = _callback;
			instance.StartCoroutine(instance.tmp());
		}

		private IEnumerator tmp()
		{
			yield return target;
			callback(target, string.IsNullOrEmpty(target.error));
			target = null;
			running = false;
		}
	}
}
