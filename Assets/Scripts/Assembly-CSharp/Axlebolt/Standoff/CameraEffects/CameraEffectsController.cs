using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Axlebolt.Standoff.CameraEffects
{
	public class CameraEffectsController : Singleton<CameraEffectsController>
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CPlayDieWithFocus_003Ec__async0 : IAsyncStateMachine
		{
			internal Camera cam;

			internal BipedMap character;

			internal Transform killerTransform;

			internal RawImage screenshotImage;

			internal CameraEffectsController _0024this;

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
						if (!_0024this._isInitialized)
						{
							throw new InvalidOperationException("CameraEffectsController not initialized");
						}
						_0024awaiter0 = _0024this.PlayerDieEffect.PlayEffect(cam, character, killerTransform).GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						goto case 1u;
					case 1u:
						_0024awaiter0.GetResult();
						_0024awaiter0 = _0024this.PlayerFocusEffect.PlayEffect(cam, killerTransform, screenshotImage).GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 2;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						break;
					case 2u:
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
		private struct _003CPlayDie_003Ec__async1 : IAsyncStateMachine
		{
			internal Camera cam;

			internal BipedMap character;

			internal CameraEffectsController _0024this;

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
						if (!_0024this._isInitialized)
						{
							throw new InvalidOperationException("CameraEffectsController not initialized");
						}
						_0024awaiter0 = _0024this.PlayerDieEffect.PlayEffect(cam, character, character.transform).GetAwaiter();
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

		private Camera _camera;

		private bool _isInitialized;

		public PlayerDieEffect PlayerDieEffect { get; private set; }

		public PlayerFocusEffect PlayerFocusEffect { get; private set; }

		public void Initialize()
		{
			PlayerDieEffect = base.gameObject.AddComponent<PlayerDieEffect>();
			PlayerFocusEffect = base.gameObject.AddComponent<PlayerFocusEffect>();
			_isInitialized = true;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CPlayDieWithFocus_003Ec__async0))]
		public Task PlayDieWithFocus(Camera cam, BipedMap character, Transform killerTransform, RawImage screenshotImage)
		{
			_003CPlayDieWithFocus_003Ec__async0 stateMachine = default(_003CPlayDieWithFocus_003Ec__async0);
			stateMachine.cam = cam;
			stateMachine.character = character;
			stateMachine.killerTransform = killerTransform;
			stateMachine.screenshotImage = screenshotImage;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CPlayDie_003Ec__async1))]
		public Task PlayDie(Camera cam, BipedMap character)
		{
			_003CPlayDie_003Ec__async1 stateMachine = default(_003CPlayDie_003Ec__async1);
			stateMachine.cam = cam;
			stateMachine.character = character;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}
	}
}
