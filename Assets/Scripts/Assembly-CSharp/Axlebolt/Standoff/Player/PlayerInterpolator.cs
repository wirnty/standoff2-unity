using Axlebolt.Networking;
using Axlebolt.Standoff.Player.Aim;
using Axlebolt.Standoff.Player.Inventory;
using Axlebolt.Standoff.Player.Mecanim;
using Axlebolt.Standoff.Player.Movement;
using Axlebolt.Standoff.Player.Networking;

namespace Axlebolt.Standoff.Player
{
	public class PlayerInterpolator : Interpolator
	{
		public MecanimInterpolator mecanimInterpolator = new MecanimInterpolator();

		public AimInterpolator aimInterpolator = new AimInterpolator();

		public MovementInterpolator movementInterpolator = new MovementInterpolator();

		public InventoryInterpolator inventoryInterpolator = new InventoryInterpolator();

		private PlayerSnapshot fromSnapshot;

		private PlayerSnapshot toSnapshot;

		private readonly PlayerSnapshot resultSnapshot = new PlayerSnapshot();

		public override ObjectSnapshot Interpolate(ObjectSnapshot a, ObjectSnapshot b, float progress)
		{
			fromSnapshot = (PlayerSnapshot)a;
			toSnapshot = (PlayerSnapshot)b;
			fromSnapshot.inventorySnapshot.time = fromSnapshot.time;
			toSnapshot.inventorySnapshot.time = toSnapshot.time;
			resultSnapshot.mecanimSnapshot = (MecanimSnapshot)mecanimInterpolator.Interpolate(fromSnapshot.mecanimSnapshot, toSnapshot.mecanimSnapshot, progress);
			resultSnapshot.aimingSnapshot = (AimSnapshot)aimInterpolator.Interpolate(fromSnapshot.aimingSnapshot, toSnapshot.aimingSnapshot, progress);
			resultSnapshot.inventorySnapshot = (InventorySnapshot)inventoryInterpolator.Interpolate(fromSnapshot.inventorySnapshot, toSnapshot.inventorySnapshot, progress);
			resultSnapshot.movementSnapshot = (MovementSnapshot)movementInterpolator.Interpolate(fromSnapshot.movementSnapshot, toSnapshot.movementSnapshot, progress);
			return resultSnapshot;
		}
	}
}
