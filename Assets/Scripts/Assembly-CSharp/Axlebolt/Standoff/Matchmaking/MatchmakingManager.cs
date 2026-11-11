using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Level;
using Axlebolt.Standoff.Photon;
using ExitGames.Client.Photon;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Matchmaking
{
	public class MatchmakingManager
	{
		public delegate void ServerDetailsHandler(int currentOnline, string region, int ping);

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CFind_003Ec__async0 : IAsyncStateMachine
		{
			internal MatchmakingFilter filter;

			internal ServerDetailsHandler handler;

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
						_0024awaiter0 = Find(filter, CancellationToken.None, handler).GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						break;
					case 1u:
						break;
					}
					_0024awaiter0.GetResult();
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
		private struct _003CFind_003Ec__async1 : IAsyncStateMachine
		{
			internal MatchmakingFilter filter;

			internal CancellationToken cancellationToken;

			internal GameMode _003CgameMode_003E__1;

			internal LevelDefinition _003Clevel_003E__1;

			internal ServerDetailsHandler handler;

			internal bool _003Csuccess_003E__2;

			internal Exception _003Cex_003E__3;

			internal AsyncTaskMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter<GameServer> _0024awaiter0;

			private TaskAwaiter _0024awaiter1;

			private TaskAwaiter<bool> _0024awaiter2;

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
						if (filter == null)
						{
							throw new ArgumentNullException("filter");
						}
						if (InProgress)
						{
							throw new InvalidOperationException("FindGame already in progress");
						}
						_cancellationToken = cancellationToken;
						InProgress = true;
						_003CgameMode_003E__1 = GameModeUtility.GetByName(filter.GameModeName);
						_003Clevel_003E__1 = ((filter.LevelName == null) ? null : LevelUtility.GetLevel(filter.LevelName));
						PhotonNetwork.AuthValues = new AuthenticationValues(filter.PlayerId);
						num = 4294967293u;
						break;
					case 1u:
					case 2u:
					case 3u:
					case 4u:
					case 5u:
					case 6u:
						break;
					}
					try
					{
						switch (num)
						{
						default:
						{
							Exception ex2 = default(Exception);
							int num2 = default(int);
							try
							{
								switch (num)
								{
								default:
									_0024awaiter0 = FindBestServer().GetAwaiter();
									if (!_0024awaiter0.IsCompleted)
									{
										_0024PC = 1;
										flag = true;
										_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
										return;
									}
									goto case 1u;
								case 1u:
									_gameServer = _0024awaiter0.GetResult();
									goto IL_0148;
								case 2u:
									_0024awaiter1.GetResult();
									OnServerDetailsUpdated(handler);
									ThrowIfCancellationRequested();
									_0024awaiter1 = JoinOrCreateRoom(_003CgameMode_003E__1, _003Clevel_003E__1, filter.ExpectedPlayers).GetAwaiter();
									if (!_0024awaiter1.IsCompleted)
									{
										_0024PC = 3;
										flag = true;
										_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter1, ref this);
										return;
									}
									goto case 3u;
								case 3u:
									_0024awaiter1.GetResult();
									if (filter.AllowEmptyRoom)
									{
										break;
									}
									_0024awaiter2 = WaitOtherPlayers(handler).GetAwaiter();
									if (!_0024awaiter2.IsCompleted)
									{
										_0024PC = 4;
										flag = true;
										_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter2, ref this);
										return;
									}
									goto case 4u;
								case 4u:
									_003Csuccess_003E__2 = _0024awaiter2.GetResult();
									if (_003Csuccess_003E__2)
									{
										break;
									}
									_0024awaiter1 = Disconnect().GetAwaiter();
									if (!_0024awaiter1.IsCompleted)
									{
										_0024PC = 5;
										flag = true;
										_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter1, ref this);
										return;
									}
									goto case 5u;
								case 5u:
									{
										_0024awaiter1.GetResult();
										goto IL_0148;
									}
									IL_0148:
									_0024awaiter1 = Connect().GetAwaiter();
									if (!_0024awaiter1.IsCompleted)
									{
										_0024PC = 2;
										flag = true;
										_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter1, ref this);
										return;
									}
									goto case 2u;
								}
								Log.Debug("Game found successfully!");
							}
							catch (Exception ex)
							{
								ex2 = ex;
								num2 = 1;
							}
							switch (num2)
							{
							default:
								goto end_IL_00cf;
							case 1:
								_003Cex_003E__3 = ex2;
								Log.Debug(_003Cex_003E__3);
								_0024awaiter1 = Disconnect().GetAwaiter();
								if (!_0024awaiter1.IsCompleted)
								{
									_0024PC = 6;
									flag = true;
									_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter1, ref this);
									return;
								}
								break;
							}
							break;
						}
						case 6u:
							break;
						}
						_0024awaiter1.GetResult();
						throw _003Cex_003E__3;
						end_IL_00cf:;
					}
					finally
					{
						if (!flag)
						{
							InProgress = false;
							_cancellationToken = CancellationToken.None;
							_gameServer = null;
						}
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

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CFindBestServer_003Ec__async2 : IAsyncStateMachine
		{
			internal float _003Ctime_003E__1;

			internal Dictionary<GameServer, int> _003CbestPings_003E__1;

			internal Dictionary<GameServer, Ping> _003Cpings_003E__1;

			internal GameServer[] _0024locvar0;

			internal int _0024locvar1;

			internal Dictionary<GameServer, Ping>.Enumerator _0024locvar2;

			internal GameServer[] _0024locvar3;

			internal int _0024locvar4;

			internal KeyValuePair<GameServer, int> _003CbestPing_003E__1;

			internal AsyncTaskMethodBuilder<GameServer> _0024builder;

			internal int _0024PC;

			private AsyncTools.Awaiter _0024awaiter0;

			private static Func<KeyValuePair<GameServer, int>, int> _003C_003Ef__am_0024cache0;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				GameServer key;
				try
				{
					switch (num)
					{
					default:
						return;
					case 0u:
						_003Ctime_003E__1 = Time.time;
						_003CbestPings_003E__1 = new Dictionary<GameServer, int>();
						_003Cpings_003E__1 = new Dictionary<GameServer, Ping>();
						_0024locvar0 = Servers;
						for (_0024locvar1 = 0; _0024locvar1 < _0024locvar0.Length; _0024locvar1++)
						{
							GameServer gameServer = _0024locvar0[_0024locvar1];
							_003CbestPings_003E__1[gameServer] = int.MaxValue;
							_003Cpings_003E__1[gameServer] = new Ping(gameServer.Ip);
						}
						goto IL_0264;
					case 1u:
						{
							_0024awaiter0.GetResult();
							goto IL_0264;
						}
						IL_0264:
						if (Time.time - _003Ctime_003E__1 < 3f)
						{
							ThrowIfCancellationRequested();
							_0024locvar2 = _003Cpings_003E__1.GetEnumerator();
							try
							{
								while (_0024locvar2.MoveNext())
								{
									KeyValuePair<GameServer, Ping> current = _0024locvar2.Current;
									if (current.Value.isDone && _003CbestPings_003E__1[current.Key] > current.Value.time)
									{
										_003CbestPings_003E__1[current.Key] = current.Value.time;
										if (Log.DebugEnabled)
										{
											Log.Debug(string.Format("Ping to {0} is {1}", current.Key.Location, current.Value.time));
										}
									}
								}
							}
							finally
							{
								((IDisposable)_0024locvar2).Dispose();
							}
							_0024locvar3 = Servers;
							for (_0024locvar4 = 0; _0024locvar4 < _0024locvar3.Length; _0024locvar4++)
							{
								GameServer gameServer2 = _0024locvar3[_0024locvar4];
								if (_003Cpings_003E__1[gameServer2].isDone)
								{
									_003Cpings_003E__1[gameServer2] = new Ping(gameServer2.Ip);
								}
							}
							_0024awaiter0 = 0.1f.GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 1;
								_0024builder.AwaitOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							goto case 1u;
						}
						_003CbestPing_003E__1 = _003CbestPings_003E__1.OrderBy((KeyValuePair<GameServer, int> ping) => ping.Value).First();
						if (_003CbestPing_003E__1.Value == int.MaxValue)
						{
							throw new ConnectionFailedException();
						}
						key = _003CbestPing_003E__1.Key;
						break;
					}
				}
				catch (Exception exception)
				{
					_0024PC = -1;
					_0024builder.SetException(exception);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult(key);
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}

			private static int _003C_003Em__0(KeyValuePair<GameServer, int> ping)
			{
				return ping.Value;
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CWaitOtherPlayers_003Ec__async3 : IAsyncStateMachine
		{
			internal float _003Ctime_003E__1;

			internal float _003Cduration_003E__1;

			internal ServerDetailsHandler handler;

			internal AsyncTaskMethodBuilder<bool> _0024builder;

			internal int _0024PC;

			private AsyncTools.Awaiter _0024awaiter0;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				bool result;
				try
				{
					switch (num)
					{
					default:
						return;
					case 0u:
						Log.Debug("Waiting other players");
						_003Ctime_003E__1 = Time.time;
						_003Cduration_003E__1 = UnityEngine.Random.Range(3f, 9f);
						goto IL_00b1;
					case 1u:
						{
							_0024awaiter0.GetResult();
							goto IL_00b1;
						}
						IL_00b1:
						if (PhotonNetwork.playerList.Length > 1 || !(Time.time - _003Ctime_003E__1 < _003Cduration_003E__1))
						{
							break;
						}
						OnServerDetailsUpdated(handler);
						ThrowIfCancellationRequested();
						_0024awaiter0 = 0.5f.GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						goto case 1u;
					}
					result = PhotonNetwork.playerList.Length > 1;
				}
				catch (Exception exception)
				{
					_0024PC = -1;
					_0024builder.SetException(exception);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult(result);
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CJoinOrCreateRoom_003Ec__async4 : IAsyncStateMachine
		{
			internal GameMode gameMode;

			internal LevelDefinition level;

			internal string[] expectedPlayers;

			internal bool _003Cfound_003E__1;

			internal AsyncTaskMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter<bool> _0024awaiter0;

			private TaskAwaiter<bool> _0024awaiter1;

			private TaskAwaiter _0024awaiter2;

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
						_0024awaiter0 = JoinRandomRoom(gameMode, level, 19, expectedPlayers).GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						goto case 1u;
					case 1u:
						_003Cfound_003E__1 = _0024awaiter0.GetResult();
						ThrowIfCancellationRequested();
						if (!_003Cfound_003E__1)
						{
							_0024awaiter1 = JoinRandomRoom(gameMode, level, 199, expectedPlayers).GetAwaiter();
							if (!_0024awaiter1.IsCompleted)
							{
								_0024PC = 2;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter1, ref this);
								return;
							}
							goto case 2u;
						}
						goto IL_010a;
					case 2u:
						_003Cfound_003E__1 = _0024awaiter1.GetResult();
						ThrowIfCancellationRequested();
						goto IL_010a;
					case 3u:
						break;
						IL_010a:
						if (!_003Cfound_003E__1)
						{
							_0024awaiter2 = CreateRoom(gameMode, level, expectedPlayers).GetAwaiter();
							if (!_0024awaiter2.IsCompleted)
							{
								_0024PC = 3;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter2, ref this);
								return;
							}
							break;
						}
						goto end_IL_000e;
					}
					_0024awaiter2.GetResult();
					ThrowIfCancellationRequested();
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

		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CConnect_003Ec__async5 : IAsyncStateMachine
		{
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
						_0024awaiter0 = PhotonAsync.Connect(_gameServer.Ip, 5055, "", "0.3.0.final").GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						break;
					case 1u:
						break;
					}
					_0024awaiter0.GetResult();
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
		private struct _003CDisconnect_003Ec__async6 : IAsyncStateMachine
		{
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
						_0024awaiter0 = PhotonAsync.Disconnect().GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						break;
					case 1u:
						break;
					}
					_0024awaiter0.GetResult();
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
		private struct _003CCreateRoom_003Ec__async7 : IAsyncStateMachine
		{
			internal LevelDefinition level;

			internal GameMode gameMode;

			internal Hashtable _003CroomProperties_003E__1;

			internal RoomOptions _003CroomOptions_003E__1;

			internal string[] expectedPlayers;

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
						if (level == null)
						{
							level = LevelUtility.GetRandomLevel(gameMode.Name);
						}
						_003CroomProperties_003E__1 = new Hashtable
						{
							{ "C0", gameMode.Name },
							{ "C1", level.name },
							{
								"C2",
								(byte)0
							}
						};
						_003CroomOptions_003E__1 = new RoomOptions
						{
							PlayerTtl = 25000,
							PublishUserId = true,
							IsOpen = true,
							IsVisible = true,
							MaxPlayers = gameMode.MaxPlayers,
							CustomRoomPropertiesForLobby = new string[3] { "C0", "C1", "C2" },
							CustomRoomProperties = _003CroomProperties_003E__1
						};
						_0024awaiter0 = PhotonAsync.CreateRoom(null, _003CroomOptions_003E__1, GetTypedLobby(), expectedPlayers).GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						break;
					case 1u:
						break;
					}
					_0024awaiter0.GetResult();
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
		private struct _003CJoinRandomRoom_003Ec__async8 : IAsyncStateMachine
		{
			internal GameMode gameMode;

			internal int maxGameStateId;

			internal string _003Csql_003E__1;

			internal LevelDefinition level;

			internal string[] expectedPlayers;

			internal AsyncTaskMethodBuilder<bool> _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

			public void MoveNext()
			{
				uint num = (uint)_0024PC;
				_0024PC = -1;
				bool flag = false;
				bool result;
				try
				{
					switch (num)
					{
					default:
						return;
					case 0u:
						num = 4294967293u;
						break;
					case 1u:
						break;
					}
					try
					{
						switch (num)
						{
						default:
							_003Csql_003E__1 = string.Format("{0}=\"{1}\" AND {2}<={3}", "C0", gameMode.name, "C2", maxGameStateId);
							if (level != null)
							{
								_003Csql_003E__1 += string.Format(" AND {0}=\"{1}\"", "C1", level.name);
							}
							_0024awaiter0 = PhotonAsync.JoinRandomRoom(null, gameMode.MaxPlayers, MatchmakingMode.FillRoom, GetTypedLobby(), _003Csql_003E__1, expectedPlayers, false).GetAwaiter();
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
						result = true;
					}
					catch (JoinFailedException ex)
					{
						if (ex.Code != 32765 && ex.Code != 32760 && ex.Code != 32758)
						{
							throw;
						}
						result = false;
					}
				}
				catch (Exception exception)
				{
					_0024PC = -1;
					_0024builder.SetException(exception);
					return;
				}
				_0024PC = -1;
				_0024builder.SetResult(result);
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		private static readonly Log Log = Log.Create(typeof(MatchmakingManager));

		private const float FindBestRegionDuration = 3f;

		private const float MinEmptyRoomWaitTime = 3f;

		private const float MaxEmptyRoomWaitTime = 9f;

		public const float PlayerTtl = 25f;

		public const float ReconnectDuration = 20f;

		private const string GameVersion = "0.3.0.final";

		private static readonly GameServer[] Servers = new GameServer[1]
		{
			new GameServer("91.108.240.118", "Miami")
		};

		private static CancellationToken _cancellationToken;

		private static GameServer _gameServer;

		public static bool InProgress { get; private set; }

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CFind_003Ec__async0))]
		public static Task Find([NotNull] MatchmakingFilter filter, ServerDetailsHandler handler = null)
		{
			_003CFind_003Ec__async0 stateMachine = default(_003CFind_003Ec__async0);
			stateMachine.filter = filter;
			stateMachine.handler = handler;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[AsyncStateMachine(typeof(_003CFind_003Ec__async1))]
		[DebuggerStepThrough]
		public static Task Find([NotNull] MatchmakingFilter filter, CancellationToken cancellationToken, ServerDetailsHandler handler = null)
		{
			_003CFind_003Ec__async1 stateMachine = default(_003CFind_003Ec__async1);
			stateMachine.filter = filter;
			stateMachine.cancellationToken = cancellationToken;
			stateMachine.handler = handler;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[AsyncStateMachine(typeof(_003CFindBestServer_003Ec__async2))]
		[DebuggerStepThrough]
		private static Task<GameServer> FindBestServer()
		{
			_003CFindBestServer_003Ec__async2 stateMachine = default(_003CFindBestServer_003Ec__async2);
			stateMachine._0024builder = AsyncTaskMethodBuilder<GameServer>.Create();
			ref AsyncTaskMethodBuilder<GameServer> _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CWaitOtherPlayers_003Ec__async3))]
		private static Task<bool> WaitOtherPlayers(ServerDetailsHandler handler)
		{
			_003CWaitOtherPlayers_003Ec__async3 stateMachine = default(_003CWaitOtherPlayers_003Ec__async3);
			stateMachine.handler = handler;
			stateMachine._0024builder = AsyncTaskMethodBuilder<bool>.Create();
			ref AsyncTaskMethodBuilder<bool> _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		private static void OnServerDetailsUpdated(ServerDetailsHandler handler)
		{
			if (handler != null)
			{
				handler(PhotonNetwork.countOfPlayers, _gameServer.Location, PhotonNetwork.GetPing());
			}
		}

		[AsyncStateMachine(typeof(_003CJoinOrCreateRoom_003Ec__async4))]
		[DebuggerStepThrough]
		private static Task JoinOrCreateRoom(GameMode gameMode, LevelDefinition level, string[] expectedPlayers)
		{
			_003CJoinOrCreateRoom_003Ec__async4 stateMachine = default(_003CJoinOrCreateRoom_003Ec__async4);
			stateMachine.gameMode = gameMode;
			stateMachine.level = level;
			stateMachine.expectedPlayers = expectedPlayers;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		private static void ThrowIfCancellationRequested()
		{
			_cancellationToken.ThrowIfCancellationRequested();
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CConnect_003Ec__async5))]
		private static Task Connect()
		{
			_003CConnect_003Ec__async5 stateMachine = default(_003CConnect_003Ec__async5);
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CDisconnect_003Ec__async6))]
		private static Task Disconnect()
		{
			_003CDisconnect_003Ec__async6 stateMachine = default(_003CDisconnect_003Ec__async6);
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CCreateRoom_003Ec__async7))]
		private static Task CreateRoom(GameMode gameMode, LevelDefinition level, string[] expectedPlayers)
		{
			_003CCreateRoom_003Ec__async7 stateMachine = default(_003CCreateRoom_003Ec__async7);
			stateMachine.level = level;
			stateMachine.gameMode = gameMode;
			stateMachine.expectedPlayers = expectedPlayers;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		private static TypedLobby GetTypedLobby()
		{
			return new TypedLobby("0.3.0.final", LobbyType.SqlLobby);
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CJoinRandomRoom_003Ec__async8))]
		private static Task<bool> JoinRandomRoom(GameMode gameMode, LevelDefinition level, int maxGameStateId, string[] expectedPlayers)
		{
			_003CJoinRandomRoom_003Ec__async8 stateMachine = default(_003CJoinRandomRoom_003Ec__async8);
			stateMachine.gameMode = gameMode;
			stateMachine.maxGameStateId = maxGameStateId;
			stateMachine.level = level;
			stateMachine.expectedPlayers = expectedPlayers;
			stateMachine._0024builder = AsyncTaskMethodBuilder<bool>.Create();
			ref AsyncTaskMethodBuilder<bool> _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}
	}
}
