using Axlebolt.Standoff.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main
{
	public class SearchGameView : View
	{
		[NotNull]
		[SerializeField]
		private Text _onlinePlayerText;

		[SerializeField]
		[NotNull]
		private Text _regionText;

		[SerializeField]
		[NotNull]
		private Text _pingText;

		public string OnlinePlayers
		{
			set
			{
				_onlinePlayerText.text = value;
			}
		}

		public string Region
		{
			set
			{
				_regionText.text = value;
			}
		}

		public int Ping
		{
			set
			{
				_pingText.text = value.ToString();
			}
		}

		private void Awake()
		{
			_onlinePlayerText.text = "";
			_regionText.text = "";
			_pingText.text = "";
		}
	}
}
