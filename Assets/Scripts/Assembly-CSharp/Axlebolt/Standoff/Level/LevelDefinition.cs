using System.Runtime.CompilerServices;
using Axlebolt.Standoff.Player;
using UnityEngine;

namespace Axlebolt.Standoff.Level
{
	[CreateAssetMenu(fileName = "NewLevel", menuName = "Standoff/Create Level", order = 1)]
	public class LevelDefinition : ScriptableObject
	{
		[SerializeField]
		private string _displayName;

		[SerializeField]
		private Sprite _image;

		[SerializeField]
		private Sprite _schemaImage;

		[SceneName]
		[SerializeField]
		private string _sceneName;

		[SerializeField]
		private GameModes _gameModes;

		[Space]
		[Header("Counter Terrorist")]
		[SerializeField]
		[PlayerArms]
		private string _ctArms;

		[SerializeField]
		private PlayerCharacters _ctCharacters;

		[Space]
		[SerializeField]
		[Header("Terrorist")]
		[PlayerArms]
		private string _trArms;

		[SerializeField]
		private PlayerCharacters _trCharacters;

		public string Name
		{
			[CompilerGenerated]
			get
			{
				return base.name;
			}
		}

		public string DisplayName
		{
			[CompilerGenerated]
			get
			{
				return _displayName;
			}
		}

		public string SceneName
		{
			[CompilerGenerated]
			get
			{
				return _sceneName;
			}
		}

		public string[] GameModes
		{
			[CompilerGenerated]
			get
			{
				return _gameModes.value;
			}
		}

		public string CtArms
		{
			[CompilerGenerated]
			get
			{
				return _ctArms;
			}
		}

		public PlayerCharacters CtCharacters
		{
			[CompilerGenerated]
			get
			{
				return _ctCharacters;
			}
		}

		public string TrArms
		{
			[CompilerGenerated]
			get
			{
				return _trArms;
			}
		}

		public PlayerCharacters TrCharacters
		{
			[CompilerGenerated]
			get
			{
				return _trCharacters;
			}
		}

		public Sprite Image
		{
			[CompilerGenerated]
			get
			{
				return _image;
			}
		}

		public Sprite SchemaImage
		{
			[CompilerGenerated]
			get
			{
				return _schemaImage;
			}
		}
	}
}
