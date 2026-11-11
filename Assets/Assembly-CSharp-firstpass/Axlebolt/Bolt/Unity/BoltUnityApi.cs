using Axlebolt.RpcSupport;
using UnityEngine;

namespace Axlebolt.Bolt.Unity
{
	public class BoltUnityApi : MonoBehaviour, ILogStream
	{
		private readonly ILogger _logger = UnityEngine.Debug.unityLogger;

		public static void Init(string gameId)
		{
			GameObject gameObject = new GameObject("BoltUnityApi");
			gameObject.AddComponent<BoltUnityApi>();
			BoltApi.FileCache.CachePath = Application.temporaryCachePath;
			BoltApi.Init(gameId);
		}

		private void Awake()
		{
			Axlebolt.RpcSupport.Logger.Stream = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}

		public void Debug(object msg)
		{
			_logger.Log("Bolt", msg);
		}

		public void Error(object msg)
		{
			_logger.LogError("Bolt", msg);
		}

		private void OnDestroy()
		{
			BoltApi.Destroy();
		}
	}
}
