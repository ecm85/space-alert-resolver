using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
	public abstract class HackedShields : MinorWhiteInternalThreat
	{
		protected HackedShields(StationLocation station)
			: base(3, 2, station, PlayerActionType.B)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.DrainShield(CurrentZone);
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.DrainReactor(CurrentZone);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(2);
		}
	}
}
