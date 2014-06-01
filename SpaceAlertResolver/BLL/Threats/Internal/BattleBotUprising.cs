using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public class BattleBotUprising : SeriousWhiteInternalThreat
	{
		private ISet<Station> StationsHitThisTurn { get; set; }

		public BattleBotUprising(int timeAppears, SittingDuck sittingDuck)
			: base(4, 2, timeAppears, new List<Station> {sittingDuck.BlueZone.UpperStation, sittingDuck.RedZone.LowerStation}, PlayerAction.C, sittingDuck)
		{
			StationsHitThisTurn = new HashSet<Station>();
		}

		public override void PeformXAction()
		{
			var playersWithBattleBots = sittingDuck.Zones.SelectMany(zone => zone.Players).Where(player => player.BattleBots != null);
			KnockOut(playersWithBattleBots);
		}

		public override void PerformYAction()
		{
			var playersInCurrentStations = CurrentStations.SelectMany(station => station.Players);
			KnockOut(playersInCurrentStations);
		}

		public override void PerformZAction()
		{
			var playersNotOnBridge = sittingDuck.Zones.SelectMany(zone => zone.Players).Except(sittingDuck.WhiteZone.UpperStation.Players);
			KnockOut(playersNotOnBridge);
		}

		public override void PerformEndOfPlayerActions()
		{
			if (CurrentStations.All(station => StationsHitThisTurn.Contains(station)))
				base.TakeDamage(1, null);
			StationsHitThisTurn.Clear();
		}

		public override void TakeDamage(int damage, Player performingPlayer)
		{
			StationsHitThisTurn.Add(performingPlayer.CurrentStation);
			base.TakeDamage(damage, performingPlayer);
		}
	}
}
