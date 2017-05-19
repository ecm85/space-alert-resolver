using System.Linq;
using BLL.ShipComponents;

namespace BLL.Threats.External.Serious.Yellow
{
	public class PsionicSatellite : SeriousYellowExternalThreat
	{
		public PsionicSatellite()
			: base(2, 5, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.ShiftPlayersAfterPlayerActions(new [] {CurrentZone}, currentTurn + 1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.ShiftPlayersAfterPlayerActions(EnumFactory.All<StationLocation>().Where(stationLocation => stationLocation.IsOnShip()), currentTurn + 1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayers(EnumFactory.All<StationLocation>().Where(stationLocation => stationLocation.IsOnShip()));
		}

		public override string Id { get; } = "SE2-03";
		public override string DisplayName { get; } = "Psionic Satellite";
		public override string FileName { get; } = "PsionicSatellite";

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return DistanceToShip != 3 && base.CanBeTargetedBy(damage);
		}
	}
}
