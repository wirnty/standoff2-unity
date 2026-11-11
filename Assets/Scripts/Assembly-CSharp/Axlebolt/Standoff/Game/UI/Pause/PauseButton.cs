using System;
using Axlebolt.Standoff.UI;
using UnityEngine;

namespace Axlebolt.Standoff.Game.UI.Pause
{
	public class PauseButton : View
	{
		public Action OnClick { get; set; }

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				Action onClick = OnClick;
				if (onClick != null)
				{
					onClick();
				}
			}
		}
	}
}
