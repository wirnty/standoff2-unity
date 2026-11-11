using System;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main
{
	[RequireComponent(typeof(Image))]
	[RequireComponent(typeof(Button))]
	public class SidebarButton : View
	{
		[SerializeField]
		[NotNull]
		private Image _iconImage;

		[SerializeField]
		[NotNull]
		private Sprite _sprite;

		[SerializeField]
		[NotNull]
		private Sprite _selectedSprite;

		private Image _glowImage;

		public Action OnClickEvent { get; set; }

		public bool Selected
		{
			get
			{
				return _iconImage.sprite == _selectedSprite;
			}
			set
			{
				_iconImage.sprite = ((!value) ? _sprite : _selectedSprite);
				_glowImage.enabled = value;
			}
		}

		private void Awake()
		{
			Button requireComponent = this.GetRequireComponent<Button>();
			requireComponent.onClick.AddListener(delegate
			{
				if (!Selected)
				{
					Action onClickEvent = OnClickEvent;
					if (onClickEvent != null)
					{
						onClickEvent();
					}
				}
			});
			_glowImage = this.GetRequireComponent<Image>();
		}
	}
}
