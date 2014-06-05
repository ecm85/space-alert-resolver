using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public class PowerPackOverload : MinorYellowInternalThreat
	{
		private ISet<StationLocation> StationsHitThisTurn { get; set; }

		public PowerPackOverload(int timeAppears, ISittingDuck sittingDuck)
			: base(4, 3, timeAppears, new List<StationLocation>{StationLocation.LowerBlue, StationLocation.LowerRed}, PlayerAction.A, sittingDuck)
		{
			StationsHitThisTurn = new HashSet<StationLocation>();
		}

		public static string GetDisplayName()
		{
			return "Power Pack Overload";
		}

		public override void PeformXAction()
		{
			((BattleBotsComponent)sittingDuck.RedZone.LowerStation.CComponent).DisableBattleBots();
			var rockets = sittingDuck.RocketsComponent.Rockets;
			if (rockets.Any())
				rockets.Remove(rockets.First());
			throw new NotImplementedException();
		}

		public override void PerformYAction()
		{
			Repair(1);
		}

		public override void PerformZAction()
		{
			sittingDuck.KnockOutPlayers(CurrentStations);
			Damage(3, CurrentZones);
		}

		public override void PerformEndOfPlayerActions()
		{
			if (CurrentStations.All(station => StationsHitThisTurn.Contains(station)))
				base.TakeDamage(1, null, false);
			StationsHitThisTurn.Clear();
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic)
		{
			StationsHitThisTurn.Add(performingPlayer.CurrentStation.StationLocation);
			base.TakeDamage(damage, performingPlayer, isHeroic);
		}
	}
}
