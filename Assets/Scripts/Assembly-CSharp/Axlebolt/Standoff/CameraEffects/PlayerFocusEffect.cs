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
using UnityEngine.UI;

namespace Axlebolt.Standoff.CameraEffects
{
	public class PlayerFocusEffect : MonoBehaviour
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CPlayEffect_003Ec__async0 : IAsyncStateMachine
		{
			internal Camera cam;

			internal Transform characterTransform;

			internal RawImage screenshotBackground;

			internal Vector3 _003Cdir_003E__1;

			internal PlayerFocusEffect _0024this;

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
						if (characterTransform == null)
						{
							throw new ArgumentNullException("characterTransform");
						}
						if (screenshotBackground == null)
						{
							throw new ArgumentNullException("screenshotBackground");
						}
						_0024this._camera = cam;
						_0024this._cameraTransform = cam.transform;
						_0024this._characterTransform = characterTransform;
						_0024this._screenshotBackground = screenshotBackground;
						_0024this._effectStartTime = Time.time;
						_003Cdir_003E__1 = characterTransform.position - _0024this._cameraTransform.position;
						_0024this._cameraHelper.position = _0024this._cameraTransform.position;
						_0024this._cameraHelper.rotation = Quaternion.LookRotation(_003Cdir_003E__1);
						_0024this._cameraTransform.SetParent(_0024this._cameraHelper, true);
						_0024this._cameraInitialLocalAngles = _0024this._cameraTransform.localEulerAngles;
						_0024this._effectDuration = _0024this._parameters.DurationCurve.Evaluate(Vector3.Distance(_0024this._cameraTransform.position, characterTransform.position));
						_0024this._cameraStartPosition = _0024this._cameraHelper.position;
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

		private PlayerFocusEffectParameters _parameters;

		private Transform _cameraHelper;

		private Transform _cameraTransform;

		private Transform _characterTransform;

		private RawImage _screenshotBackground;

		private float _effectStartTime;

		private float _effectDuration;

		private float _screenshotTime;

		private Vector3 _cameraInitialLocalAngles;

		private Vector3 _cameraStartPosition;

		private Camera _camera;

		private RawImage _screenshotSprite;

		private Texture2D _screenShot;

		private void Awake()
		{
			_parameters = ResourcesUtility.Load<PlayerFocusEffectParameters>("Camera/PlayerFocusEffectParameters");
			GameObject gameObject = new GameObject();
			gameObject.name = "OnCharacterFocusCameraHelper";
			GameObject gameObject2 = gameObject;
			_cameraHelper = gameObject2.transform;
			_screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CPlayEffect_003Ec__async0))]
		public Task PlayEffect([NotNull] Camera cam, [NotNull] Transform characterTransform, [NotNull] RawImage screenshotBackground)
		{
			_003CPlayEffect_003Ec__async0 stateMachine = default(_003CPlayEffect_003Ec__async0);
			stateMachine.cam = cam;
			stateMachine.characterTransform = characterTransform;
			stateMachine.screenshotBackground = screenshotBackground;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}

		private IEnumerator EffectLoop()
		{
			while (Time.time - _effectStartTime <= _effectDuration)
			{
				float dTime = (Time.time - _effectStartTime) / _effectDuration;
				float distancingCoeff = _parameters.DistancingCurve.Evaluate(dTime);
				Vector3 characterPosition = _characterTransform.position + Vector3.up * _parameters.CharacterHeightOffset;
				Vector3 targetPosition = characterPosition + (_cameraHelper.transform.position - characterPosition).normalized * _parameters.CharacterForwardOffset;
				_cameraHelper.position = Vector3.Lerp(_cameraStartPosition, targetPosition, distancingCoeff);
				Vector3 dir = (characterPosition - _cameraHelper.position).normalized;
				_cameraHelper.rotation = Quaternion.Lerp(_cameraHelper.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 15f);
				float localAngleCorrectCoeff = _parameters.CameraLocalAnglesCorrectingCurve.Evaluate(dTime);
				_cameraTransform.localEulerAngles = VectorAngle.LerpEulerAngle(_cameraInitialLocalAngles, Vector3.zero, localAngleCorrectCoeff);
				yield return null;
			}
			TakeScreenshot();
		}

		private void TakeScreenshot()
		{
			int width = Screen.width;
			int height = Screen.height;
			RenderTexture renderTexture = new RenderTexture(width, height, 24);
			_camera.targetTexture = renderTexture;
			_camera.Render();
			RenderTexture.active = renderTexture;
			_screenShot.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
			_camera.targetTexture = null;
			RenderTexture.active = null;
			UnityEngine.Object.Destroy(renderTexture);
			_screenShot.Apply();
			_screenshotBackground.texture = _screenShot;
			_screenshotBackground.gameObject.SetActive(true);
			_screenshotBackground.enabled = true;
		}

		public void FinishFocusing()
		{
			_screenshotBackground.gameObject.SetActive(false);
		}
	}
}
