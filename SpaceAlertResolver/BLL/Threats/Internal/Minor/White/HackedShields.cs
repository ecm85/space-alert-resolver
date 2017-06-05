using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
	public abstract class HackedShields : MinorWhiteInternalThreat
	{
		protected HackedShields(StationLocation station)
			: base(3, 2, station, PlayerActionType.Bravo)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.DrainShields(new [] {CurrentZone});
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.DrainReactors(new [] {CurrentZone});
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(2);
		}
	}
}
