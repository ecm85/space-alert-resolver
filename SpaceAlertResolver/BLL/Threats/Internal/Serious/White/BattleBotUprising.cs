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

		public override void Initialize(ISittingDuck sittingDuck, ThreatController threatController, int timeAppears)
		{
			base.Initialize(sittingDuck, threatController, timeAppears);
			ThreatController.EndOfPlayerActions += PerformEndOfPlayerActions;
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayersWithBattleBots();
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayers(CurrentStations);
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayers(EnumFactory.All<StationLocation>().Except(new[] {StationLocation.UpperWhite}));
		}

		public static string GetDisplayName()
		{
			return "BattleBot Uprising";
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
			base.OnHealthReducedToZero();
			ThreatController.EndOfPlayerActions -= PerformEndOfPlayerActions;
		}

		protected override void OnReachingEndOfTrack()
		{
			base.OnReachingEndOfTrack();
			ThreatController.EndOfPlayerActions -= PerformEndOfPlayerActions;
		}
	}
}
