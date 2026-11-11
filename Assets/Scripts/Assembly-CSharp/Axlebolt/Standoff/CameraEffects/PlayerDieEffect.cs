using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Axlebolt.Standoff.Common;
using Axlebolt.Standoff.Player;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.CameraEffects
{
	public class PlayerDieEffect : MonoBehaviour
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CPlayEffect_003Ec__async0 : IAsyncStateMachine
		{
			internal Camera cam;

			internal BipedMap character;

			internal Transform killerTransform;

			internal Vector3 _003Cdir_003E__1;

			internal Transform _003CcameraTransform_003E__1;

			internal PlayerDieEffect _0024this;

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
						if (cam == null)
						{
							throw new ArgumentNullException("cam");
						}
						if (character == null)
						{
							throw new ArgumentNullException("character");
						}
						if (killerTransform == null)
						{
							throw new ArgumentNullException("killerTransform");
						}
						_0024this._pelvislTransform = character.Hip;
						_003Cdir_003E__1 = -(killerTransform.position - _0024this._pelvislTransform.position);
						_003Cdir_003E__1.y = 0f;
						_0024this._direction = _003Cdir_003E__1.normalized;
						_003CcameraTransform_003E__1 = cam.transform;
						_003CcameraTransform_003E__1.SetParent(_0024this._cameraHelper);
						_003CcameraTransform_003E__1.localEulerAngles = Vector3.zero;
						_003CcameraTransform_003E__1.localPosition = Vector3.zero;
						_0024this._cameraTransform = _003CcameraTransform_003E__1;
						_0024this._effectStartTime = Time.time;
						_0024this._cameraHelper.rotation = Quaternion.LookRotation(-_0024this._direction);
						_0024this._cameraHelper.position = _0024this._pelvislTransform.position;
						_0024this._cameraHelper.position += _0024this._cameraHelper.TransformDirection(_0024this._parameters.InitialOffset);
						_0024this._pelvisPosition = _0024this._pelvislTransform.position;
						_0024awaiter0 = _0024this.EffectLoop().GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitOnCompleted(ref _0024awaiter0, ref this);
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

		private float _effectStartTime;

		private Vector3 _direction;

		[SerializeField]
		private PlayerDieEffectParameters _parameters;

		private Transform _cameraHelper;

		private Transform _cameraTransform;

		private Transform _pelvislTransform;

		private Vector3 _pelvisPosition;

		private void Awake()
		{
			_parameters = ResourcesUtility.Load<PlayerDieEffectParameters>("Camera/PlayerDieEffectParameters");
			GameObject gameObject = new GameObject();
			gameObject.name = "PlayerDieEffectHelper";
			GameObject gameObject2 = gameObject;
			_cameraHelper = gameObject2.transform;
		}

		[AsyncStateMachine(typeof(_003CPlayEffect_003Ec__async0))]
		[DebuggerStepThrough]
		public Task PlayEffect([NotNull] Camera cam, [NotNull] BipedMap character, [NotNull] Transform killerTransform)
		{
			_003CPlayEffect_003Ec__async0 stateMachine = default(_003CPlayEffect_003Ec__async0);
			stateMachine.cam = cam;
			stateMachine.character = character;
			stateMachine.killerTransform = killerTransform;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		private IEnumerator EffectLoop()
		{
			while (Time.time - _effectStartTime < _parameters.EffectDuration)
			{
				_pelvisPosition = Vector3.Lerp(_pelvisPosition, _pelvislTransform.position, Time.deltaTime * 20f);
				float dCoeff = (Time.time - _effectStartTime) / _parameters.EffectDuration;
				Vector3 cameraAngles = new Vector3(_parameters.XAxisRotationCurve.Evaluate(dCoeff), 0f, 0f);
				_cameraTransform.localEulerAngles = cameraAngles;
				_cameraHelper.position = _pelvisPosition + _direction * _parameters.DistancingCurve.Evaluate(dCoeff) + Vector3.up * _parameters.YAxisProjectionCurve.Evaluate(dCoeff);
				yield return new WaitForFixedUpdate();
			}
		}
	}
}
