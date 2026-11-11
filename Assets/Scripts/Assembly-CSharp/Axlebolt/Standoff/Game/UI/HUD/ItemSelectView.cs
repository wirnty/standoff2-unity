using System.Collections.Generic;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.HUD
{
	public class ItemSelectView : View
	{
		[SerializeField]
		private List<ItemSelectViewItem> _selectableItems;

		[SerializeField]
		private Color _reloadImgColorSelected;

		[SerializeField]
		private Color _reloadImgColorUnselected;

		[SerializeField]
		private Image _reloadImg;

		public override void Show()
		{
			base.Show();
			foreach (ItemSelectViewItem selectableItem in _selectableItems)
			{
				selectableItem.SetSelected(false);
				selectableItem.FadeIn();
			}
		}

		public void SetWeaponList(List<WeaponController> weapons)
		{
			if (weapons.Count > 4)
			{
				Debug.LogError("Slot Length Can't Be More Than > 4");
			}
			for (int i = 0; i < weapons.Count; i++)
			{
				_selectableItems[i].SetWeapon(weapons[i]);
			}
		}

		public void SetSelectedSlot(int slotNO)
		{
			foreach (ItemSelectViewItem selectableItem in _selectableItems)
			{
				selectableItem.SetSelected(false);
			}
			if (slotNO == 0)
			{
				_reloadImg.color = _reloadImgColorSelected;
				return;
			}
			_reloadImg.color = _reloadImgColorUnselected;
			_selectableItems[slotNO - 1].SetSelected(true);
		}

		private void Update()
		{
		}
	}
}
