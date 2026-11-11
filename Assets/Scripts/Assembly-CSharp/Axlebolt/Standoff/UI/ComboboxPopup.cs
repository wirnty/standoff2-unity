using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Axlebolt.Standoff.UI
{
	public class ComboboxPopup : View, IDeselectHandler, IEventSystemHandler
	{
		public delegate void EventHandler(int index);

		[SerializeField]
		[NotNull]
		private ComboboxPopupRow _row;

		private bool _isEnter;

		private readonly List<ComboboxPopupRow> _rows = new List<ComboboxPopupRow>();

		private Transform _originalParent;

		public EventHandler SelectEventHandler { get; set; }

		public Transform ParentTransform { get; set; }

		private void Awake()
		{
			_originalParent = base.transform.parent;
			_row.gameObject.SetActive(false);
		}

		public void AddRow(string text)
		{
			ComboboxPopupRow comboboxPopupRow = Object.Instantiate(_row, base.transform, false);
			comboboxPopupRow.gameObject.SetActive(true);
			comboboxPopupRow.Text = text;
			_rows.Add(comboboxPopupRow);
			int index = _rows.Count - 1;
			Button requireComponent = comboboxPopupRow.GetRequireComponent<Button>();
			requireComponent.onClick.AddListener(delegate
			{
				Hide();
				EventHandler selectEventHandler = SelectEventHandler;
				if (selectEventHandler != null)
				{
					selectEventHandler(index);
				}
			});
		}

		public void Clear()
		{
			foreach (ComboboxPopupRow row in _rows)
			{
				Object.Destroy(row.gameObject);
			}
			_rows.Clear();
		}

		public void OnDeselect(BaseEventData eventData)
		{
			Invoke("Hide", 0.2f);
		}

		public override void Show()
		{
			base.Show();
			if (base.transform.parent == _originalParent)
			{
				base.transform.SetParent(ParentTransform);
			}
		}

		public override void Hide()
		{
			base.Hide();
			if (base.transform.parent == ParentTransform)
			{
				base.transform.SetParent(_originalParent);
			}
		}
	}
}
