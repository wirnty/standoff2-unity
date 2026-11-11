using System;
using Axlebolt.Standoff.Common;
using I2.Loc;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.UI
{
	public class Dialogs : MonoBehaviour
	{
		private static Dialogs _instance;

		[SerializeField]
		[NotNull]
		private Dialog _dialogPrefab;

		private static Dialogs Instance
		{
			get
			{
				if (_instance == null)
				{
					GameObject original = ResourcesUtility.Load<GameObject>("UI/Dialogs");
					_instance = UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.identity).GetRequireComponent<Dialogs>();
				}
				return _instance;
			}
		}

		public static Dialog Create()
		{
			return Instance.CreateDialog();
		}

		public static Dialog Create(string title, View content, params DialogButton[] buttons)
		{
			Debug.Log("Created");
			Dialog dialog = Create();
			dialog.Title = title;
			dialog.SetContent(content);
			dialog.AddButtons(buttons);
			return dialog;
		}

		public static Dialog Create(string title, string contentText, params DialogButton[] buttons)
		{
			Debug.Log("Created");
			Dialog dialog = Create();
			dialog.Title = title;
			dialog.ContentText = contentText;
			dialog.AddButtons(buttons);
			return dialog;
		}

		public static Dialog Create(string title, Sprite content, params DialogButton[] buttons)
		{
			Debug.Log("Created");
			Dialog dialog = Create();
			dialog.Title = title;
			dialog.SetContent(content);
			dialog.AddButtons(buttons);
			return dialog;
		}

		public static Dialog Message([NotNull] string title, [NotNull] string message, [NotNull] Action okAction)
		{
			Debug.Log("Created");
			if (title == null)
			{
				throw new ArgumentNullException("title");
			}
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			if (okAction == null)
			{
				throw new ArgumentNullException("okAction");
			}
			Dialog dialog = Create(title, message, new DialogButton(ScriptLocalization.Common.Ok));
			dialog.Show(delegate
			{
				okAction();
			});
			return dialog;
		}

		private void Awake()
		{
			_dialogPrefab.gameObject.SetActive(false);
		}

		private Dialog CreateDialog()
		{
			Debug.Log("Created");
			Dialog dialog = UnityEngine.Object.Instantiate(_dialogPrefab, base.transform, false);
			dialog.gameObject.SetActive(true);
			dialog.gameObject.SetActive(false);
			return dialog;
		}

		private void OnDestroy()
		{
			_instance = null;
		}
	}
}
