using Axlebolt.Bolt.Protobuf;
using Axlebolt.RpcSupport;

namespace Axlebolt.Bolt.Api
{
	[RpcService("InventoryRemoteService")]
	public interface IInventoryRemoteService
	{
		[Rpc("getInventoryItems")]
		InventoryMessage.InventoryItem[] GetInventoryItems();

		[Rpc("getPlayerInventory")]
		InventoryMessage.PlayerInventory GetPlayerInventory();

		[Rpc("buyInventoryItem")]
		InventoryMessage.InventoryItemStack BuyInventoryItem(int inventoryItemId, int quantity, int currencyId);

		[Rpc("sellInventoryItem")]
		InventoryMessage.InventoryItemStack SellInventoryItem(int inventoryItemStackId, int quantity, int currencyId);

		[Rpc("consumeInventoryItem")]
		InventoryMessage.InventoryItemStack ConsumeInventoryItem(int inventoryItemStackId, int quantity);

		[Rpc("transferInventoryItems")]
		InventoryMessage.InventoryItemStack[] TransferInventoryItems(int fromStackId, int toStackId, int quantity);

		[Rpc("tradeInventoryItems")]
		InventoryMessage.InventoryItemStack[] TradeInventoryItems(int[] localStackId, CurrencyMessage.CurrencyAmount[] localCurrencyAmount, long? remotePlayerId, int[] remoteStackId, CurrencyMessage.CurrencyAmount[] remoteCurrencyAmount);
	}
}
