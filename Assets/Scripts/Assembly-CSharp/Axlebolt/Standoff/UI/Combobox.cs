using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.UI
{
	public class Combobox<T> : Field<T>
	{
		[SerializeField]
		private Transform _popupParent;

		[SerializeField]
		private bool _lowerPopup = true;

		private ComboboxField _field;

		private ComboboxPopup _popup;

		private readonly List<T> _models = new List<T>();

		private Func<T, string> _formatter;

		private bool _initialized;

		public T[] Models
		{
			get
			{
				return _models.ToArray();
			}
			set
			{
				SetModels(value);
			}
		}

		private void Awake()
		{
			InitIfNeed();
		}

		private void Init()
		{
			_field = base.transform.Find("Field").GetRequireComponent<ComboboxField>();
			_field.Text = "";
			ComboboxPopup requireComponent = base.transform.Find("UpperPopup").GetRequireComponent<ComboboxPopup>();
			ComboboxPopup requireComponent2 = base.transform.Find("LowerPopup").GetRequireComponent<ComboboxPopup>();
			if (_lowerPopup)
			{
				_popup = requireComponent2;
				UnityEngine.Object.Destroy(requireComponent.gameObject);
			}
			else
			{
				_popup = requireComponent;
				UnityEngine.Object.Destroy(requireComponent2.gameObject);
			}
			_popup.ParentTransform = _popupParent;
			_popup.Show();
			_popup.Hide();
			_field.Popup = _popup;
			if (_formatter == null)
			{
				_formatter = (T t) => t.ToString();
			}
			_popup.SelectEventHandler = delegate(int index)
			{
				base.Value = _models[index];
			};
			_initialized = true;
		}

		private void InitIfNeed()
		{
			if (!_initialized)
			{
				Init();
			}
		}

		protected override void SetValue(T value)
		{
			_field.Text = ((value != null) ? _formatter(value) : "");
			base.SetValue(value);
		}

		private void SetModels([NotNull] T[] models)
		{
			if (models == null)
			{
				throw new ArgumentNullException("models");
			}
			_models.Clear();
			_popup.Clear();
			_models.AddRange(models);
			if (!_lowerPopup)
			{
				_models.Reverse();
			}
			foreach (T model in _models)
			{
				_popup.AddRow(_formatter(model));
			}
		}

		public void SetFormatter([NotNull] Func<T, string> formatter)
		{
			if (formatter == null)
			{
				throw new ArgumentNullException("formatter");
			}
			_formatter = formatter;
		}
	}
}
