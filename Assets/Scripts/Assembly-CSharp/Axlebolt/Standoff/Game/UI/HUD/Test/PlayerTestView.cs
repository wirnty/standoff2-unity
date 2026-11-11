using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Game.UI.HUD.Test
{
	public class PlayerTestView : View
	{
		[SerializeField]
		private Image _traceImg;

		[SerializeField]
		private Image _cycleImg;

		[SerializeField]
		private Image _halfcycleImg;

		public PlayerController PlayerController { get; set; }

		private void Update()
		{
			if (PlayerController != null)
			{
				_cycleImg.fillAmount = PlayerController.ArmsAnimController._curFootstepCycleProgress;
				_traceImg.fillAmount = PlayerController.ArmsAnimController._curFootstepTraceProgress;
				_halfcycleImg.fillAmount = PlayerController.ArmsAnimController._halfOffsetFootstepCycleProgress;
			}
		}
	}
}
