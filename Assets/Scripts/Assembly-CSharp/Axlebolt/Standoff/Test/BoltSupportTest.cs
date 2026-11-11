using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Axlebolt.Bolt;
using Axlebolt.Bolt.Unity;

namespace Axlebolt.Standoff.Test
{
	public class BoltSupportTest : BaseTest
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CAuthTest_003Ec__async0 : IAsyncStateMachine
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
						_0024awaiter0 = BoltApi.Instance.AuthTest("d23a7391-00fb-46cb-a9fd-c5cfca4f3c5d").GetAwaiter();
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
		private struct _003CLogout_003Ec__async1 : IAsyncStateMachine
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
						_0024awaiter0 = BoltApi.Instance.Logout().GetAwaiter();
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

		
		public void Init()
		{
			BoltUnityApi.Init("e0eb8b95-72b3-4821-9dce-31bbb632da7c");
		}

		
		public void Destroy()
		{
			BoltApi.Destroy();
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CAuthTest_003Ec__async0))]
		protected Task AuthTest()
		{
			_003CAuthTest_003Ec__async0 stateMachine = default(_003CAuthTest_003Ec__async0);
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[AsyncStateMachine(typeof(_003CLogout_003Ec__async1))]
		[DebuggerStepThrough]
		protected Task Logout()
		{
			_003CLogout_003Ec__async1 stateMachine = default(_003CLogout_003Ec__async1);
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}
	}
}
