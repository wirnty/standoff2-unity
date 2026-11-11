using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Player;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public class PhotonPrefabPool : IPunPrefabPool
	{
		private static readonly Log Log = Log.Create(typeof(PhotonPrefabPool));

		public PhotonPrefabPool()
		{
			foreach (string id in Singleton<PlayerManager>.Instance.GetIds())
			{
				PhotonNetwork.PrefabCache[id] = Singleton<PlayerManager>.Instance.PlayerController.gameObject;
				Log.Debug(string.Format("Player {0} registered in PhotonCache", id));
			}
		}

		public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
		{
			Log.Debug(string.Format("Get from pool {0}", prefabId));
			return Singleton<PlayerManager>.Instance.GetFromPool(prefabId, position, rotation);
		}

		public void Destroy(GameObject gameObject)
		{
			Log.Debug(string.Format("Return to pool {0}", gameObject.name));
			Singleton<PlayerManager>.Instance.ReturnToPool(gameObject);
		}
	}
}
