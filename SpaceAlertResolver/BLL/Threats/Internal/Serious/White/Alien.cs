using BLL.Common;
using BLL.Players;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.White
{
	public class Alien : SeriousWhiteInternalThreat
	{
		private bool GrownUp
		{
			get { return GetThreatStatus(ThreatStatus.GrownUp); }
			set { SetThreatStatus(ThreatStatus.GrownUp, value); }
		}

		internal Alien()
			: base(2, 2, StationLocation.LowerWhite, PlayerActionType.BattleBots)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			GrownUp = true;
		}

		protected override void PerformYAction(int currentTurn)
		{
			ChangeDecks();
			Damage(SittingDuck.GetPlayerCount(CurrentStation));
		}

		protected override void PerformZAction(int currentTurn)
		{
			throw new LoseException(this);
		}

		public override string Id { get; } = "SI1-03";
		public override string DisplayName { get; } = "Alien";
		public override string FileName { get; } = "Alien";

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (GrownUp && !isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}
	}
}
