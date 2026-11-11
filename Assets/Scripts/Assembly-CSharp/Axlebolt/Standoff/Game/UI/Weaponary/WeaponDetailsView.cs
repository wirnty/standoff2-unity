using System.Linq;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.Weaponary
{
	public class WeaponDetailsView : View
	{
		[SerializeField]
		[NotNull]
		private Text _nameText;

		[SerializeField]
		[NotNull]
		private Image _previewImage;

		[SerializeField]
		[NotNull]
		private Text _costTitle;

		[SerializeField]
		[NotNull]
		private Text _costText;

		[SerializeField]
		[NotNull]
		private GunParametersView _gunParamtersView;

		private void Awake()
		{
			_gunParamtersView.Show();
			Clear();
		}

		public void SetInventories(WeaponParameters[] inventories)
		{
			GunParameters[] guns = inventories.Where((WeaponParameters inventory) => inventory is GunParameters).Cast<GunParameters>().ToArray();
			_gunParamtersView.SetGuns(guns);
		}

		public void ShowDeatils(WeaponParameters weapon)
		{
			_nameText.text = weapon.DisplayName;
			_previewImage.gameObject.SetActive(true);
			_costTitle.gameObject.SetActive(true);
			_previewImage.sprite = weapon.Sprites.Preview;
			_costText.text = "$" + weapon.Cost;
			GunParameters gunParameters = weapon as GunParameters;
			if (gunParameters != null)
			{
				_gunParamtersView.ShowGun(gunParameters);
			}
		}

		public void Clear()
		{
			_nameText.text = "";
			_previewImage.gameObject.SetActive(false);
			_costTitle.gameObject.SetActive(false);
			_costText.text = "";
			_gunParamtersView.Hide();
		}
	}
}
