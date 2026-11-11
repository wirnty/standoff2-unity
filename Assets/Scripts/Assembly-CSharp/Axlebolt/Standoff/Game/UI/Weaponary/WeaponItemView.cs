using System;
using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Weaponary
{
	[RequireComponent(typeof(Button), typeof(Image))]
	public class WeaponItemView : View
	{
		[SerializeField]
		private Text _nameText;

		[SerializeField]
		private Image _weaponIcon;

		[SerializeField]
		private Text _customAttributeText;

		[SerializeField]
		private Color _selectableColor;

		[SerializeField]
		private Color _notSelectableColor;

		[SerializeField]
		private Sprite _notSelectedSprite;

		[SerializeField]
		private Sprite _selectedSprite;

		private Button _button;

		private Image _image;

		private bool _selectable;

		private bool _selected;

		public string Name
		{
			set
			{
				_nameText.text = value;
			}
		}

		public string CustomParameter
		{
			set
			{
				_customAttributeText.text = value;
			}
		}

		public Sprite WeaponIcon
		{
			set
			{
				_weaponIcon.sprite = value;
			}
		}

		public Action OnSelected { get; set; }

		public bool IsSelectable
		{
			get
			{
				return _selectable;
			}
			set
			{
				_selectable = value;
				Color color = ((!_selectable) ? _notSelectableColor : _selectableColor);
				_nameText.color = color;
				_weaponIcon.color = color;
				_customAttributeText.color = color;
			}
		}

		public bool IsSelected
		{
			get
			{
				return _selected;
			}
			set
			{
				if (_selectable && _selected != value)
				{
					_selected = value;
					if (_selected)
					{
						_image.sprite = _selectedSprite;
						FireOnSelected();
					}
					else
					{
						_image.sprite = _notSelectedSprite;
					}
				}
			}
		}

		private void Awake()
		{
			_button = GetComponent<Button>();
			_image = GetComponent<Image>();
			_button.onClick.AddListener(delegate
			{
				IsSelected = true;
			});
			_image.sprite = _notSelectedSprite;
			_selected = false;
		}

		private void FireOnSelected()
		{
			Action onSelected = OnSelected;
			if (onSelected != null)
			{
				onSelected();
			}
		}
	}
}
