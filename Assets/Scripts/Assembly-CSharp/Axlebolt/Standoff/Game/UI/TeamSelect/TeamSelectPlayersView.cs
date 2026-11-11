using Axlebolt.Standoff.UI;

namespace Axlebolt.Standoff.Game.UI.TeamSelect
{
	public class TeamSelectPlayersView : View
	{
		public TeamSelectPlayerView playerPrefab;

		private ViewPool<TeamSelectPlayerView> itemsPool;

		private void Awake()
		{
			itemsPool = new ViewPool<TeamSelectPlayerView>(playerPrefab, 5);
		}

		public void Refresh(PhotonPlayer[] players)
		{
			TeamSelectPlayerView[] items = itemsPool.GetItems(players.Length);
			for (int i = 0; i < items.Length; i++)
			{
				items[i].Refresh(players[i]);
			}
		}
	}
}
