using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Axlebolt.Bolt;
using JetBrains.Annotations;
using UnityEngine;

namespace Axlebolt.Standoff.Main.Auth
{
	public class TestAuthService : AuthService
	{
		[SerializeField]
		[NotNull]
		private Sprite _sprite;

		public override Sprite Sprite
		{
			[CompilerGenerated]
			get
			{
				return _sprite;
			}
		}

		public override void Authenticate(Action<AuthResult> action)
		{
			action(AuthResult.Ok);
		}

		public override Task AuthenticateBolt()
		{
			return BoltApi.Instance.AuthTest("56c10982-cd05-4b9e-807e-af1e6154612f");
		}
	}
}
