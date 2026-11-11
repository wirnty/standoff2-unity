using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	[RequireComponent(typeof(Image))]
	public class Dialog : View
	{
		[NotNull]
		[SerializeField]
		private Text _titleText;

		[SerializeField]
		[NotNull]
		private Text _defaultTimer;

		[SerializeField]
		[NotNull]
		private Text _countDownTimer;

		[NotNull]
		[SerializeField]
		private Text _contentText;

		[SerializeField]
		[NotNull]
		private Button _buttonPrefab;

		[SerializeField]
		[NotNull]
		private GameObject _progressIndicator;

		private Image _backgroundImage;

		private DialogTimer.ITimer _timer;

		private Action<DialogButton> _resultCallback;

		private bool _hideAfter;

		private MonoBehaviour _customContent;

		public string Title
		{
			get
			{
				return _titleText.text;
			}
			set
			{
				_titleText.text = value;
			}
		}

		public string ContentText
		{
			set
			{
				_contentText.text = value;
			}
		}

		public bool BackgroundEnabled
		{
			set
			{
				_backgroundImage.enabled = value;
			}
		}

		private void Awake()
		{
			_countDownTimer.gameObject.SetActive(false);
			_defaultTimer.gameObject.SetActive(false);
			_buttonPrefab.gameObject.SetActive(false);
			_backgroundImage = this.GetRequireComponent<Image>();
			_progressIndicator.gameObject.SetActive(true);
		}

		public void SetContent(View content)
		{
			SetCustomContent(UnityEngine.Object.Instantiate(content));
		}

		public void SetContent([NotNull] Sprite sprite)
		{
			if (sprite == null)
			{
				throw new ArgumentNullException("sprite");
			}
			GameObject gameObject = new GameObject(sprite.name);
			Image image = gameObject.AddComponent<Image>();
			image.sprite = sprite;
			image.SetNativeSize();
			image.preserveAspect = true;
			SetCustomContent(image);
		}

		private void SetCustomContent(MonoBehaviour content)
		{
			content.transform.SetParent(_contentText.transform.parent, false);
			_contentText.gameObject.SetActive(false);
			_customContent = content;
		}

		public T GetContent<T>() where T : View
		{
			return (T)_customContent;
		}

		public void SetTimer(DialogTimer.ITimer timer)
		{
			_timer = timer;
		}

		public void AddButton(DialogButton btn)
		{
			Button button = UnityEngine.Object.Instantiate(_buttonPrefab, _buttonPrefab.transform.parent, false);
			button.GetComponentInChildren<Text>().text = btn.Name;
			button.onClick.AddListener(delegate
			{
				Result(btn);
			});
			button.gameObject.SetActive(true);
			_progressIndicator.gameObject.SetActive(false);
		}

		private void Result(DialogButton btn)
		{
			if (_resultCallback != null)
			{
				_resultCallback(btn);
				if (_hideAfter)
				{
					Hide();
				}
			}
			else
			{
				Hide();
			}
		}

		public void AddButtons(params DialogButton[] buttons)
		{
			foreach (DialogButton btn in buttons)
			{
				AddButton(btn);
			}
		}

		public override void Show()
		{
			base.Show();
			if ((object)_customContent != null)
			{
				_customContent.gameObject.SetActive(true);
			}
			if (_timer != null)
			{
				StartCoroutine(Timer());
			}
		}

		public void Show([NotNull] Action<DialogButton> resultCallback, bool hideAfterResult = true)
		{
			if (resultCallback == null)
			{
				throw new ArgumentNullException("resultCallback");
			}
			_resultCallback = resultCallback;
			_hideAfter = hideAfterResult;
			Show();
		}

		private IEnumerator Timer()
		{
			float time = Time.time;
			bool countDownTimer = _timer is DialogTimer.CountDown;
			if (countDownTimer)
			{
				_countDownTimer.gameObject.SetActive(true);
			}
			else
			{
				_defaultTimer.gameObject.SetActive(true);
			}
			while (base.IsVisible)
			{
				if (countDownTimer)
				{
					float num = ((DialogTimer.CountDown)_timer).Duration - Time.time + time;
					if (num < 0f)
					{
						num = 0f;
					}
					_countDownTimer.text = StringUtils.FormatTwoDigit(num);
				}
				else
				{
					_defaultTimer.text = StringUtils.FormatTwoDigit(Time.time - time);
				}
				yield return new WaitForSeconds(1f);
			}
		}

		public override void Hide()
		{
			base.Hide();
			StopAllCoroutines();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
