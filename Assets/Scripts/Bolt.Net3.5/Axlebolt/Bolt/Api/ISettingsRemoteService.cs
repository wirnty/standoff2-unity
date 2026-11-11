using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("SettingsRemoteService")]
	public interface ISettingsRemoteService
	{
		[Rpc("getSettings")]
		SettingsMessage.PlayerSetting[] GetSettings();

		[Rpc("updateSettings")]
		void UpdateSettings(SettingsMessage.PlayerSetting[] settings);

		[Rpc("updateSettingString")]
		void UpdateSetting(string key, string value);

		[Rpc("updateSettingBoolean")]
		void UpdateSetting(string key, bool? value);

		[Rpc("updateSettingFloat")]
		void UpdateSetting(string key, float? value);

		[Rpc("updateSettingInt")]
		void UpdateSetting(string key, int? value);

		[Rpc("getSetting")]
		SettingsMessage.PlayerSetting GetSetting(string key);
	}
}
