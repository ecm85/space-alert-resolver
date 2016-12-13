using BLL.Common;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Minor.Red
{
	public abstract class PhasingTroopers : MinorRedInternalThreat
	{
		private PhasingThreatCore phasingThreatCore;

		protected PhasingTroopers(StationLocation currentStation)
			: base(2, 2, currentStation, PlayerActionType.BattleBots)
		{
		}

		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			phasingThreatCore = new PhasingThreatCore(this);
		}

		protected override void PerformXAction(int currentTurn)
		{
			if (!phasingThreatCore.WasPhasedOutAtStartOfTurn)
				ChangeDecks();
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(phasingThreatCore.WasPhasedOutAtStartOfTurn ? 3 : 4);
		}

		public override bool IsDamageable => base.IsDamageable && phasingThreatCore.IsDamageable;

		public override bool IsMoveable => base.IsDamageable && phasingThreatCore.IsDamageable;

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation? stationLocation)
		{
			Check.ArgumentIsNotNull(performingPlayer, "performingPlayer");
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (!isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}

		protected override void OnThreatTerminated()
		{
			phasingThreatCore.ThreatTerminated();
			base.OnThreatTerminated();
		}
	}
}
