using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public class PlayerAttr
	{
		private static readonly string[] RandomNames = new string[4] { "ChuckNorris", "BruceWillis", "JackieChan", "Gaben" };

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly string _003CName_003Ek__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly byte[] _003CAvatar_003Ek__BackingField;

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return _003CName_003Ek__BackingField;
			}
		}

		public byte[] Avatar
		{
			[CompilerGenerated]
			get
			{
				return _003CAvatar_003Ek__BackingField;
			}
		}

		public PlayerAttr(string name, byte[] avatar)
		{
			_003CName_003Ek__BackingField = name;
			_003CAvatar_003Ek__BackingField = avatar;
		}

		public static PlayerAttr Random()
		{
			string name = RandomNames[UnityEngine.Random.Range(0, RandomNames.Length)] + UnityEngine.Random.Range(0, 100);
			return new PlayerAttr(name, null);
		}
	}
}
