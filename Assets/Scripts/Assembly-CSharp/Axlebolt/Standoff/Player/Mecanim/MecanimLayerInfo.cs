using System;
using UnityEngine.Networking;

namespace Axlebolt.Standoff.Player.Mecanim
{
	public class MecanimLayerInfo : MessageBase
	{
		public float time;

		public int layerInd;

		public bool isInTransition;

		public int stateFPNameHash;

		public float stateNormalizedTime;

		public float stateLength;

		public int nextStateNameHash;

		public int transitionNameHash;

		public float transitionNormalizedTime;

		public float transitionDuration;

		public float transitionStartTime;

		public override void Deserialize(NetworkReader reader)
		{
			time = reader.ReadSingle();
			layerInd = reader.ReadByte();
			isInTransition = reader.ReadBoolean();
			stateFPNameHash = reader.ReadInt32();
			stateNormalizedTime = (float)reader.ReadInt16() / 1000f;
			stateLength = (float)reader.ReadInt16() / 1000f;
			if (isInTransition)
			{
				nextStateNameHash = reader.ReadInt32();
				transitionNameHash = stateFPNameHash;
				transitionNormalizedTime = stateNormalizedTime;
				transitionDuration = stateLength;
			}
		}

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(time);
			writer.Write((byte)layerInd);
			writer.Write(isInTransition);
			if (isInTransition)
			{
				writer.Write(transitionNameHash);
				float num = transitionNormalizedTime - (float)Math.Truncate(transitionNormalizedTime);
				writer.Write((short)(num * 1000f));
				writer.Write((short)(transitionDuration * 1000f));
				writer.Write(nextStateNameHash);
			}
			else
			{
				writer.Write(stateFPNameHash);
				float num = stateNormalizedTime - (float)Math.Truncate(stateNormalizedTime);
				writer.Write((short)(num * 1000f));
				writer.Write((short)(stateLength * 1000f));
			}
		}

		public float GetPastTransTime()
		{
			return transitionNormalizedTime * transitionDuration;
		}

		public MecanimLayerInfo Clone()
		{
			return (MecanimLayerInfo)MemberwiseClone();
		}
	}
}
