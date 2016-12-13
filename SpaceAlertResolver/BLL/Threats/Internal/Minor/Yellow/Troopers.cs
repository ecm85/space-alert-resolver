using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.Yellow
{
	public abstract class Troopers : MinorYellowInternalThreat
	{
		protected Troopers(StationLocation currentStation)
			: base(2, 2, currentStation, PlayerActionType.BattleBots)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			ChangeDecks();
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(4);
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (!isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}
	}
}
