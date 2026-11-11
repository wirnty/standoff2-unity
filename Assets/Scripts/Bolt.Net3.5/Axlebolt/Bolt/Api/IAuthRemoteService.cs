using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("AuthRemoteService")]
	public interface IAuthRemoteService
	{
		[Rpc("authGPGS")]
		string AuthGPGS(string gameId, string tempToken);

		[Rpc("authGC")]
		string AuthGC(string gameId, string token);

		[Rpc("authFb")]
		string AuthFb(string gameId, string token);

		[Rpc("authTest")]
		string AuthTest(string gameId, string token);
	}
}
