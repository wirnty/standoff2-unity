using System.Collections.Generic;
using Axlebolt.Standoff.Player;
using UnityEngine;

namespace Axlebolt.Standoff.Inventory.Animation
{
	public class WeaponAnimationTool : MonoBehaviour
	{
		public WeaponAnimationParameters WeaponAnimationParameters;

		[Space(5f)]
		[RequiredField]
		public Animator characterAnimator;

		[Space(5f)]
		[RequiredField]
		public PlayerController playerController;

		[RequiredField]
		[Space(5f)]
		public Transform weaponDirection;

		[RequiredField]
		[Space(5f)]
		public WeaponAnimationController weaponAnimationController;

		[HideInInspector]
		public WeaponAnimationClip currentAnimationClip;

		[HideInInspector]
		public float currentPlayback = 0f;

		public void AddNewClip(WeaponAnimationClip animationClip)
		{
			animationClip.sourceAnimationStateNameHashList = new List<int>();
			foreach (string sourceAnimationStateName in animationClip.sourceAnimationStateNameList)
			{
				animationClip.sourceAnimationStateNameHashList.Add(Animator.StringToHash(sourceAnimationStateName));
			}
			animationClip.targetAnimationStateNameHash = Animator.StringToHash(animationClip.targetAnimationStateName);
			WeaponAnimationParameters.animationClipList.Add(animationClip);
		}

		public void RemoveClip(string targetStateName)
		{
			WeaponAnimationClip weaponAnimationClip = null;
			foreach (WeaponAnimationClip animationClip in WeaponAnimationParameters.animationClipList)
			{
				if (animationClip.targetAnimationStateName == targetStateName)
				{
					weaponAnimationClip = animationClip;
					break;
				}
			}
			if (weaponAnimationClip != null)
			{
				if (currentAnimationClip == weaponAnimationClip)
				{
					currentAnimationClip = null;
				}
				WeaponAnimationParameters.animationClipList.Remove(weaponAnimationClip);
			}
		}

		public void Play()
		{
			weaponAnimationController.AnimationParameters = WeaponAnimationParameters;
			weaponAnimationController.Play(currentAnimationClip.sourceAnimationStateNameHashList[0], currentPlayback, Time.deltaTime);
			characterAnimator.Play(currentAnimationClip.sourceAnimationStateNameHashList[0], 1, currentPlayback);
			characterAnimator.Update(0f);
		}
	}
}
