using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Axlebolt.Standoff.CameraEffects;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Effects;
using Axlebolt.Standoff.Game.Event;
using Axlebolt.Standoff.Game.State;
using Axlebolt.Standoff.Game.UI;
using Axlebolt.Standoff.Game.UI.GameStats;
using Axlebolt.Standoff.Game.UI.Pause;
using Axlebolt.Standoff.Game.UI.Statistics;
using Axlebolt.Standoff.Game.UI.TeamSelect;
using Axlebolt.Standoff.Game.UI.Weaponary;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Level;
using Axlebolt.Standoff.Player;
using Axlebolt.Standoff.Player.Ragdoll;
using Axlebolt.Standoff.Settings;
using Axlebolt.Standoff.UI;
using ExitGames.Client.Photon;
using I2.Loc;
using JetBrains.Annotations;
using Photon;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public abstract class GameController : PunBehaviour
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CPlayDieWithFocusEffect_003Ec__async0 : IAsyncStateMachine
		{
			internal RagdollController _003Cragdoll_003E__1;

			internal PhotonPlayer killer;

			internal PlayerController _003CplayerController_003E__1;

			internal GameController _0024this;

			internal AsyncTaskMethodBuilder _0024builder;

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
						_0024this.ScreenshotView.Hide();
						_003Cragdoll_003E__1 = Singleton<RagdollManager>.Instance.GetActiveRagdollByPlayerId(_0024this.Player.ID);
						_003CplayerController_003E__1 = Singleton<PlayerManager>.Instance.GetController(killer.ID);
						if (_003Cragdoll_003E__1 == null)
						{
							Log.Error("Can't show effect, ragdoll not found");
						}
						else
						{
							if (!(_003CplayerController_003E__1 == null))
							{
								_0024awaiter0 = Singleton<CameraEffectsController>.Instance.PlayDieWithFocus(_0024this._mainCamera, _003Cragdoll_003E__1.Character, _003CplayerController_003E__1.transform, _0024this.ScreenshotView.RawImage).GetAwaiter();
								if (!_0024awaiter0.IsCompleted)
								{
									_0024PC = 1;
									_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
									return;
								}
								break;
							}
							Log.Error("Can't show effect, killer player not found");
						}
						goto end_IL_000e;
					case 1u:
						break;
					}
					_0024awaiter0.GetResult();
					end_IL_000e:;
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

		private static readonly Log Log = Log.Create(typeof(GameController));

		public const int AssistMinDamage = 41;

		public readonly Event<GameInitEventArgs> GameInitEvent = new Event<GameInitEventArgs>();

		public readonly Event<GameExitEventArgs> GameExitEvent = new Event<GameExitEventArgs>();

		public readonly Event<PhotonPlayer> PlayerConnectedEvent = new Event<PhotonPlayer>();

		public readonly Event<PhotonPlayer> PlayerDisconnectedEvent = new Event<PhotonPlayer>();

		public readonly Event<PlayerPropertiesEventArg> PlayerPropertiesChangedEvent = new Event<PlayerPropertiesEventArg>();

		public readonly Event<HitEventArgs> PlayerWasKilledEvent = new Event<HitEventArgs>();

		public readonly Event<HitEventArgs> PlayerKilledEvent = new Event<HitEventArgs>();

		public readonly Event<HitEventArgs> PlayerAssistEvent = new Event<HitEventArgs>();

		[SerializeField]
		private bool _teamCollisionOn;

		[SerializeField]
		private bool _friendlyFireOn;

		[SerializeField]
		private byte _maxPlayers;

		private GameStateMachine _stateMachine;

		private SpawnPointManager _spawnPointManager;

		private LevelDefinition _level;

		private Camera _fpsCamera;

		private Camera _mainCamera;

		private Dictionary<int, GamePlayer> _gamePlayers;

		private WeaponParameters[] _allWeapons;

		protected TeamSelectView TeamSelectView;

		protected WeaponBuyView WeaponBuyView;

		protected WeaponBuyButton WeaponBuyButton;

		protected StatisticsView StatisticsView;

		protected StatisticsButton StatisticsButton;

		protected NotificationsView NotificationsView;

		protected PersonalGameStatsView PersonalGameStatsView;

		protected KillLogView KillLogView;

		protected PauseButton PauseButton;

		protected PauseView PauseView;

		protected ScreenshotView ScreenshotView;

		protected WhoKillView WhoKillView;

		public static GameController Instance { get; private set; }

		public bool TeamCollisionOn
		{
			[CompilerGenerated]
			get
			{
				return _teamCollisionOn;
			}
		}

		public bool FriendlyFireOn
		{
			[CompilerGenerated]
			get
			{
				return _friendlyFireOn;
			}
		}

		public byte MaxPlayers
		{
			[CompilerGenerated]
			get
			{
				return _maxPlayers;
			}
		}

		public GameView GameView { get; private set; }

		public Controls Controls { get; private set; }

		protected IGameState CurrentGameState
		{
			[CompilerGenerated]
			get
			{
				return _stateMachine.GameState;
			}
		}

		protected byte CurrentGameStateId
		{
			[CompilerGenerated]
			get
			{
				return _stateMachine.GameState.Id;
			}
		}

		public virtual GameType GameType
		{
			[CompilerGenerated]
			get
			{
				return GameType.Personal;
			}
		}

		public float PlayerSpawnedTime { get; private set; }

		public PlayerController PlayerController { get; private set; }

		public PhotonPlayer Player
		{
			[CompilerGenerated]
			get
			{
				return PhotonNetwork.player;
			}
		}

		public void Init([NotNull] LevelDefinition level)
		{
			if (level == null)
			{
				throw new ArgumentNullException("level");
			}
			if (Instance != null)
			{
				throw new InvalidOperationException("GameController already initialized");
			}
			Instance = this;
			_level = level;
			InitGamePlayers();
			InitManagers();
			InitSceneObjects();
			InitSpawnPointProvider();
			PhotonNetworkExtension.MessageListeners.Add(this);
			StartCoroutine(PhotonGameExtension.UpdatePing());
			string gameModeName = PhotonNetwork.room.GetGameModeName();
			GameInitEvent.Invoke(new GameInitEventArgs(gameModeName, _maxPlayers));
		}

		public void StartGame()
		{
			CreateUi();
			InitUi();
			InitCamera();
			BindHitManager();
			StartStateMachine();
		}

		private void InitCamera()
		{
			GameObject original = ResourcesUtility.Load<GameObject>("Camera/FpsCamera");
			GameObject original2 = ResourcesUtility.Load<GameObject>("Camera/MainCamera");
			_fpsCamera = UnityEngine.Object.Instantiate(original).GetRequireComponent<Camera>();
			_mainCamera = UnityEngine.Object.Instantiate(original2).GetRequireComponent<Camera>();
		}

		private void CreateUi()
		{
			Canvas requireComponent = GameObject.Find("Canvas").GetRequireComponent<Canvas>();
			Controls = CreateControls(requireComponent);
			GameView = CreateGameView(requireComponent);
		}

		public static GameView CreateGameView(Canvas canvas)
		{
			GameObject original = ResourcesUtility.Load<GameObject>("UI/GameView");
			GameObject gameObject = UnityEngine.Object.Instantiate(original, canvas.transform, false);
			return gameObject.GetRequireComponent<GameView>();
		}

		public static Controls CreateControls(Canvas canvas)
		{
			GameObject original = ResourcesUtility.Load<GameObject>("UI/HUDView");
			GameObject gameObject = UnityEngine.Object.Instantiate(original, canvas.transform, false);
			return gameObject.GetRequireComponent<Controls>();
		}

		protected virtual void InitUi()
		{
			GameView.Init();
			TeamSelectView = GameView.GetView<TeamSelectView>();
			PersonalGameStatsView = GameView.GetView<PersonalGameStatsView>();
			NotificationsView = GameView.GetView<NotificationsView>();
			StatisticsButton = GameView.GetView<StatisticsButton>();
			StatisticsView = GameView.GetView<StatisticsView>();
			WeaponBuyButton = GameView.GetView<WeaponBuyButton>();
			WeaponBuyView = GameView.GetView<WeaponBuyView>();
			KillLogView = GameView.GetView<KillLogView>();
			PauseButton = GameView.GetView<PauseButton>();
			PauseView = GameView.GetView<PauseView>();
			ScreenshotView = GameView.GetView<ScreenshotView>();
			WhoKillView = GameView.GetView<WhoKillView>();
			InitWeaponBuyView();
			InitStatisticsView();
			InitTeamSelectView();
			InitPauseView();
		}

		protected virtual void InitWeaponBuyView()
		{
			if (IsWeaponBuySupport())
			{
				WeaponBuyView.Init(this);
				WeaponBuyButton.Clicked = ShowWeaponBuyView;
				WeaponBuyView.Closed = HideWeaponBuyView;
				WeaponBuyView.VisibleChanged += DisableControlsHandler(WeaponBuyView);
			}
		}

		protected virtual void InitStatisticsView()
		{
			StatisticsView.Init("", "");
			StatisticsButton.Show();
			StatisticsButton.Down = delegate
			{
				StatisticsView.Show();
			};
			StatisticsButton.Up = delegate
			{
				StatisticsView.Hide();
			};
		}

		protected virtual void InitTeamSelectView()
		{
			TeamSelectView.Init(this);
			TeamSelectView.LevelImage = _level.Image;
			TeamSelectView.VisibleChanged += DisableControlsHandler(TeamSelectView);
		}

		protected virtual void InitPauseView()
		{
			PauseButton.Show();
			PauseButton.OnClick = delegate
			{
				PauseView.Show();
			};
			PauseView.VisibleChanged += DisableControlsHandler(PauseView);
			PauseView.Main.AddItem(ScriptLocalization.PauseMenu.BackToGame, delegate
			{
				PauseView.Hide();
			}, KeyCode.Escape);
			if (IsTeamSelectEnabled())
			{
				PauseView.Main.AddItem(ScriptLocalization.PauseMenu.ChangeTeam, delegate
				{
					PauseView.Hide();
					TeamSelectView.Show();
				});
			}
			PauseMenuView settingsMenu = InitSettingsView();
			PauseView.Main.AddItem(ScriptLocalization.PauseMenu.Settings, delegate
			{
				PauseView.Main.Hide();
				settingsMenu.Show();
			});
			PauseView.Main.AddItem(ScriptLocalization.PauseMenu.ExitGame, delegate
			{
				ConfirmExitGame();
				PauseView.Hide();
			});
		}

		protected virtual PauseMenuView InitSettingsView()
		{
			SettingsController settingsController = SettingsController.Create(PauseView.transform.parent);
			PauseMenuView settingsMenu = PauseView.CreateMenu(ScriptLocalization.PauseMenu.Settings);
			settingsController.VisibleChanged += delegate(bool visible)
			{
				settingsMenu.IsVisible = !visible;
			};
			settingsMenu.AddItem(ScriptLocalization.VideoSettings.Title, delegate
			{
				settingsController.ShowVideoSettings();
			});
			settingsMenu.AddItem(ScriptLocalization.ControlSettings.Title, delegate
			{
				settingsController.ShowControlSettings();
			});
			settingsMenu.AddItem(ScriptLocalization.Common.Back, delegate
			{
				settingsMenu.Hide();
				PauseView.Main.Show();
			}, KeyCode.Escape);
			return settingsMenu;
		}

		private View.VisibleChangedHandler DisableControlsHandler(UnityEngine.Object view)
		{
			return delegate(bool visible)
			{
				if (visible)
				{
					Controls.RequestDisable(view);
				}
				else
				{
					Controls.RequestEnable(view);
				}
			};
		}

		private void InitSpawnPointProvider()
		{
			_spawnPointManager = new SpawnPointManager(SpawnPointManager.Type.TeamRandomSpawnArea);
		}

		private void InitManagers()
		{
			EffectsModule.Init();
			Singleton<WeaponManager>.Instance.Init(GetAllWeapons());
			Singleton<PlayerManager>.Instance.Init(_level.CtArms, _level.CtCharacters, _level.TrArms, _level.TrCharacters);
			Singleton<PlayerManager>.Instance.LocalInstantiateEvent += OnPlayerSpawned;
			Singleton<PlayerManager>.Instance.RemoteInstantiateEvent += OnRemotePlayerSpawned;
			Singleton<PlayerManager>.Instance.LocalDestroyEvent += OnDestroyed;
			Singleton<AvatarSupport>.Instance.Init(_maxPlayers);
			Singleton<CameraEffectsController>.Instance.Initialize();
			PhotonNetwork.PrefabPool = new PhotonPrefabPool();
		}

		private void InitSceneObjects()
		{
			GameObject[] array = ResourcesUtility.LoadAll<GameObject>("SceneObjects");
			GameObject[] array2 = array;
			foreach (GameObject original in array2)
			{
				UnityEngine.Object.Instantiate(original, base.transform);
			}
		}

		private void InitGamePlayers()
		{
			_gamePlayers = new Dictionary<int, GamePlayer>();
			PhotonPlayer[] playerList = PhotonNetwork.playerList;
			foreach (PhotonPlayer photonPlayer in playerList)
			{
				_gamePlayers.Add(photonPlayer.ID, new GamePlayer(photonPlayer));
			}
		}

		private void StartStateMachine()
		{
			_stateMachine = new GameStateMachine(CreateStateRouter());
			_stateMachine.GameInit += OnGameInit;
			_stateMachine.GameFinished += OnGameFinished;
			RegisterStates(_stateMachine);
			_stateMachine.Setup(this);
			StartCoroutine(_stateMachine.GameLoop());
		}

		protected abstract GameStateRouter CreateStateRouter();

		protected abstract void RegisterStates(GameStateMachine stateMachine);

		protected virtual void OnGameInit()
		{
			if (IsTeamSelectEnabled())
			{
				TeamSelectView.Show();
			}
		}

		private void OnGameFinished()
		{
			ExitGame();
		}

		protected virtual void BindHitManager()
		{
			Singleton<HitManager>.Instance.Init(_friendlyFireOn);
			Singleton<HitManager>.Instance.HitEvent.AddListener(OnHit);
			Singleton<HitManager>.Instance.SuicideEvent.AddListener(OnSuicide);
		}

		protected virtual bool IsTeamSelectEnabled()
		{
			return true;
		}

		public bool CanChangeTeamTo(Team team)
		{
			if (team == Team.None)
			{
				Log.Error("Invalid argument, can't change team to None");
				return false;
			}
			int num = PhotonNetwork.playerList.GetByTeam(Team.Tr, true).Length;
			int num2 = PhotonNetwork.playerList.GetByTeam(Team.Ct, true).Length;
			if (team == Team.Tr)
			{
				num++;
			}
			else
			{
				num2++;
			}
			return Math.Abs(num - num2) <= 1;
		}

		public void ChangeTeamTo(Team team)
		{
			if (PhotonNetwork.player.GetTeam() == team)
			{
				TeamSelectView.Hide();
				return;
			}
			PhotonNetwork.player.SetTeam(team);
			if (PlayerController != null)
			{
				KillPlayer();
			}
			else
			{
				SpawnPlayer();
			}
		}

		protected virtual void OnHit(HitEventArgs hitEventArgs)
		{
			GamePlayer gamePlayer = GetGamePlayer(hitEventArgs.Victim);
			gamePlayer.AddHit(hitEventArgs.Shooter, hitEventArgs.Damage);
			if (hitEventArgs.Victim.IsDead())
			{
				HitLogs hits = gamePlayer.GetHits(hitEventArgs.Shooter);
				HitLogs assistHits = gamePlayer.GetAssistHits(hitEventArgs.Shooter, 41);
				KillLogView.LogKill(hitEventArgs.Shooter, (assistHits != null) ? assistHits.Player : null, hitEventArgs.Victim, hitEventArgs.Weapon, hitEventArgs.Headshot, hitEventArgs.Penetrated);
				if (hitEventArgs.Shooter.IsLocal)
				{
					OnPlayerKilled(hitEventArgs);
					PlayerKilledEvent.Invoke(hitEventArgs);
				}
				if (((assistHits != null) ? assistHits.Player : null) != null && assistHits.Player.IsLocal)
				{
					OnPlayerAssist(hitEventArgs);
					PlayerAssistEvent.Invoke(hitEventArgs);
				}
				if (hitEventArgs.Victim.IsLocal)
				{
					OnPlayerWasKilled(hitEventArgs, hits);
					PlayerWasKilledEvent.Invoke(hitEventArgs);
				}
			}
		}

		protected virtual void OnSuicide(PhotonPlayer photonPlayer)
		{
			if (photonPlayer.IsLocal)
			{
				OnPlayerSuicide();
			}
		}

		protected virtual void OnRemotePlayerSpawned(PlayerController playerController)
		{
			GetGamePlayer(playerController.Player).ClearHits();
		}

		protected virtual void OnPlayerSpawned(PlayerController playerController)
		{
			GetGamePlayer(playerController.Player).ClearHits();
			TeamSelectView.Hide();
			ScreenshotView.Hide();
			WhoKillView.Hide();
			PlayerController = playerController;
			PlayerController.SetMainCamera(_mainCamera);
			PlayerController.SetFpsCamera(_fpsCamera);
			PlayerController.SetFPSView();
			Controls.gameObject.SetActive(true);
			Controls.PlayerController = PlayerController;
			PlayerSpawnedTime = Time.time;
		}

		protected virtual void OnPlayerKilled(HitEventArgs hitEventArgs)
		{
			Player.AddKill();
		}

		protected virtual void OnPlayerAssist(HitEventArgs hitEventArgs)
		{
			Player.AddAssist();
		}

		protected virtual void OnPlayerWasKilled(HitEventArgs hitEventArgs, HitLogs takenHits)
		{
			Player.AddDeath();
		}

		protected virtual void OnPlayerSuicide()
		{
			Player.AddDeath();
		}

		protected virtual void OnDestroyed(PlayerController playerController)
		{
			NotificationsView.Hide();
			WeaponBuyButton.Hide();
			HideWeaponBuyView();
			Controls.PlayerController = null;
			Controls.gameObject.SetActive(false);
			PlayerController = null;
		}

		protected virtual bool CanSpawnPlayer()
		{
			return _stateMachine.GameState.CanSpawnPlayer();
		}

		public void SpawnPlayer()
		{
			Log.Debug("SpawnPlayer");
			if (!CanSpawnPlayer())
			{
				Log.Debug("Can't spawn player, state rejected");
				return;
			}
			Team team = PhotonNetwork.player.GetTeam();
			if (team == Team.None)
			{
				Log.Debug("Team None, skip spawn");
				return;
			}
			if (PlayerController != null)
			{
				DestroyPlayer();
			}
			SpawnPoint spawnPoint = _spawnPointManager.GetSpawnPoint(team);
			InstantiatePlayer(team, spawnPoint);
		}

		protected virtual void InstantiatePlayer(Team team, SpawnPoint spawnPoint)
		{
			Log.Debug("InstantiatePlayer");
			Singleton<PlayerManager>.Instance.Instantiate(team, spawnPoint.Position, spawnPoint.Rotation);
		}

		protected void KillPlayer()
		{
			if (PlayerController != null)
			{
				PlayerController.Die();
			}
		}

		public void DestroyPlayer()
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("DestroyPlayer");
			}
			if (PlayerController != null)
			{
				PlayerController.DestroyPlayer();
			}
		}

		[AsyncStateMachine(typeof(_003CPlayDieWithFocusEffect_003Ec__async0))]
		[DebuggerStepThrough]
		protected Task PlayDieWithFocusEffect(PhotonPlayer killer)
		{
			_003CPlayDieWithFocusEffect_003Ec__async0 stateMachine = default(_003CPlayDieWithFocusEffect_003Ec__async0);
			stateMachine.killer = killer;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		public void ShowWeaponBuyView()
		{
			if (IsWeaponBuySupport() && CanBuyWeapons())
			{
				WeaponBuyView.Show();
			}
		}

		public void HideWeaponBuyView()
		{
			WeaponBuyView.Hide();
		}

		public virtual bool IsWeaponBuySupport()
		{
			return true;
		}

		public virtual bool CanBuyWeapons()
		{
			return PlayerController != null;
		}

		public bool CanBuyWeapons(WeaponParameters weapon)
		{
			return true;
		}

		public virtual void BuyWeapon(WeaponParameters weapon)
		{
			SetPlayerWeapon(weapon);
		}

		protected void SetPlayerWeapon(WeaponParameters weapon)
		{
			if (PlayerController != null)
			{
				WeaponController weapon2 = Singleton<WeaponManager>.Instance.Get(weapon.NumId);
				PlayerController.InventoryController.SetWeapon(weapon2);
			}
			else
			{
				Log.Warning("Can't set weapon, player not spawned");
			}
		}

		public WeaponParameters[] GetPlayerWeapons()
		{
			if (PlayerController != null)
			{
				return (from controller in PlayerController.InventoryController.GetWeapons()
					select controller.WeaponParameters).ToArray();
			}
			Log.Warning("Can't get weapon's, player not spawned");
			return new WeaponParameters[0];
		}

		public virtual WeaponParameters[] GetAllWeapons()
		{
			return _allWeapons ?? (_allWeapons = WeaponUtility.LoadWeapons());
		}

		private void Update()
		{
			if (!(_level == null) && _stateMachine != null && _level.name != PhotonNetwork.room.GetLevelName())
			{
				GameManager.Instance.ReInitGame();
			}
		}

		protected virtual void ConfirmExitGame()
		{
			DialogButton yesButton = new DialogButton(ScriptLocalization.Common.Yes);
			DialogButton dialogButton = new DialogButton(ScriptLocalization.Common.No);
			Dialog dialog = Dialogs.Create(ScriptLocalization.Common.Confirmation, ScriptLocalization.Dialogs.ExitGameConfirmation, yesButton, dialogButton);
			dialog.Show(delegate(DialogButton btn)
			{
				if (btn == yesButton)
				{
					ExitGame();
				}
			});
		}

		public virtual void Pause()
		{
			base.gameObject.SetActive(false);
		}

		public virtual void Resume()
		{
			base.gameObject.SetActive(true);
			StartCoroutine(PhotonGameExtension.UpdatePing());
			if (_stateMachine != null)
			{
				StartCoroutine(_stateMachine.GameLoop());
			}
		}

		public virtual void ExitGame()
		{
			GameExitEvent.Invoke(new GameExitEventArgs());
			GameManager.Instance.Exit();
		}

		public override void OnPhotonPlayerConnected(PhotonPlayer photonPlayer)
		{
			GameView gameView = GameView;
			if ((object)gameView != null)
			{
				gameView.Refresh();
			}
			_gamePlayers[photonPlayer.ID] = new GamePlayer(photonPlayer);
			PlayerConnectedEvent.Invoke(photonPlayer);
		}

		public override void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer)
		{
			GameView gameView = GameView;
			if ((object)gameView != null)
			{
				gameView.Refresh();
			}
			_gamePlayers.Remove(photonPlayer.ID);
			PlayerDisconnectedEvent.Invoke(photonPlayer);
		}

		public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
		{
			Hashtable hashtable = playerAndUpdatedProps[1] as Hashtable;
			PhotonPlayer photonPlayer = playerAndUpdatedProps[0] as PhotonPlayer;
			if (hashtable != null && photonPlayer != null)
			{
				GameView gameView = GameView;
				if ((object)gameView != null)
				{
					gameView.Refresh(hashtable.Keys);
				}
				PlayerPropertiesChangedEvent.Invoke(new PlayerPropertiesEventArg(hashtable.Keys, photonPlayer));
			}
		}

		protected GamePlayer GetGamePlayer(PhotonPlayer player)
		{
			if (_gamePlayers.ContainsKey(player.ID))
			{
				return _gamePlayers[player.ID];
			}
			Log.Error(string.Format("GamePlayer for {0} not found", player));
			_gamePlayers[player.ID] = new GamePlayer(player);
			return _gamePlayers[player.ID];
		}

		protected virtual void OnDestroy()
		{
			PhotonNetworkExtension.MessageListeners.Remove(this);
		}
	}
}
