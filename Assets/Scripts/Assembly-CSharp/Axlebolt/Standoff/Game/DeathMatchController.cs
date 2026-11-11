using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Axlebolt.Standoff.Core;
using Axlebolt.Standoff.Game.State;
using Axlebolt.Standoff.Inventory;
using Axlebolt.Standoff.Player;
using I2.Loc;
using UnityEngine;

namespace Axlebolt.Standoff.Game
{
	public class DeathMatchController : GameController
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003COnPlayerSuicide_003Ec__async0 : IAsyncStateMachine
		{
			internal DeathMatchController _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private AsyncTools.Awaiter _0024awaiter0;

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
						_0024this._003COnPlayerSuicide_003E__BaseCallProxy0();
						_0024awaiter0 = _0024this._respawnTime.GetAwaiter();
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
					_0024this.SpawnPlayer();
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
		private struct _003COnPlayerWasKilled_003Ec__async1 : IAsyncStateMachine
		{
			internal HitEventArgs hitEventArgs;

			internal HitLogs takenHits;

			internal HitLogs _003CgivenHits_003E__1;

			internal DeathMatchController _0024this;

			internal AsyncVoidMethodBuilder _0024builder;

			internal int _0024PC;

			private TaskAwaiter _0024awaiter0;

			private AsyncTools.Awaiter _0024awaiter1;

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
						_0024this._003COnPlayerWasKilled_003E__BaseCallProxy1(hitEventArgs, takenHits);
						_003CgivenHits_003E__1 = _0024this.GetGamePlayer(hitEventArgs.Shooter).GetHits(_0024this.Player);
						_0024awaiter0 = _0024this.PlayDieWithFocusEffect(hitEventArgs.Shooter).GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						goto case 1u;
					case 1u:
						_0024awaiter0.GetResult();
						_0024this.WhoKillView.Show(hitEventArgs.Weapon, hitEventArgs.Shooter, takenHits, _003CgivenHits_003E__1);
						_0024awaiter1 = _0024this._respawnTime.GetAwaiter();
						if (!_0024awaiter1.IsCompleted)
						{
							_0024PC = 2;
							_0024builder.AwaitOnCompleted(ref _0024awaiter1, ref this);
							return;
						}
						break;
					case 2u:
						break;
					}
					_0024awaiter1.GetResult();
					_0024this.SpawnPlayer();
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
		private float _startingDuration;

		[SerializeField]
		private float _deathMatchDuration;

		[SerializeField]
		private float _winnersDuration;

		[SerializeField]
		private float _statisticsDuration;

		[SerializeField]
		private float _respawnTime;

		[SerializeField]
		private float _nextLevelVotingDuration;

		[SerializeField]
		private float _nextLevelVotingResultDuration;

		[SerializeField]
		private float _weaponBuyDuration;

		private Coroutine _buyWeaponsCoroutine;

		private WeaponId? _lastWeaponId;

		protected override void InitUi()
		{
			base.InitUi();
			PersonalGameStatsView.Show();
			PersonalGameStatsView.ScoreVisible = false;
			PersonalGameStatsView.TimeVisible = false;
			WeaponBuyView.TimeLeftTitle = ScriptLocalization.TeamDeathMatch.WeaponBuyTitle;
		}

		protected override GameStateRouter CreateStateRouter()
		{
			return GameStateRouter.Create(10, () => 21).Route(21, () => 30).Route(30, () => 200)
				.Route(200, () => 201)
				.Route(201, () => byte.MaxValue);
		}

		protected override void RegisterStates(GameStateMachine stateMachine)
		{
			stateMachine.Register(new WaitingPlayersState());
			stateMachine.Register(new StartingState(_startingDuration));
			stateMachine.Register(new RunningState(_deathMatchDuration));
			stateMachine.Register(new WinnersState(_winnersDuration));
			stateMachine.Register(new StatisticsState(_statisticsDuration));
		}

		protected override void InstantiatePlayer(Team team, SpawnPoint spawnPoint)
		{
			Singleton<PlayerManager>.Instance.Instantiate(team, spawnPoint.Position, spawnPoint.Rotation, 100, 100, true);
		}

		protected override void OnPlayerSpawned(PlayerController playerControlller)
		{
			base.OnPlayerSpawned(playerControlller);
			WeaponController spawnWeapon = GetSpawnWeapon();
			playerControlller.InventoryController.SetWeapon(spawnWeapon);
			_buyWeaponsCoroutine = StartCoroutine(BuyWeapons());
		}

		protected override void OnDestroyed(PlayerController playerControlller)
		{
			base.OnDestroyed(playerControlller);
			StopCoroutine(_buyWeaponsCoroutine);
		}

		protected override void OnPlayerKilled(HitEventArgs hitEventArgs)
		{
			base.OnPlayerKilled(hitEventArgs);
			base.Player.AddScore(12);
		}

		protected override void OnPlayerAssist(HitEventArgs hitEventArgs)
		{
			base.OnPlayerAssist(hitEventArgs);
			base.Player.AddScore(4);
		}

		[AsyncStateMachine(typeof(_003COnPlayerSuicide_003Ec__async0))]
		[DebuggerStepThrough]
		protected override void OnPlayerSuicide()
		{
			_003COnPlayerSuicide_003Ec__async0 stateMachine = default(_003COnPlayerSuicide_003Ec__async0);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		[AsyncStateMachine(typeof(_003COnPlayerWasKilled_003Ec__async1))]
		[DebuggerStepThrough]
		protected override void OnPlayerWasKilled(HitEventArgs hitEventArgs, HitLogs takenHits)
		{
			_003COnPlayerWasKilled_003Ec__async1 stateMachine = default(_003COnPlayerWasKilled_003Ec__async1);
			stateMachine.hitEventArgs = hitEventArgs;
			stateMachine.takenHits = takenHits;
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncVoidMethodBuilder.Create();
			stateMachine._0024builder.Start(ref stateMachine);
		}

		private IEnumerator BuyWeapons()
		{
			WeaponBuyButton.Show();
			while (CanBuyWeapons())
			{
				float timeLeft = _weaponBuyDuration - (Time.time - base.PlayerSpawnedTime);
				WeaponBuyView.TimeLeft = timeLeft;
				yield return new WaitForSeconds(0.1f);
			}
			WeaponBuyButton.Hide();
			HideWeaponBuyView();
		}

		private WeaponController GetSpawnWeapon()
		{
			WeaponId? lastWeaponId = _lastWeaponId;
			if (!lastWeaponId.HasValue)
			{
				_lastWeaponId = Singleton<WeaponManager>.Instance.GetRandomGunId(GunType.Heavy, GunType.Rifels, GunType.Smg);
			}
			WeaponManager instance = Singleton<WeaponManager>.Instance;
			WeaponId? lastWeaponId2 = _lastWeaponId;
			return instance.Get(lastWeaponId2.Value);
		}

		public override void BuyWeapon(WeaponParameters weapon)
		{
			base.BuyWeapon(weapon);
			_lastWeaponId = weapon.Id;
		}

		public override bool CanBuyWeapons()
		{
			return Time.time - base.PlayerSpawnedTime < _weaponBuyDuration;
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _003COnPlayerSuicide_003E__BaseCallProxy0()
		{
			base.OnPlayerSuicide();
		}

		[DebuggerHidden]
		[CompilerGenerated]
		private void _003COnPlayerWasKilled_003E__BaseCallProxy1(HitEventArgs hitEventArgs, HitLogs takenHits)
		{
			base.OnPlayerWasKilled(hitEventArgs, takenHits);
		}
	}
}
