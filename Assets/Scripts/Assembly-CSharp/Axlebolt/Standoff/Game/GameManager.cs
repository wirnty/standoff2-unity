using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.UI;
using Axlebolt.Standoff.Level;
using Axlebolt.Standoff.Photon;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Settings;
using Axlebolt.Standoff.UI;
using I2.Loc;
using JetBrains.Annotations;
using Photon;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Axlebolt.Standoff.Game
{
	public class GameManager : PunBehaviour
	{
		public delegate void ExitHandler(string withError);

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CExit_003Ec__async0 : IAsyncStateMachine
		{
			internal string withError;

			internal GameManager _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				try
				{
					switch (num)
					{
					default:
						return;
					case 0u:
					{
						PhotonNetworkExtension.MessageListeners.Remove(_0024this);
						_0024this._gameController.gameObject.SetActive(false);
						PhotonPlayer player = PhotonNetwork.player;
						if (player != null)
						{
							player.Clear();
						}
						_0024awaiter0 = PhotonAsync.Disconnect().GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						break;
					}
					case 1u:
						break;
					}
					_0024awaiter0.GetResult();
					_playerAttr = null;
					_exitHandler(withError);
				}
				catch (Exception exception)
				{
					_0024PC = -1;
					_0024builder.SetException(exception);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult();
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003COnDisconnectedFromPhoton_003Ec__async3 : IAsyncStateMachine
		{
			internal Dialog _003Cdialog_003E__1;

			internal float _003Ctime_003E__1;

			internal Exception _0024locvar0;

			internal GameManager _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				bool flag = false;
				try
				{
					switch (num)
					{
					default:
						return;
					case 0u:
						PhotonNetworkExtension.MessageListeners.Remove(_0024this);
						_0024this._gameController.Pause();
						_003Cdialog_003E__1 = Dialogs.Create(ScriptLocalization.Dialogs.ConnectionHasBeenLost, ScriptLocalization.Dialogs.TryingToReconnect);
						_003Cdialog_003E__1.SetTimer(new DialogTimer.CountDown(20f));
						_003Cdialog_003E__1.Show();
						_003Ctime_003E__1 = Time.time;
						Log.Debug("Trying to reconnect");
						goto IL_0176;
					case 1u:
					{
						Exception ex2 = default(Exception);
						int num2 = default(int);
						try
						{
							switch (num)
							{
							default:
								_0024awaiter0 = PhotonAsync.ReconnectAndRejoin(false).GetAwaiter();
								if (!_0024awaiter0.IsCompleted)
								{
									_0024PC = 1;
									flag = true;
									_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
									return;
								}
								break;
							case 1u:
								break;
							}
							_0024awaiter0.GetResult();
						}
						catch (Exception ex)
						{
							ex2 = ex;
							num2 = 1;
						}
						switch (num2)
						{
						case 1:
							break;
						default:
							goto IL_0176;
						}
						_0024locvar0 = ex2;
						_0024awaiter0 = PhotonAsync.Disconnect().GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 2;
							flag = true;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						goto case 2u;
					}
					case 2u:
						{
							_0024awaiter0.GetResult();
							goto IL_0176;
						}
						IL_0176:
						if (PhotonNetwork.inRoom || !(Time.time - _003Ctime_003E__1 < 20f))
						{
							break;
						}
						num = 4294967293u;
						goto case 1u;
					}
					_003Cdialog_003E__1.Hide();
					if (PhotonNetwork.inRoom)
					{
						Log.Debug("Reconnected successfully");
						PhotonNetworkExtension.MessageListeners.Add(_0024this);
						_0024this._gameController.Resume();
						PhotonNetwork.isMessageQueueRunning = true;
					}
					else
					{
						Log.Debug("Reconnection failed");
						PhotonNetwork.isMessageQueueRunning = true;
						_0024this.Exit(ScriptLocalization.GameExitMessages.ReconnectFailed);
					}
				}
				catch (Exception exception)
				{
					_0024PC = -1;
					_0024builder.SetException(exception);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult();
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		private static readonly Log Log = Log.Create(typeof(GameManager));

		private static ExitHandler _exitHandler;

		private static PlayerAttr _playerAttr;

		private static GameManager _instance;

		[NotNull]
		[SerializeField]
		private LevelLoadingView _loadingView;

		private GameMode _gameMode;

		private LevelDefinition _level;

		private AsyncOperation _levelLoadingAsync;

		private GameController _gameController;

		public static GameManager Instance
		{
			get
			{
				if (_instance == null)
				{
					throw new InvalidOperationException("Are you in game?");
				}
				return _instance;
			}
		}

		public static bool InGame
		{
			[CompilerGenerated]
			get
			{
				return _instance != null;
			}
		}

		public static void InitGame([NotNull] PlayerAttr playerAttr, [NotNull] ExitHandler exitHandler)
		{
			if (playerAttr == null)
			{
				throw new ArgumentNullException("playerAttr");
			}
			if (exitHandler == null)
			{
				throw new ArgumentNullException("exitHandler");
			}
			_playerAttr = playerAttr;
			_exitHandler = exitHandler;
			PhotonNetwork.isMessageQueueRunning = false;
			SetupSendRate();
			SceneManager.LoadScene("Game", LoadSceneMode.Single);
		}

		private void Awake()
		{
			_instance = this;
			if (!PhotonNetwork.inRoom)
			{
				Exit(ScriptLocalization.GameExitMessages.Disconnected);
				return;
			}
			try
			{
				_level = GetLevel();
				_gameMode = GetGameMode();
				LoadLevel();
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);
				_exitHandler(ex.Message);
			}
		}

		private static void SetupSendRate()
		{
			PhotonNetwork.sendRate = 15;
			PhotonNetwork.sendRateOnSerialize = 15;
		}

		internal void ReInitGame()
		{
			InitGame(_playerAttr, _exitHandler);
		}

		[AsyncStateMachine(typeof(_003CExit_003Ec__async0))]
		[DebuggerStepThrough]
		internal void Exit(string withError = null)
		{
			_003CExit_003Ec__async0 stateMachine = default(_003CExit_003Ec__async0);
			stateMachine.withError = withError;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		private LevelDefinition GetLevel()
		{
			string levelName = PhotonNetwork.room.GetLevelName();
			return LevelUtility.GetLevel(levelName);
		}

		private GameMode GetGameMode()
		{
			string gameModeName = PhotonNetwork.room.GetGameModeName();
			return GameModeUtility.GetByName(gameModeName);
		}

		private void LoadLevel()
		{
			StartCoroutine(LoadLevelAsync());
		}

		private IEnumerator LoadLevelAsync()
		{
			_loadingView.Level = _level;
			_loadingView.GameMode = _gameMode;
			_loadingView.LoadingText = ScriptLocalization.LevelLoadingView.Loading;
			_loadingView.Show();
			_levelLoadingAsync = SceneManager.LoadSceneAsync(_level.SceneName, LoadSceneMode.Additive);
			SettingsManager.Instance.Apply();
			while (!_levelLoadingAsync.isDone)
			{
				_loadingView.Progress = _levelLoadingAsync.progress;
				yield return null;
			}
			_levelLoadingAsync = null;
			if (PhotonNetwork.inRoom)
			{
				InitPhotonPlayer();
				_gameController = UnityEngine.Object.Instantiate(_gameMode.Controller);
				_gameController.Init(_level);
				yield return new WaitForSeconds(0.1f);
				PhotonNetwork.isMessageQueueRunning = true;
				yield return WaitForInit();
			}
			else
			{
				Exit(ScriptLocalization.GameExitMessages.Disconnected);
			}
		}

		private static void InitPhotonPlayer()
		{
			PhotonNetwork.player.Clear();
			PhotonNetwork.playerName = _playerAttr.Name;
			AvatarSupport.SetPlayerAvatar(_playerAttr.Avatar);
		}

		private IEnumerator WaitForInit()
		{
			_loadingView.Progress = 1f;
			_loadingView.LoadingText = ScriptLocalization.LevelLoadingView.InitWaiting;
			while (PhotonNetwork.inRoom && !PhotonNetwork.isMasterClient && PhotonNetwork.room.GetGameStateId() == 0)
			{
				yield return null;
			}
			if (PhotonNetwork.inRoom)
			{
				StartGame();
			}
			else
			{
				Exit(ScriptLocalization.GameExitMessages.Disconnected);
			}
		}

		private void StartGame()
		{
			PhotonNetworkExtension.MessageListeners.Add(this);
			UnityEngine.Object.Destroy(Camera.main.gameObject);
			_loadingView.Hide();
			_gameController.StartGame();
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(_level.SceneName));
		}

		[AsyncStateMachine(typeof(_003COnDisconnectedFromPhoton_003Ec__async3))]
		[DebuggerStepThrough]
		public override void OnDisconnectedFromPhoton()
		{
			_003COnDisconnectedFromPhoton_003Ec__async3 stateMachine = default(_003COnDisconnectedFromPhoton_003Ec__async3);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		private void OnDestroy()
		{
			PhotonNetworkExtension.MessageListeners.Remove(this);
			_instance = null;
		}
	}
}
