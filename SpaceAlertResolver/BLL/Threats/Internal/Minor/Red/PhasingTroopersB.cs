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
	}
}
