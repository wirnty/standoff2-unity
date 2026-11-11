using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	public class WeaponMap : MonoBehaviour
	{
		public enum WeaponPart
		{
			Magazine1 = 1,
			Magazine2 = 2,
			Magazine3 = 3,
			Gunlock1 = 4,
			Gunlock2 = 5,
			Weapon = 6,
			MuzzlePoint = 7
		}

		public GameObject Magazine1 { get; private set; }

		public GameObject Magazine2 { get; private set; }

		public GameObject Magazine3 { get; private set; }

		public GameObject Gunlock1 { get; private set; }

		public GameObject Gunlock2 { get; private set; }

		public GameObject Weapon { get; private set; }

		public GameObject MuzzlePoint { get; private set; }

		internal void Initialize()
		{
			Weapon = base.gameObject;
			Transform obj = base.transform.Find(WeaponPart.Magazine1.ToString());
			Magazine1 = (((object)obj != null) ? obj.gameObject : null);
			Transform obj2 = base.transform.Find(WeaponPart.Magazine2.ToString());
			Magazine2 = (((object)obj2 != null) ? obj2.gameObject : null);
			Transform obj3 = base.transform.Find(WeaponPart.Magazine3.ToString());
			Magazine3 = (((object)obj3 != null) ? obj3.gameObject : null);
			Transform obj4 = base.transform.Find(WeaponPart.Gunlock1.ToString());
			Gunlock1 = (((object)obj4 != null) ? obj4.gameObject : null);
			Transform obj5 = base.transform.Find(WeaponPart.Gunlock2.ToString());
			Gunlock2 = (((object)obj5 != null) ? obj5.gameObject : null);
			Transform obj6 = base.transform.Find(WeaponPart.MuzzlePoint.ToString());
			MuzzlePoint = (((object)obj6 != null) ? obj6.gameObject : null);
		}

		public void SetLayer(int layer)
		{
			if (Magazine1 != null)
			{
				Magazine1.layer = layer;
			}
			if (Magazine2 != null)
			{
				Magazine2.layer = layer;
			}
			if (Magazine2 != null)
			{
				Magazine2.layer = layer;
			}
			if (Gunlock1 != null)
			{
				Gunlock1.layer = layer;
			}
			if (Gunlock2 != null)
			{
				Gunlock2.layer = layer;
			}
			if (Weapon != null)
			{
				Weapon.layer = layer;
			}
		}

		public GameObject GetWeaponPart(WeaponPart part)
		{
			switch (part)
			{
			case WeaponPart.Gunlock1:
				return Gunlock1;
			case WeaponPart.Gunlock2:
				return Gunlock2;
			case WeaponPart.Magazine1:
				return Magazine1;
			case WeaponPart.Magazine2:
				return Magazine2;
			default:
				switch (part)
				{
				case WeaponPart.Magazine2:
					return Magazine3;
				case WeaponPart.Weapon:
					return Weapon;
				default:
					return null;
				}
			}
		}
	}
}
