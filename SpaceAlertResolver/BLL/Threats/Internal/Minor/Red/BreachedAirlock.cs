using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Red
{
	public class BreachedAirlock : MinorRedInternalThreat
	{
		public BreachedAirlock()
			: base(3, 4, StationLocation.UpperRed, PlayerActionType.C)
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

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			var bonusDamage = performingPlayer.BattleBots != null && !performingPlayer.BattleBots.IsDisabled ? 1 : 0;
			base.TakeDamage(damage + bonusDamage, performingPlayer, isHeroic, stationLocation);
		}

		public static string GetDisplayName()
		{
			return "Breached Airlock";
		}
	}
}
