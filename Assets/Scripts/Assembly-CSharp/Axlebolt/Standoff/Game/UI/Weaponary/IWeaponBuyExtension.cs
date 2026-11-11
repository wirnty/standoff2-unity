using Axlebolt.Standoff.Inventory;

namespace Axlebolt.Standoff.Game.UI.Weaponary
{
	public interface IWeaponBuyExtension
	{
		string GetCustomParameter();

		string GetCustomParameterValue(WeaponParameters weapon);
	}
}
