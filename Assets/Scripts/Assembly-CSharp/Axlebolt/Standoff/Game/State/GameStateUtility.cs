namespace Axlebolt.Standoff.Game.State
{
	public class GameStateUtility
	{
		public static double GetTimePassed()
		{
			return PhotonNetwork.time - PhotonNetwork.room.GetTime();
		}

		public static double GetTimeLeft(double duration)
		{
			double num = duration - GetTimePassed();
			if (num < 0.0)
			{
				num = 0.0;
			}
			return num;
		}
	}
}
