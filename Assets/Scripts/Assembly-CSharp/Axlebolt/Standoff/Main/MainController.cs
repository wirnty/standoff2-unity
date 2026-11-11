using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Unity;
using Axlebolt.RpcSupport;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Main.Auth;
using Axlebolt.Standoff.Main.Play;
using Axlebolt.Standoff.Main.Profile;
using Axlebolt.Standoff.UI;
using Axlebolt.Standoff.Utils;
using I2.Loc;
using UnityEngine;

namespace Axlebolt.Standoff.Main
{
	public class MainController : MonoBehaviour
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CReconnect_003Ec__async0 : IAsyncStateMachine
		{
			internal MainController _0024this;

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
							_0024awaiter0 = AsyncUtility.Async(Dialogs.Create(ScriptLocalization.Dialogs.ConnectionHasBeenLost, ScriptLocalization.Dialogs.TryingToReconnect), BoltApi.Instance.Reconnect()).GetAwaiter();
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
						_0024this.Init();
					}
					catch (ConnectionFailedException)
					{
						DialogButton dialogButton = new DialogButton(ScriptLocalization.Common.Ok);
						Dialog dialog = Dialogs.Create(ScriptLocalization.Dialogs.ConnectionHasBeenLost, ScriptLocalization.Dialogs.CheckInternetConnection, dialogButton);
						MainController athis = _0024this;
						dialog.Show(delegate
						{
							athis.Reconnect();
						});
					}
					catch (Exception ex2)
					{
						Log.Error(ex2);
						DialogButton dialogButton2 = new DialogButton(ScriptLocalization.Common.Ok);
						Dialog dialog2 = Dialogs.Create(ScriptLocalization.Dialogs.ConnectionHasBeenLost, ex2.Message, dialogButton2);
						MainController athis = _0024this;
						dialog2.Show(delegate
						{
							athis.Reconnect();
						});
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

		private static readonly Log Log = Log.Create(typeof(MainController));

		[SerializeField]
		private AuthController _authController;

		[SerializeField]
		private SidebarView _sidebarView;

		[SerializeField]
		private ShortProfileController _shortProfileController;

		[SerializeField]
		private PlayController _playController;

		[SerializeField]
		private ProfileController _profileController;

		private void Start()
		{
			if (!BoltApi.IsInitialized)
			{
				Setup();
			}
			else if (BoltApi.Instance.IsConnectedAndReady)
			{
				Init();
			}
			else
			{
				Reconnect();
			}
			BoltApi.Instance.ConnectionFailedEvent += Reconnect;
		}

		private void Setup()
		{
			UnityEngine.Debug.unityLogger.logEnabled = true;
			UnityEngine.Debug.Log("asdsad");
			BoltUnityApi.Init("e0eb8b95-72b3-4821-9dce-31bbb632da7c");
            UnityEngine.Debug.Log("asdsad");
			_authController.Authenticate(Init);
			UnityEngine.Debug.Log("asdsad");
		}

		private void Init()
		{
			_shortProfileController.Refresh();
			_profileController.Refresh();
		}

		[AsyncStateMachine(typeof(_003CReconnect_003Ec__async0))]
		[DebuggerStepThrough]
		private void Reconnect()
		{
			_003CReconnect_003Ec__async0 stateMachine = default(_003CReconnect_003Ec__async0);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		private void OnDestroy()
		{
			if (BoltApi.IsInitialized)
			{
				BoltApi.Instance.ConnectionFailedEvent -= Reconnect;
			}
		}
	}
}
