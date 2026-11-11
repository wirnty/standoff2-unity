using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Axlebolt.Bolt.Api;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt
{
	public class BoltApi
	{
		public delegate void EventHandler();


		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CAuthTest_003Ec__async1 : IAsyncStateMachine
		{
			private sealed class _003CAuthTest_003Ec__AnonStorey6
			{
				internal string token;

				internal _003CAuthTest_003Ec__async1 _003C_003Ef__ref_00241;

				internal string _003C_003Em__0()
				{
					return _003C_003Ef__ref_00241._0024this._authRemoteService.AuthTest(_003C_003Ef__ref_00241._0024this._gameId, token);
				}
			}

			internal string token;

			internal BoltApi _0024this;

			internal AsyncTaskMethodBuilder _0024builder;

			internal int _0024PC;

			private _003CAuthTest_003Ec__AnonStorey6 _0024locvar0;

			private TaskAwaiter _0024awaiter0;

			private TaskAwaiter<string> _0024awaiter1;

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
						_0024locvar0 = new _003CAuthTest_003Ec__AnonStorey6();
						_0024locvar0._003C_003Ef__ref_00241 = this;
						_0024locvar0.token = token;
						if (_0024locvar0.token == null)
						{
							throw new ArgumentNullException("token");
						}
						_0024this._synchronizationContext = SynchronizationContext.Current;
						_0024awaiter0 = _0024this.Connect().GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						goto case 1u;
					case 1u:
						_0024awaiter0.GetResult();
						string token2 = token;
						_0024awaiter1 = _0024this.Async(() => BoltApi.Instance._authRemoteService.AuthTest(BoltApi.Instance._gameId, token2), false).GetAwaiter();
						if (!_0024awaiter1.IsCompleted)
						{
							_0024PC = 2;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter1, ref this);
							return;
						}
						goto case 2u;
					case 2u:
						_0024this._ticket = _0024awaiter1.GetResult();
						_0024awaiter0 = _0024this.Handshake().GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 3;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						break;
					case 3u:
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
		private struct _003CAuthGPGS_003Ec__async2 : IAsyncStateMachine
		{
			private sealed class _003CAuthGPGS_003Ec__AnonStorey7
			{
				internal string serverAuthCode;

				internal _003CAuthGPGS_003Ec__async2 _003C_003Ef__ref_00242;

				internal string _003C_003Em__0()
				{
					return _003C_003Ef__ref_00242._0024this._authRemoteService.AuthGPGS(_003C_003Ef__ref_00242._0024this._gameId, serverAuthCode);
				}
			}

			internal string serverAuthCode;

			internal BoltApi _0024this;

			internal AsyncTaskMethodBuilder _0024builder;

			internal int _0024PC;

			private _003CAuthGPGS_003Ec__AnonStorey7 _0024locvar0;

			private TaskAwaiter _0024awaiter0;

			private TaskAwaiter<string> _0024awaiter1;

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
						_0024locvar0 = new _003CAuthGPGS_003Ec__AnonStorey7();
						_0024locvar0._003C_003Ef__ref_00242 = this;
						_0024locvar0.serverAuthCode = serverAuthCode;
						if (string.IsNullOrEmpty(_0024locvar0.serverAuthCode))
						{
							throw new ArgumentNullException("serverAuthCode");
						}
						_0024this._synchronizationContext = SynchronizationContext.Current;
						_0024awaiter0 = _0024this.Connect().GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						goto case 1u;
					case 1u:
						_0024awaiter0.GetResult();
							string serverAuthCode1 = serverAuthCode;
						_0024awaiter1 = _0024this.Async(() => BoltApi.Instance._authRemoteService.AuthGPGS(BoltApi.Instance._gameId, serverAuthCode1), false).GetAwaiter();
						if (!_0024awaiter1.IsCompleted)
						{
							_0024PC = 2;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter1, ref this);
							return;
						}
						goto case 2u;
					case 2u:
						_0024this._ticket = _0024awaiter1.GetResult();
						_0024awaiter0 = _0024this.Handshake().GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 3;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						break;
					case 3u:
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
		private struct _003CReconnect_003Ec__async4 : IAsyncStateMachine
		{
			internal BoltApi _0024this;

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
						if (!_0024this.IsAuthenticated)
						{
							throw new BoltApiException("Can't reconnect, player is not authenticted");
						}
						_0024awaiter0 = _0024this.Connect().GetAwaiter();
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


		private static BoltApi _instance;

		private readonly ClientService _clientService;

		private readonly IAuthRemoteService _authRemoteService;

		private readonly IHandshakeRemoteService _handshakeRemoteService;

		private string _ticket;

		private SynchronizationContext _synchronizationContext;

		private static readonly List<Service> Services = new List<Service>();

		public readonly string _gameId;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static FileCache _003CFileCache_003Ek__BackingField = new FileCache();

		public bool CallbackInSyncContext { get; set; } = true;


		public string Ip { get; set; } = "46.101.200.35";


		public int Port { get; set; } = 2222;


		public static FileCache FileCache
		{
			[CompilerGenerated]
			get
			{
				return _003CFileCache_003Ek__BackingField;
			}
		}

		public static BoltApi Instance
		{
			get
			{
				if (_instance == null)
				{
					throw new BoltApiException("BoltApi is not initialized");
				}
				return _instance;
			}
		}

		public static bool IsInitialized
		{
			[CompilerGenerated]
			get
			{
				return _instance != null;
			}
		}

		public bool IsConnectedAndReady
		{
			[CompilerGenerated]
			get
			{
				return _clientService.ConnectionState == RpcSupport.ConnectionState.Connected && IsAuthenticated;
			}
		}

		public bool IsAuthenticated { get; private set; }

		public event EventHandler ConnectionFailedEvent;

		private BoltApi(string gameId)
		{
			if (gameId == null)
			{
				throw new ArgumentNullException("gameId");
			}
			_gameId = gameId;
			_clientService = new ClientService(Assembly.GetExecutingAssembly());
			_clientService.ConnectionFailed += OnConnectionFailedEvent;
			_authRemoteService = Create<IAuthRemoteService>();
			_handshakeRemoteService = Create<IHandshakeRemoteService>();
		}

		public static void Init(string gameId)
		{
			if (_instance != null)
			{
				throw new BoltApiException("BoltApi already initialized");
			}
			_instance = new BoltApi(gameId);
			Services.Add(new BoltPlayer());
			Services.Add(new BoltPlayerStats());
		}

		public static void Destroy()
		{
			if (_instance == null)
			{
				return;
			}
			_instance._clientService.Disconnect();
			foreach (Service service in Services)
			{
				service.Destroy();
			}
			_instance = null;
		}
		private Task Connect()
		{
			return Async(delegate
			{
				_clientService.Connect(Ip, 2222);
			}, false);
		}
		
		public async Task AuthTest(string token)
		{
			await Handshake();
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CAuthGPGS_003Ec__async2))]
		public Task AuthGPGS(string serverAuthCode)
		{
			_003CAuthGPGS_003Ec__async2 stateMachine = default(_003CAuthGPGS_003Ec__async2);
			stateMachine.serverAuthCode = serverAuthCode;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		public Task Logout()
		{
			return Async(delegate
			{
				_clientService.Disconnect();
			}, false);
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CReconnect_003Ec__async4))]
		public Task Reconnect()
		{
			_003CReconnect_003Ec__async4 stateMachine = default(_003CReconnect_003Ec__async4);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		private Task Handshake()
		{
			return Async(delegate
			{
				IsAuthenticated = true;
				LoadService();
			}, false);
		}

		private void LoadService()
		{
			try
			{
				Task[] tasks = Services.Select((Service service) => Async(delegate
				{
					try
					{
						service.Load();
					}
					catch (Exception msg)
					{
						Logger.Error(msg);
						throw;
					}
				})).ToArray();
				Task.WaitAll(tasks);
			}
			catch (AggregateException ex)
			{
				if (ex.InnerException != null)
				{
					throw ex.InnerException;
				}
				throw;
			}
		}

		internal void CheckAuthenication()
		{
			if (!IsAuthenticated)
			{
				throw new BoltApiException("Are you authenticated?");
			}
		}

		internal Task Async(Action action, bool requireAuth = true)
		{
			if (requireAuth)
			{
				CheckAuthenication();
			}
			return Task.Factory.StartNew(action);
		}

		internal Task<T> Async<T>(Func<T> action, bool requireAuth = true)
		{
			if (requireAuth)
			{
				CheckAuthenication();
			}
			return Task.Factory.StartNew(action);
		}

		public T Create<T>()
		{
			return _clientService.Create<T>();
		}

		private void OnConnectionFailedEvent()
		{
			if (!IsAuthenticated)
			{
				return;
			}
			if (CallbackInSyncContext)
			{
				_synchronizationContext.Post(delegate
				{
					if (this.ConnectionFailedEvent != null)
					{
						this.ConnectionFailedEvent();
					}
				}, null);
			}
			else if (this.ConnectionFailedEvent != null)
			{
				this.ConnectionFailedEvent();
			}
		}
	}
}
