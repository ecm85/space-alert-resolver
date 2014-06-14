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

		public PowerPackOverload()
			: base(
				4,
				3,
				new List<StationLocation> {StationLocation.LowerBlue, StationLocation.LowerRed},
				PlayerAction.A)
		{
			StationsHitThisTurn = new HashSet<StationLocation>();
		}

		public override void Initialize(ISittingDuck sittingDuck, ThreatController threatController, int timeAppears)
		{
			base.Initialize(sittingDuck, threatController, timeAppears);
			ThreatController.EndOfPlayerActions += PerformEndOfPlayerActions;
		}

		public static string GetDisplayName()
		{
			return "Power Pack Overload";
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.DisableInactiveBattlebots(new[] {StationLocation.LowerRed});
			SittingDuck.RemoveRocket();
		}

		protected override void PerformYAction(int currentTurn)
		{
			Repair(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayers(CurrentStations);
			Damage(3, CurrentZones);
		}

		private void PerformEndOfPlayerActions()
		{
			if (CurrentStations.All(station => StationsHitThisTurn.Contains(station)))
				base.TakeDamage(1, null, false, CurrentStation);
			StationsHitThisTurn.Clear();
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			StationsHitThisTurn.Add(stationLocation);
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
		}

		protected override void OnHealthReducedToZero()
		{
			ThreatController.EndOfPlayerActions -= PerformEndOfPlayerActions;
			base.OnHealthReducedToZero();
		}

		protected override void OnReachingEndOfTrack()
		{
			ThreatController.EndOfPlayerActions -= PerformEndOfPlayerActions;
			base.OnReachingEndOfTrack();
		}
	}
}
