using UnityEngine;

namespace Axlebolt.Standoff.Player
{
	public abstract class BaseController<TSnapshot, TCmdType> : MonoBehaviour
	{
		public abstract void SetSnapshot(TSnapshot parameters);

		public abstract TSnapshot GetSnapshot();

		public abstract void ExecuteCommands(TCmdType commands, float duration, float time);

		public abstract void PreInitialize();

		public abstract void Initialize();
	}
}
