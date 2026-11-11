using System.Runtime.CompilerServices;
using I2.Loc;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	[CreateAssetMenu(fileName = "NewLevel", menuName = "Standoff/Create Game Mode", order = 1)]
	public class GameMode : ScriptableObject
	{
		[SerializeField]
		private GameController _controller;

		public string LocalizedName { get; private set; }

		public string LocalizedDescription { get; private set; }

		public string[] LocalizedSettings { get; private set; }

		public GameController Controller
		{
			[CompilerGenerated]
			get
			{
				return _controller;
			}
		}

		public byte MaxPlayers
		{
			[CompilerGenerated]
			get
			{
				return _controller.MaxPlayers;
			}
		}

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return base.name;
			}
		}

		private void OnEnable()
		{
			LocalizedName = ScriptLocalization.Get(string.Format("GameMode/{0}", Name));
			LocalizedDescription = ScriptLocalization.Get(string.Format("GameMode/{0}Description", Name));
			LocalizedSettings = new string[2];
			LocalizedSettings[0] = ((!_controller.FriendlyFireOn) ? "OFF" : "ON");
			LocalizedSettings[1] = ((!_controller.TeamCollisionOn) ? "OFF" : "ON");
		}
	}
}
