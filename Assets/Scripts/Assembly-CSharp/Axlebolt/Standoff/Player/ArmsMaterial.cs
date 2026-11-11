using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	[CreateAssetMenu(fileName = "NewMaterial", menuName = "Standoff/Create Arms Material", order = 5)]
	public class ArmsMaterial : ScriptableObject
	{
		[NotNull]
		[SerializeField]
		private Material _material;

		[NotNull]
		public Material Material
		{
			[CompilerGenerated]
			get
			{
				return _material;
			}
		}
	}
}
