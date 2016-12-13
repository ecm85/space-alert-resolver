using System.Collections.Generic;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class Contamination : SeriousYellowInternalThreat
	{
		public Contamination()
			: base(
				3,
				2,
				new List<StationLocation>
				{
					StationLocation.UpperBlue,
					StationLocation.UpperRed,
					StationLocation.LowerBlue,
					StationLocation.LowerRed
				},
				PlayerActionType.BattleBots)
		{
		}
		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.ShiftPlayers(CurrentStations, currentTurn + 1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Damage(1, CurrentZones);
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayers(CurrentStations);
			SittingDuck.SubscribeToMoveIn(CurrentStations, KnockOutPlayer);
		}

		private void KnockOutPlayer(Player performingPlayer, int currentTurn)
		{
			performingPlayer.IsKnockedOut = true;
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (stationLocation != null)
				CurrentStations.Remove(stationLocation.Value);
		}
	}
}
