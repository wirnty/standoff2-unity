using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Settings;
using Axlebolt.Standoff.Utils;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public class PlayerPool : SimplePool<string, PlayerController, BipedMap, PlayerPool.InstanceAttr>
	{
		public class InstanceAttr
		{
			public SkinnedMeshLodGroup CharacterLodGroup;

			public SkinnedMeshLodGroup ArmsLodGroup;
		}

		private readonly string _arms;

		private readonly PlayerController _playerController;

		private readonly PlayerHitboxConfig _playerHitboxConfig;

		private readonly PlayerCharacters _characters;

		private readonly BipedMap _armsBiped;

		private ArmsMaterial _armsMaterial;

		public string[] Characters
		{
			[CompilerGenerated]
			get
			{
				return _characters.Skins;
			}
		}

		public PlayerPool([NotNull] PlayerController playerController, [NotNull] PlayerHitboxConfig playerHitboxConfig, [NotNull] string arms, [NotNull] PlayerCharacters characters, int poolSize)
		{
			if (playerController == null)
			{
				throw new ArgumentNullException("playerController");
			}
			if (playerHitboxConfig == null)
			{
				throw new ArgumentNullException("playerHitboxConfig");
			}
			if (arms == null)
			{
				throw new ArgumentNullException("arms");
			}
			if (characters == null)
			{
				throw new ArgumentNullException("characters");
			}
			_playerController = playerController;
			_playerHitboxConfig = playerHitboxConfig;
			_arms = arms;
			_characters = characters;
			_armsBiped = PlayerUtility.LoadArms(arms);
			ProcessArms();
			Init(poolSize);
		}

		private void ProcessArms()
		{
			UnityUtility.SetLayerRecursively(_armsBiped.gameObject, 8);
			if (_armsBiped.GetComponent<SkinnedMeshLodGroup>() == null)
			{
				throw new ArgumentException(string.Format("{0} does not contain SkinnedMeshLodGroup component", _armsBiped));
			}
		}

		protected override Dictionary<string, BipedMap> InitPrefabs()
		{
			Dictionary<string, BipedMap> dictionary = PlayerUtility.LoadCharacters(_characters);
			foreach (KeyValuePair<string, BipedMap> item in dictionary)
			{
				if (item.Value.GetComponent<SkinnedMeshLodGroup>() == null)
				{
					throw new ArgumentException(string.Format("{0} does not contain SkinnedMeshLodGroup component", item.Value));
				}
			}
			return dictionary;
		}

		protected override Pair Create(string key, BipedMap character)
		{
			PlayerController playerController = UnityEngine.Object.Instantiate(_playerController);
			BipedMap bipedMap = UnityEngine.Object.Instantiate(_armsBiped);
			BipedMap bipedMap2 = UnityEngine.Object.Instantiate(character);
			playerController.PreInitialize(bipedMap2, bipedMap, _playerHitboxConfig);
			playerController.gameObject.SetActive(false);
			InstanceAttr instanceAttr = new InstanceAttr();
			instanceAttr.ArmsLodGroup = bipedMap.GetRequireComponent<SkinnedMeshLodGroup>();
			instanceAttr.CharacterLodGroup = bipedMap2.GetRequireComponent<SkinnedMeshLodGroup>();
			InstanceAttr attr = instanceAttr;
			Pair pair = new Pair();
			pair.Instance = playerController;
			pair.Attr = attr;
			return pair;
		}

		protected override void LoadMaterials()
		{
			base.LoadMaterials();
			if (_armsMaterial != null)
			{
				ResourcesUtility.Unload(_armsMaterial);
			}
			QualityLvl shaderDetail = QualityManager.Instance.ShaderDetail;
			if (shaderDetail >= QualityLvl.High)
			{
				_armsMaterial = PlayerUtility.LoadArmsPbrMaterial(_arms);
			}
			else if (shaderDetail >= QualityLvl.Medium)
			{
				_armsMaterial = PlayerUtility.LoadArmsBumpedSpecular(_arms);
			}
			else if (shaderDetail >= QualityLvl.Low)
			{
				_armsMaterial = PlayerUtility.LoadArmsBumpedDiffuse(_arms);
			}
			else
			{
				_armsMaterial = PlayerUtility.LoadArmsDiffuseMaterial(_arms);
			}
		}

		protected override void LoadMaterial(BipedMap prefabAttr)
		{
		}

		protected override void UpdateMaterial(PlayerController instance, InstanceAttr attr, BipedMap prefabAttr)
		{
			SkinnedMeshRenderer[] skinnedMeshRenderers = attr.ArmsLodGroup.SkinnedMeshRenderers;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
			{
				skinnedMeshRenderer.sharedMaterial = _armsMaterial.Material;
			}
		}

		public string GetFreeCharacter()
		{
			foreach (KeyValuePair<string, Queue<PlayerController>> item in MainPool)
			{
				if (item.Value.Count > 0)
				{
					return item.Key;
				}
			}
			int num = UnityEngine.Random.Range(0, _characters.Skins.Length);
			return _characters.Skins[num];
		}

		public void RefreshLods()
		{
			foreach (InstanceAttr value in InstanceAttrs.Values)
			{
				value.ArmsLodGroup.RefreshLod();
				value.CharacterLodGroup.RefreshLod();
			}
		}
	}
}
