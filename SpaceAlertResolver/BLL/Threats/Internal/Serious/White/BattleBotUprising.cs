using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.White
{
	public class BattleBotUprising : SeriousWhiteInternalThreat
	{
		private ISet<StationLocation> StationsHitThisTurn { get; set; }

		public BattleBotUprising()
			: base(4, 2,new List<StationLocation> {StationLocation.UpperBlue, StationLocation.LowerRed}, PlayerAction.C)
		{
			StationsHitThisTurn = new HashSet<StationLocation>();
		}

		public override void PerformXAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayersWithBattleBots();
		}

		public override void PerformYAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayers(CurrentStations);
		}

		public override void PerformZAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayers(EnumFactory.All<StationLocation>().Except(new[] {StationLocation.UpperWhite}));
		}

		public static string GetDisplayName()
		{
			return "BattleBot Uprising";
		}

		protected override void PerformEndOfPlayerActionsOnTrack()
		{
			if (CurrentStations.All(station => StationsHitThisTurn.Contains(station)))
				base.TakeDamageOnTrack(1, null, false, CurrentStation);
			StationsHitThisTurn.Clear();
			base.PerformEndOfPlayerActionsOnTrack();
		}

		protected override void TakeDamageOnTrack(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			StationsHitThisTurn.Add(stationLocation);
			base.TakeDamageOnTrack(damage, performingPlayer, isHeroic, stationLocation);
		}
	}
}
