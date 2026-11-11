using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Bolt;
using Axlebolt.RpcSupport;
using Axlebolt.Standoff.Core;
using UnityEngine;

namespace Axlebolt.Standoff.Game.Bolt
{
	public class BoltSupport : MonoBehaviour
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CReconnect_003Ec__async0 : IAsyncStateMachine
		{
			internal BoltSupport _0024this;

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
						if (Log.DebugEnabled)
						{
							Log.Debug("Trying reconnect to Bolt");
						}
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
							_0024awaiter0 = BoltApi.Instance.Reconnect().GetAwaiter();
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
					catch (ConnectionFailedException message)
					{
						Log.Debug(message);
					}
					catch (Exception message2)
					{
						Log.Error(message2);
					}
					if (_0024this._destroyed || BoltApi.Instance.IsAuthenticated)
					{
						return;
					}
					_0024this.StartCoroutine(_0024this.ReconnectAfter());
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

		private static readonly Log Log = Log.Create(typeof(BoltSupport));

		private const int ReconnectInterval = 30;

		private bool _destroyed;

		private void Awake()
		{
			if (BoltApi.IsInitialized && BoltApi.Instance.IsAuthenticated)
			{
				Setup();
			}
		}

		private void Setup()
		{
			base.gameObject.AddComponent<PlayerStatsCollector>();
			BoltApi.Instance.ConnectionFailedEvent += Reconnect;
			if (!BoltApi.Instance.IsConnectedAndReady)
			{
				Reconnect();
			}
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CReconnect_003Ec__async0))]
		private void Reconnect()
		{
			_003CReconnect_003Ec__async0 stateMachine = default(_003CReconnect_003Ec__async0);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		private IEnumerator ReconnectAfter()
		{
			if (Log.DebugEnabled)
			{
				Log.Debug(string.Format("Reconnect to the Bolt after {0} seconds", 30));
			}
			yield return new WaitForSeconds(30f);
			Reconnect();
		}

		private void OnDestroy()
		{
			_destroyed = true;
		}
	}
}
