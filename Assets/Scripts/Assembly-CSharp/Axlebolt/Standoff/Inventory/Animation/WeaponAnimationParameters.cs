using System.Collections.Generic;
using Axlebolt.Standoff.Common;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Animation
{
	public class WeaponAnimationParameters : ScriptableObject
	{
		public int sourceAnimationStatesLayer = 1;

		public RuntimeAnimatorController animatorController;

		public RuntimeAnimatorController lHandAnimatorController;

		public RuntimeAnimatorController rHandAnimatorController;

		public List<WeaponAnimationClip> animationClipList = new List<WeaponAnimationClip>();

		public TransformTR fpsCamOffset;

		public TransformTR spineFPSDirectorTR;

		public TransformTR weaponDirectorOnStand;

		public TransformTR weaponDirectorOnCrouch;

		public float ArmsShakeMult;

		public string magazine1AnimationName = "";

		public string magazine2AnimationName = "";

		public string magazine3AnimationName = "";

		public string gunlock1AnimationName = "";

		public string gunlock2AnimationName = "";

		[Header("Sound Parameters")]
		[Space(20f)]
		public int audioSourcesCount;

		public AudioClip shotSound;

		public List<AnimationSoundEvent> soundEventlist = new List<AnimationSoundEvent>();
	}
}
