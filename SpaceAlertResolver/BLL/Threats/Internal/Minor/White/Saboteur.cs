using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
	public abstract class Saboteur : MinorWhiteInternalThreat
	{
		protected Saboteur()
			: base(1, 4, StationLocation.LowerWhite, PlayerActionType.BattleBots)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
			var energyDrained = SittingDuck.DrainReactors(new [] {CurrentZone}, 1);
			if (energyDrained == 0)
				Attack(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			Attack(2);
		}
	}
}
