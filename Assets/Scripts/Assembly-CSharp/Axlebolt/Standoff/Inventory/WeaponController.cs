using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Inventory.Animation;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Player.Inventory;
using Axlebolt.Standoff.Player.Mecanim;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory
{
	[RequireComponent(typeof(MeshLodGroup))]
	[RequireComponent(typeof(WeaponAnimationController))]
	[RequireComponent(typeof(WeaponMap))]
	public abstract class WeaponController : MonoBehaviour
	{
		public enum HandleState
		{
			Primary = 0,
			Secondary = 1,
			NotStated = 2
		}

		internal const int FpsLodLayer = 0;

		internal const int TpsLodLayer = 1;

		internal GameObject WeaponHierarchy;

		internal AudioSource AnimationAudioSource;

		internal PlayerController PlayerController;

		internal MecanimController MecanimController;

		internal WeaponAnimationController AnimationController;

		internal WeaponAnimationParameters AnimationParameters;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly byte _003CInventoryInstanceId_003Ek__BackingField;

		protected InventoryControllerCmd Commands;

		public float LocalTime { get; private set; }

		public float DeltaTime { get; private set; }

		public byte InventoryInstanceId
		{
			[CompilerGenerated]
			get
			{
				return _003CInventoryInstanceId_003Ek__BackingField;
			}
		}

		public MeshLodGroup LodGroup { get; private set; }

		public HandleState HandlingState { get; private set; } = HandleState.Secondary;


		public byte WeaponId
		{
			[CompilerGenerated]
			get
			{
				return (byte)WeaponParameters.Id;
			}
		}

		public string WeaponName
		{
			[CompilerGenerated]
			get
			{
				return WeaponParameters.DisplayName;
			}
		}

		public WeaponParameters WeaponParameters { get; private set; }

		public abstract byte SlotIndex { get; }

		public WeaponMap WeaponMap { get; private set; }

		public WeaponDropController DropController { get; private set; }

		protected WeaponController()
		{
			_003CInventoryInstanceId_003Ek__BackingField = 0;
		}

		internal AnimatorStateInfo GetCurrentCharacterWeaponAnimationState()
		{
			return MecanimController.GetAnimatorStateInfo(1);
		}

		internal virtual void PreInitialize(WeaponParameters weaponParameters, WeaponAnimationParameters animationParameters)
		{
			WeaponMap = GetComponent<WeaponMap>();
			WeaponMap.Initialize();
			WeaponParameters = weaponParameters;
			AnimationParameters = animationParameters;
			AnimationController = GetComponent<WeaponAnimationController>();
			WeaponHierarchy = base.gameObject;
			AnimationAudioSource = base.gameObject.AddComponent<AudioSource>();
			AnimationAudioSource.playOnAwake = false;
			DropController = base.gameObject.AddComponent<WeaponDropController>();
			DropController.PreInitialize(this);
			LodGroup = GetComponent<MeshLodGroup>();
		}

		internal virtual void Initialize()
		{
			AnimationController.SetActive(true);
		}

		public virtual void SetPlayer(PlayerController playerController)
		{
			HandlingState = HandleState.NotStated;
			PlayerController = playerController;
			LocalTime = playerController.LocalTime;
			MecanimController = playerController.MecanimController;
			AnimationController.OnPlayerSet(playerController);
		}

		public void OnCharacterSkinSet(BipedMap bipedMap)
		{
			AnimationController.OnCharacterSkinSet(bipedMap);
		}

		internal virtual void SetViewMode(ViewMode viewMode)
		{
			switch (viewMode)
			{
			case ViewMode.FPS:
				WeaponMap.SetLayer(8);
				LodGroup.SetLayer(0);
				break;
			case ViewMode.TPS:
				WeaponMap.SetLayer(0);
				LodGroup.SetLayer(1);
				break;
			default:
				throw new NotSupportedException("Unsupported view mode " + viewMode);
			}
		}

		public virtual void SetAsDefault(float time)
		{
			LocalTime = time;
			HandlingState = HandleState.Primary;
			AnimationController.OnBecomePrimary();
			base.gameObject.SetActive(true);
		}

		public virtual void SetAsSecondary()
		{
			HandlingState = HandleState.Secondary;
			base.gameObject.SetActive(false);
		}

		public virtual AccuracyData GetAccuracyData()
		{
			return new AccuracyData();
		}

		public virtual void OnAnimatorUpdated()
		{
		}

		public virtual bool IsDroppable()
		{
			return false;
		}

		public abstract WeaponSnapshot GetSnapshot();

		public abstract void SetSnapshot(WeaponSnapshot parameters);

		public virtual void ExecuteCommands(InventoryControllerCmd commands, float duration, float time)
		{
			Commands = commands;
			LocalTime = time;
			DeltaTime = duration;
		}

		public virtual void SetVisible(bool isVisible)
		{
			if (isVisible)
			{
				if (HandlingState == HandleState.Primary)
				{
					base.gameObject.SetActive(true);
				}
			}
			else
			{
				base.gameObject.SetActive(false);
			}
			AnimationController.SetActive(isVisible);
		}
	}
}
