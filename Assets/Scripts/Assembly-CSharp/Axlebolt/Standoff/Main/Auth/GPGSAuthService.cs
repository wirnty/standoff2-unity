using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Axlebolt.Bolt;
using Axlebolt.Standoff.Core;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Auth
{
	public class GPGSAuthService : AuthService
	{
		private static readonly Log Log = Log.Create(typeof(GPGSAuthService));

		[SerializeField]
		[NotNull]
		private Sprite _sprite;

		private string _serverAuthCode;

		public override Sprite Sprite
		{
			[CompilerGenerated]
			get
			{
				return _sprite;
			}
		}

		public GPGSAuthService()
		{
			PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false).AddOauthScope("profile").RequestIdToken()
				.Build();
			PlayGamesPlatform.InitializeInstance(configuration);
			PlayGamesPlatform.Activate();
		}

		public override void Authenticate(Action<AuthResult> action)
		{
			if (Log.DebugEnabled)
			{
				Log.Debug("PlayGamesPlatform Authenticate");
			}
			PlayGamesPlatform.Instance.Authenticate(delegate(bool success)
			{
				_serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();
				if (success && !string.IsNullOrEmpty(_serverAuthCode))
				{
					action(AuthResult.Ok);
				}
				else if (!success)
				{
					action(AuthResult.ServiceError);
				}
				else
				{
					action(AuthResult.ConnectionError);
				}
			});
		}

		public override Task AuthenticateBolt()
		{
			return BoltApi.Instance.AuthGPGS(_serverAuthCode);
		}
	}
}
