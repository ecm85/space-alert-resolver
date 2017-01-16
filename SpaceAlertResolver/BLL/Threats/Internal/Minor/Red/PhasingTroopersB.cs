using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Red
{
	public class PhasingTroopersB : PhasingTroopers
	{
		public PhasingTroopersB() : base(StationLocation.LowerRed)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
			MoveBlue();
		}

		public override string Id { get; } = "I3-105";
		public override string DisplayName { get; } = "Phasing Troopers";
		public override string FileName { get; } = "PhasingTroopersB";
	}
}
