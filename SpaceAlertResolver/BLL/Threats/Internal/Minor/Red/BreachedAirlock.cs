using BLL.Common;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Red
{
	public class BreachedAirlock : MinorRedInternalThreat
	{
		public BreachedAirlock()
			: base(3, 4, StationLocation.UpperRed, PlayerActionType.Charlie)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.BreachRedAirlock();
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.BreachBlueAirlock();
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(3);
			SittingDuck.KnockOutPlayers(new [] {CurrentZone});
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnHealthReducedToZero();
			SittingDuck.RepairAllAirlockBreaches();
		}

		public override string Id { get; } = "I3-104";
		public override string DisplayName { get; } = "Breached Airlock";
		public override string FileName { get; } = "BreachedAirlock";

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
			var bonusDamage = performingPlayer.BattleBots != null && !performingPlayer.BattleBots.IsDisabled ? 1 : 0;
			base.TakeDamage(damage + bonusDamage, performingPlayer, isHeroic, stationLocation);
		}
	}
}
