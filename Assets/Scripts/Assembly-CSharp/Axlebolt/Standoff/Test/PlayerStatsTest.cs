using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Axlebolt.Bolt;
using Axlebolt.Standoff.Game.Bolt;
using Axlebolt.Standoff.Inventory;
using UnityEngine.TestTools;

namespace Axlebolt.Standoff.Test
{
	public class PlayerStatsTest : BoltSupportTest
	{
		[StructLayout(LayoutKind.Auto)]
		[CompilerGenerated]
		private struct _003CPlayerStatsSaveTestAsync_003Ec__async1 : IAsyncStateMachine
		{
			internal WeaponParameters[] _003Cweapons_003E__1;

			internal PlayerStats _003Cstats_003E__1;

			internal string[] _0024locvar0;

			internal int _0024locvar1;

			internal PlayerStatsTest _0024this;

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
						_0024awaiter0 = _0024this.AuthTest().GetAwaiter();
						if (!_0024awaiter0.IsCompleted)
						{
							_0024PC = 1;
							_0024builder.AwaitUnsafeOnCompleted(ref _0024awaiter0, ref this);
							return;
						}
						goto case 1u;
					case 1u:
						_0024awaiter0.GetResult();
						_003Cweapons_003E__1 = WeaponUtility.LoadWeapons();
						_003Cstats_003E__1 = BoltService<BoltPlayerStats>.Instance.Stats;
						_0024locvar0 = GameModes;
						for (_0024locvar1 = 0; _0024locvar1 < _0024locvar0.Length; _0024locvar1++)
						{
							string gameMode = _0024locvar0[_0024locvar1];
							_003Cstats_003E__1.IncDeaths(gameMode);
							_003Cstats_003E__1.IncKills(gameMode);
							_003Cstats_003E__1.IncAssists(gameMode);
							_003Cstats_003E__1.IncHits(gameMode);
							_003Cstats_003E__1.IncShots(gameMode);
							_003Cstats_003E__1.IncHeadshots(gameMode);
							_003Cstats_003E__1.IncGamesPlayed(gameMode);
							_003Cstats_003E__1.AddDamage(gameMode, 100);
							WeaponParameters[] array = _003Cweapons_003E__1;
							foreach (WeaponParameters weaponParameters in array)
							{
								_003Cstats_003E__1.IncGunHits(gameMode, weaponParameters.Id);
								_003Cstats_003E__1.IncGunShots(gameMode, weaponParameters.Id);
								_003Cstats_003E__1.IncGunHeadshots(gameMode, weaponParameters.Id);
								_003Cstats_003E__1.IncGunKills(gameMode, weaponParameters.Id);
								_003Cstats_003E__1.AddGunDamage(gameMode, weaponParameters.Id, 100);
							}
						}
						_0024awaiter0 = BoltService<BoltPlayerStats>.Instance.StoreStats().GetAwaiter();
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

		private static readonly string[] GameModes = new string[1] { "DeathMatch" };

		public IEnumerator PlayerStatsSaveTest()
		{
			yield return Async(PlayerStatsSaveTestAsync());
		}

		[DebuggerStepThrough]
		[AsyncStateMachine(typeof(_003CPlayerStatsSaveTestAsync_003Ec__async1))]
		private Task PlayerStatsSaveTestAsync()
		{
			_003CPlayerStatsSaveTestAsync_003Ec__async1 stateMachine = default(_003CPlayerStatsSaveTestAsync_003Ec__async1);
			stateMachine._0024this = this;
			stateMachine._0024builder = AsyncTaskMethodBuilder.Create();
			ref AsyncTaskMethodBuilder _0024builder = ref stateMachine._0024builder;
			_0024builder.Start(ref stateMachine);
			return _0024builder.Task;
		}
	}
}
