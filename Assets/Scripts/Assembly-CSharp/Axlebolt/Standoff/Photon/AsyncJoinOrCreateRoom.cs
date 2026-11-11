using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Axlebolt.Standoff.Photon
{
	public class AsyncJoinOrCreateRoom : AsyncOperation<AsyncJoinOrCreateRoom>
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CJoinOrCreateRoom_003Ec__async0 : IAsyncStateMachine
		{
			internal string roomName;

			internal RoomOptions roomOptions;

			internal TypedLobby typedLobby;

			internal string[] expectedUsers;

			internal AsyncJoinOrCreateRoom _0024this;

			internal AsyncTaskMethodBuilder _0024builder;

			internal int _0024PC;

			private CoroutineAsyncExtensions.CoroutineAsyncBridge _0024awaiter0;

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
						if (PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby, expectedUsers))
						{
							_0024awaiter0 = _0024this.WaitForResult().GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 1;
								_0024builder.AwaitOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							goto case 1u;
						}
						_0024this.Result = new AsyncResult(new ConnectionFailedException());
						_0024this.Done();
						break;
					case 1u:
						_0024awaiter0.GetResult();
						_0024this.Done();
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
				_0024builder.SetResult();
			}

			[DebuggerHidden]
			public void SetStateMachine(IAsyncStateMachine stateMachine)
			{
				_0024builder.SetStateMachine(stateMachine);
			}
		}

		[AsyncStateMachine(typeof(_003CJoinOrCreateRoom_003Ec__async0))]
		[DebuggerStepThrough]
		public Task JoinOrCreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby, string[] expectedUsers = null)
		{
			_003CJoinOrCreateRoom_003Ec__async0 stateMachine = default(_003CJoinOrCreateRoom_003Ec__async0);
			stateMachine.roomName = roomName;
			stateMachine.roomOptions = roomOptions;
			stateMachine.typedLobby = typedLobby;
			stateMachine.expectedUsers = expectedUsers;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
		{
			Result = new AsyncResult(new JoinFailedException((short)codeAndMsg[0], (string)codeAndMsg[1]));
		}

		public override void OnJoinedRoom()
		{
			if (AsyncOperation<AsyncJoinOrCreateRoom>.Log.DebugEnabled)
			{
				AsyncOperation<AsyncJoinOrCreateRoom>.Log.Debug("OnJoinedRoom");
			}
			Result = new AsyncResult();
		}

		public override void OnConnectionFail(DisconnectCause cause)
		{
			AsyncOperation<AsyncJoinOrCreateRoom>.Log.Warning(string.Format("OnConnectionFail {0}", cause));
			Result = new AsyncResult(new ConnectionFailedException(cause));
		}
	}
}
