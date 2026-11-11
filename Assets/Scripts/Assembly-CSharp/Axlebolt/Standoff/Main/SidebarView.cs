using System;
using System.Collections.Generic;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Main
{
	public class SidebarView : View
	{
		[NotNull]
		[SerializeField]
		private SidebarButton _playButton;

		[SerializeField]
		[NotNull]
		private SidebarButton _profileButton;

		[SerializeField]
		[NotNull]
		private SidebarButton _inventoryButton;

		[SerializeField]
		[NotNull]
		private SidebarButton _clanButton;

		[NotNull]
		[SerializeField]
		private SidebarButton _friendsButton;

		[SerializeField]
		[NotNull]
		private SidebarButton _settingsButton;

		private readonly List<SidebarButton> _buttons = new List<SidebarButton>();

		public Action PlayButtonEvent { get; set; }

		public Action ProfileButtonEvent { get; set; }

		public Action InventoryButtonEvent { get; set; }

		public Action ClanButtonEvent { get; set; }

		public Action FriendsButtonEvent { get; set; }

		public Action VideoSettingsButtonEvent { get; set; }

		public Action ControlSettingsButtonEvent { get; set; }
	}
}
