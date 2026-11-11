using System;
using System.Collections.Generic;

namespace Axlebolt.Standoff.Inventory.Animation
{
	[Serializable]
	public class WeaponAnimationClip
	{
		public List<string> sourceAnimationStateNameList = new List<string>();

		public List<int> sourceAnimationStateNameHashList = new List<int>();

		public string targetAnimationStateName = "";

		public int targetAnimationStateNameHash;

		public float clipDuration = 0f;

		public float clipSpeed = 0f;

		public List<WeaponAnimationEvent> eventList = new List<WeaponAnimationEvent>();

		public List<WeaponAnimationMarker> markerList = new List<WeaponAnimationMarker>();

		public List<WeaponAnimationSegment> segmentsList = new List<WeaponAnimationSegment>();
	}
}
