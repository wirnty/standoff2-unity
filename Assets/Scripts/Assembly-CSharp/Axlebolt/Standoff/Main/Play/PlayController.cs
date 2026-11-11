using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Axlebolt.Bolt;
using Axlebolt.Standoff.Game;
using Axlebolt.Standoff.Matchmaking;
using Axlebolt.Standoff.Photon;
using Axlebolt.Standoff.UI;
using I2.Loc;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Axlebolt.Standoff.Main.Play
{
	public class PlayController : MonoBehaviour
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CFindGame_003Ec__async0 : IAsyncStateMachine
		{
			internal PlayController _0024this;

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
							_0024awaiter0 = _0024this.Find().GetAwaiter();
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
					catch (OperationCanceledException)
					{
					}
					catch (ConnectionFailedException)
					{
						_0024this.ConnectionFailed();
					}
					catch (Exception ex3)
					{
						_0024this.UnexpectedException(ex3);
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
		private struct _003CFind_003Ec__async1 : IAsyncStateMachine
		{
			private sealed class _003CFind_003Ec__AnonStorey2
			{
				internal CancellationTokenSource cancellationTokenSource;

				internal Dialog dialog;

				internal _003CFind_003Ec__async1 _003C_003Ef__ref_00241;

				internal void _003C_003Em__0(DialogButton btn)
				{
					cancellationTokenSource.Cancel();
				}

				internal void _003C_003Em__1(int online, string region, int ping)
				{
					SearchGameView content = dialog.GetContent<SearchGameView>();
					content.OnlinePlayers = ((online <= 1) ? "-" : online.ToString());
					content.Region = region;
					content.Ping = ping;
				}
			}

			internal MatchmakingFilter _003Cfilter_003E__1;

			internal PlayController _0024this;

			internal AsyncTaskMethodBuilder _0024builder;

			internal int _0024PC;

			private _003CFind_003Ec__AnonStorey2 _0024locvar0;

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
						_0024locvar0 = new _003CFind_003Ec__AnonStorey2();
						_0024locvar0._003C_003Ef__ref_00241 = this;
						_0024locvar0.dialog = Dialogs.Create(ScriptLocalization.Dialogs.SearchGame, _0024this._searchGameView);
						_0024locvar0.dialog.SetTimer(new DialogTimer.Default());
						_0024locvar0.dialog.AddButton(new DialogButton(ScriptLocalization.Common.Cancel));
						_0024locvar0.cancellationTokenSource = new CancellationTokenSource();
						_003Cfilter_003E__1 = new MatchmakingFilter(BoltService<BoltPlayer>.Instance.Uid, "DeathMatch")
						{
							AllowEmptyRoom = true
						};
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
								_003CFind_003Ec__AnonStorey2 _0024locvar01 = _0024locvar0;
								_0024locvar0.dialog.Show(delegate
							{
								_0024locvar01.cancellationTokenSource.Cancel();
							}, false);
							_0024awaiter0 = MatchmakingManager.Find(_003Cfilter_003E__1, _0024locvar0.cancellationTokenSource.Token, delegate(int online, string region, int ping)
							{
								SearchGameView content = _0024locvar01.dialog.GetContent<SearchGameView>();
								content.OnlinePlayers = ((online <= 1) ? "-" : online.ToString());
								content.Region = region;
								content.Ping = ping;
							}).GetAwaiter();
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
						StartGame();
					}
					catch (Exception)
					{
						_0024locvar0.dialog.Hide();
						throw;
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

		[SerializeField]
		private Button _playButton;

		[NotNull]
		[SerializeField]
		private SearchGameView _searchGameView;

		private void Awake()
		{
			_playButton.onClick.AddListener(FindGame);
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CFindGame_003Ec__async0))]
		private void FindGame()
		{
			_003CFindGame_003Ec__async0 stateMachine = default(_003CFindGame_003Ec__async0);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		[AsyncStateMachine(typeof(_003CFind_003Ec__async1))]
		[DebuggerStepThrough]
		private Task Find()
		{
			_003CFind_003Ec__async1 stateMachine = default(_003CFind_003Ec__async1);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		private void ConnectionFailed()
		{
			string connectionFailed = ScriptLocalization.Dialogs.ConnectionFailed;
			string checkInternetConnection = ScriptLocalization.Dialogs.CheckInternetConnection;
			Dialog dialog = Dialogs.Create(connectionFailed, checkInternetConnection, new DialogButton(ScriptLocalization.Common.Ok));
			dialog.Show(FindAgain);
		}

		private void UnexpectedException(Exception ex)
		{
			string connectionFailed = ScriptLocalization.Dialogs.ConnectionFailed;
			string message = ex.Message;
			Dialog dialog = Dialogs.Create(connectionFailed, message, new DialogButton(ScriptLocalization.Common.Ok));
			dialog.Show(FindAgain);
		}

		private void FindAgain(DialogButton btn)
		{
			FindGame();
		}

		private static void StartGame()
		{
			PlayerAttr playerAttr = new PlayerAttr(BoltService<BoltPlayer>.Instance.Name, BoltService<BoltPlayer>.Instance.Avatar);
			GameManager.InitGame(playerAttr, OnExit);
		}

		private static void OnExit(string errorMsg)
		{
			SceneManager.LoadScene("Main", LoadSceneMode.Single);
		}
	}
}
