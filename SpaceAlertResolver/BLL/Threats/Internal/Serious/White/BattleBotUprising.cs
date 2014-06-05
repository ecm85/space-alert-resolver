using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.White
{
	public class BattleBotUprising : SeriousWhiteInternalThreat
	{
		private ISet<StationLocation> StationsHitThisTurn { get; set; }

		public BattleBotUprising(int timeAppears, ISittingDuck sittingDuck)
			: base(4, 2, timeAppears, new List<StationLocation> {StationLocation.UpperBlue, StationLocation.LowerRed}, PlayerAction.C, sittingDuck)
		{
			StationsHitThisTurn = new HashSet<StationLocation>();
		}

		public override void PeformXAction()
		{
			sittingDuck.KnockOutPlayersWithBattleBots();
		}

		public override void PerformYAction()
		{
			sittingDuck.KnockOutPlayers(CurrentStations);
		}

		public override void PerformZAction()
		{
			sittingDuck.KnockOutPlayers(EnumFactory.All<StationLocation>().Except(new[] {StationLocation.UpperWhite}));
		}

		public static string GetDisplayName()
		{
			return "BattleBot Uprising";
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
