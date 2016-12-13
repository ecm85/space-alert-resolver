using BLL.ShipComponents;

namespace BLL.Threats.Internal.Minor.White
{
	public abstract class Skirmishers : MinorWhiteInternalThreat
	{
		protected Skirmishers(StationLocation station)
			: base(1, 3, station, PlayerActionType.BattleBots)
		{
		}

		protected override void PerformYAction(int currentTurn)
		{
			ChangeDecks();
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(3);
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (!isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}
	}
}
