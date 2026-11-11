using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Bolt;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.Event;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player;
using UnityEngine;

namespace Axlebolt.Standoff.Game.Bolt
{
	public class PlayerStatsCollector : MonoBehaviour
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003COnGameExit_003Ec__async0 : IAsyncStateMachine
		{
			internal PlayerStatsCollector _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

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
						_0024this._stats.IncGamesPlayed(_0024this._gameMode);
						if (BoltApi.Instance.IsConnectedAndReady)
						{
							_0024awaiter0 = BoltService<BoltPlayerStats>.Instance.StoreStats().GetAwaiter();
							if (!_0024awaiter0.IsCompleted)
							{
								_0024PC = 1;
								_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
								return;
							}
							break;
						}
						goto end_IL_000e;
					case 1u:
						break;
					}
					_0024awaiter0.GetResult();
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

		private PlayerStats _stats;

		private string _gameMode;

		private void Awake()
		{
			_stats = BoltService<BoltPlayerStats>.Instance.Stats;
			GameController.Instance.GameInitEvent.AddListener(OnGameInit);
			GameController.Instance.GameExitEvent.AddListener(OnGameExit);
		}

		public void OnGameInit(GameInitEventArgs initEventArgs)
		{
			_gameMode = initEventArgs.GameMode;
			Singleton<HitManager>.Instance.HitEvent.AddListener(OnHit);
			GameController.Instance.PlayerAssistEvent.AddListener(OnPlayerAssist);
			GameController.Instance.PlayerKilledEvent.AddListener(OnPlayerKilled);
			GameController.Instance.PlayerWasKilledEvent.AddListener(OnPlayerWasKilled);
			Singleton<WeaponManager>.Instance.ShootEvent.AddListener(OnShoot);
		}

		public void OnShoot(WeaponController weaponController)
		{
			_stats.IncShots(_gameMode);
			_stats.IncGunShots(_gameMode, weaponController.WeaponParameters.Id);
		}

		private void OnHit(HitEventArgs e)
		{
			if (e.Shooter.IsLocal)
			{
				_stats.IncHits(_gameMode);
				_stats.IncGunHits(_gameMode, e.Weapon.Id);
				_stats.AddDamage(_gameMode, e.Damage);
				_stats.AddGunDamage(_gameMode, e.Weapon.Id, e.Damage);
			}
		}

		private void OnPlayerAssist(HitEventArgs e)
		{
			_stats.IncAssists(_gameMode);
		}

		private void OnPlayerKilled(HitEventArgs e)
		{
			_stats.IncKills(_gameMode);
			_stats.IncGunKills(_gameMode, e.Weapon.Id);
			if (e.Headshot)
			{
				_stats.IncHeadshots(_gameMode);
				_stats.IncGunHeadshots(_gameMode, e.Weapon.Id);
			}
		}

		private void OnPlayerWasKilled(HitEventArgs e)
		{
			_stats.IncDeaths(_gameMode);
		}

		[AsyncStateMachine(typeof(_003COnGameExit_003Ec__async0))]
		[DebuggerStepThrough]
		public void OnGameExit(GameExitEventArgs eventArgs)
		{
			_003COnGameExit_003Ec__async0 stateMachine = default(_003COnGameExit_003Ec__async0);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}
	}
}
