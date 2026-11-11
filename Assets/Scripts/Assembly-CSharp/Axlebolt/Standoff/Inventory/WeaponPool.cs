using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Inventory.Animation;
using Axlebolt.Standoff.Inventory.Gun;
using Axlebolt.Standoff.Settings;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	public class WeaponPool : SimplePool<byte, WeaponController, WeaponPool.PrefabAttr, WeaponPool.InstanceAttr>
	{
		public class PrefabAttr
		{
			public byte Id;

			public WeaponParameters Parameters;

			public WeaponAnimationParameters AnimationParameters;

			public GameObject Model;

			public WeaponMaterial FpsMaterial;

			public WeaponMaterial TpsMaterial;
		}

		public class InstanceAttr
		{
			public byte Id;

			public MeshLodGroup LodGroup;
		}

		private static readonly Log Log = Log.Create(typeof(WeaponPool));

		private readonly Transform _parent;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly WeaponParameters[] _003CWeapons_003Ek__BackingField;

		public WeaponParameters[] Weapons
		{
			[CompilerGenerated]
			get
			{
				return _003CWeapons_003Ek__BackingField;
			}
		}

		public WeaponPool([NotNull] WeaponParameters[] weapons, int poolSize, [NotNull] Transform parent)
		{
			if (weapons == null)
			{
				throw new ArgumentNullException("weapons");
			}
			if (parent == null)
			{
				throw new ArgumentNullException("parent");
			}
			_003CWeapons_003Ek__BackingField = weapons;
			_parent = parent;
			Init(poolSize);
		}

		public WeaponParameters GetParameters(byte weaponId)
		{
			return Prefabs[weaponId].Parameters;
		}

		protected override Dictionary<byte, PrefabAttr> InitPrefabs()
		{
			Dictionary<byte, PrefabAttr> dictionary = new Dictionary<byte, PrefabAttr>();
			WeaponParameters[] weapons = Weapons;
			foreach (WeaponParameters weaponParameters in weapons)
			{
				GameObject gameObject = WeaponUtility.LoadModel(weaponParameters);
				if (gameObject.GetComponent<MeshLodGroup>() == null)
				{
					Log.Error(string.Format("MeshLodGroup component not found {0}", weaponParameters.name));
					throw new InvalidOperationException();
				}
				WeaponAnimationParameters animationParameters = WeaponUtility.LoadAnimationParameters(weaponParameters);
				PrefabAttr prefabAttr = new PrefabAttr();
				prefabAttr.Id = weaponParameters.NumId;
				prefabAttr.Parameters = weaponParameters;
				prefabAttr.AnimationParameters = animationParameters;
				prefabAttr.Model = gameObject;
				PrefabAttr value = prefabAttr;
				dictionary[weaponParameters.NumId] = value;
			}
			return dictionary;
		}

		protected override Pair Create(byte key, PrefabAttr character)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(character.Model);
			gameObject.transform.SetParent(_parent);
			MeshLodGroup component = gameObject.GetComponent<MeshLodGroup>();
			gameObject.SetActive(false);
			InstanceAttr instanceAttr = new InstanceAttr
			{
				Id = character.Id,
				LodGroup = component
			};
			WeaponController weaponController = AddController(character.Parameters, gameObject);
			weaponController.PreInitialize(character.Parameters, character.AnimationParameters);
			component.LayerChanged = delegate
			{
				UpdateMaterial(weaponController, instanceAttr, character);
			};
			Pair pair = new Pair();
			pair.Instance = weaponController;
			pair.Attr = instanceAttr;
			return pair;
		}

		private static WeaponController AddController(WeaponParameters weapon, GameObject prefab)
		{
			if (weapon is GunParameters)
			{
				return prefab.AddComponent<GunController>();
			}
			throw new NotSupportedException();
		}

		protected override void LoadMaterial(PrefabAttr prefabAttr)
		{
			UnloadMaterials(prefabAttr);
			WeaponParameters parameters = prefabAttr.Parameters;
			QualityLvl shaderDetail = QualityManager.Instance.ShaderDetail;
			if (shaderDetail >= QualityLvl.High)
			{
				prefabAttr.FpsMaterial = WeaponUtility.LoadPbrMaterial(parameters);
				prefabAttr.TpsMaterial = WeaponUtility.LoadBumpedDiffuse(parameters);
			}
			else if (shaderDetail >= QualityLvl.Medium)
			{
				prefabAttr.FpsMaterial = WeaponUtility.LoadBumpedSpecular(parameters);
				prefabAttr.TpsMaterial = WeaponUtility.LoadDiffuseMaterial(parameters);
			}
			else if (shaderDetail >= QualityLvl.Low)
			{
				prefabAttr.FpsMaterial = WeaponUtility.LoadBumpedDiffuse(parameters);
				prefabAttr.TpsMaterial = WeaponUtility.LoadDiffuseMaterial(parameters);
			}
			else
			{
				prefabAttr.FpsMaterial = WeaponUtility.LoadDiffuseMaterial(parameters);
				prefabAttr.TpsMaterial = WeaponUtility.LoadDiffuseMaterial(parameters);
			}
		}

		private static void UnloadMaterials(PrefabAttr prefabAttr)
		{
			if (prefabAttr.FpsMaterial != null)
			{
				ResourcesUtility.Unload(prefabAttr.FpsMaterial);
				prefabAttr.FpsMaterial = null;
			}
			if (prefabAttr.TpsMaterial != null)
			{
				ResourcesUtility.Unload(prefabAttr.TpsMaterial);
				prefabAttr.TpsMaterial = null;
			}
		}

		protected override void UpdateMaterial(WeaponController instance, InstanceAttr attr, PrefabAttr prefabAttr)
		{
			WeaponMaterial weaponMaterial = ((attr.LodGroup.CurrentLayer != 0) ? prefabAttr.TpsMaterial : prefabAttr.FpsMaterial);
			MeshRenderer[] meshRenderers = attr.LodGroup.MeshRenderers;
			MeshRenderer[] array = meshRenderers;
			foreach (MeshRenderer meshRenderer in array)
			{
				meshRenderer.sharedMaterials = new Material[1] { weaponMaterial.MainMaterial };
			}
		}

		public override WeaponController GetFromPool(byte key)
		{
			WeaponController fromPool = base.GetFromPool(key);
			fromPool.Initialize();
			return fromPool;
		}

		public override void ReturnToPool(WeaponController weapon)
		{
			weapon.gameObject.SetActive(false);
			weapon.transform.SetParent(_parent);
			base.ReturnToPool(weapon);
		}

		public void RefreshLods()
		{
			foreach (KeyValuePair<WeaponController, InstanceAttr> instanceAttr in InstanceAttrs)
			{
				instanceAttr.Value.LodGroup.RefreshLod();
			}
		}
	}
}
