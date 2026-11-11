using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Axlebolt.Standoff.Game.Event
{
	public class PlayerPropertiesEventArg
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly IEnumerable<object> _003CChangedProperties_003Ek__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private readonly PhotonPlayer _003CPlayer_003Ek__BackingField;

		public IEnumerable<object> ChangedProperties
		{
			[CompilerGenerated]
			get
			{
				return _003CChangedProperties_003Ek__BackingField;
			}
		}

		public PhotonPlayer Player
		{
			[CompilerGenerated]
			get
			{
				return _003CPlayer_003Ek__BackingField;
			}
		}

		public PlayerPropertiesEventArg([NotNull] IEnumerable<object> changedProperties, [NotNull] PhotonPlayer player)
		{
			if (changedProperties == null)
			{
				throw new ArgumentNullException("changedProperties");
			}
			if (player == null)
			{
				throw new ArgumentNullException("player");
			}
			_003CChangedProperties_003Ek__BackingField = changedProperties;
			_003CPlayer_003Ek__BackingField = player;
		}

		public bool Contains(object property)
		{
			return ChangedProperties.Contains(property);
		}
	}
}
