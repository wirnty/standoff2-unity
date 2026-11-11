using System.Linq;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Inventory.Animation;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	public static class WeaponUtility
	{
		private const string Model = "_Model";

		private const string AnimationParameters = "_Anim";

		public static WeaponParameters[] LoadWeapons()
		{
			return ResourcesUtility.LoadAll<WeaponParameters>("Weapons").Where(IsValidWeapon).ToArray();
		}

		public static WeaponParameters LoadWeapon(string name)
		{
			return ResourcesUtility.Load<WeaponParameters>("Weapons/" + name + "/" + name);
		}

		public static bool IsValidWeapon(WeaponParameters weapon)
		{
			return true;
		}

		public static WeaponMaterial LoadPbrMaterial(WeaponParameters weapon)
		{
			return ResourcesUtility.Load<WeaponMaterial>("Weapons/" + weapon.name + "/" + weapon.name + "_PBR");
		}

		public static WeaponMaterial LoadBumpedSpecular(WeaponParameters weapon)
		{
			return ResourcesUtility.Load<WeaponMaterial>("Weapons/" + weapon.name + "/" + weapon.name + "_BumpedSpecular");
		}

		public static WeaponMaterial LoadBumpedDiffuse(WeaponParameters weapon)
		{
			return ResourcesUtility.Load<WeaponMaterial>("Weapons/" + weapon.name + "/" + weapon.name + "_BumpedDiffuse");
		}

		public static WeaponMaterial LoadDiffuseMaterial(WeaponParameters weapon)
		{
			return ResourcesUtility.Load<WeaponMaterial>("Weapons/" + weapon.name + "/" + weapon.name + "_Diffuse");
		}

		public static GameObject LoadModel(WeaponParameters weapon)
		{
			return ResourcesUtility.Load<GameObject>("Weapons/" + weapon.name + "/" + weapon.name + "_Model");
		}

		public static WeaponAnimationParameters LoadAnimationParameters(WeaponParameters weapon)
		{
			return ResourcesUtility.Load<WeaponAnimationParameters>("Weapons/" + weapon.name + "/" + weapon.name + "_Anim");
		}
	}
}
