using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Red
{
	public class PhasingTroopersA : PhasingTroopers
	{
		public PhasingTroopersA() : base(StationLocation.LowerBlue)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
			MoveRed();
		}
	}
}
