using System.Collections.Generic;
using UnityEngine;

namespace Axlebolt.Standoff.Player.Mecanim
{
	public class PlayerMecanimConfig : ScriptableObject
	{
		public List<MecanimTransitionInfo> transitionList;

		public float maxTransitionNormalizedTimeDeviation;

		public float maxStateNormalizedTimeDeviation;

		public List<string> nonSyncParatetres = new List<string>();

		public RuntimeAnimatorController animatorController;

		public Avatar avatar;
	}
}
