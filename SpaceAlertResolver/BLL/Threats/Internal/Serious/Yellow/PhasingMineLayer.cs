using System.Collections.Generic;
using System.Linq;
using BLL.Common;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class PhasingMineLayer : SeriousYellowInternalThreat
	{
		private PhasingThreatCore phasingThreatCore;

		private IList<StationLocation> MineLocations => WarningIndicatorStations;

		public PhasingMineLayer()
			: base(2, 2, StationLocation.UpperWhite, PlayerActionType.BattleBots)
		{
		}

		public override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			phasingThreatCore = new PhasingThreatCore(this);
		}

		protected override void PerformXAction(int currentTurn)
		{
			LayMine();
			if (phasingThreatCore.WasPhasedOutAtStartOfTurn)
				MoveBlue();
			else
				MoveRed();
		}

		protected override void PerformYAction(int currentTurn)
		{
			LayMine();
			ChangeDecks();
		}

		protected override void PerformZAction(int currentTurn)
		{
			LayMine();
			DetonateMines();
		}

		public override bool IsDamageable => base.IsDamageable && phasingThreatCore.IsDamageable;

		public override bool IsMoveable => base.IsDamageable && phasingThreatCore.IsDamageable;

		private void LayMine()
		{
			MineLocations.Add(CurrentStation);
		}

		private void DetonateMines()
		{
			Damage(2, MineLocations.Select(mineLocation => mineLocation.ZoneLocation()).ToList());
		}

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

		public override string Id { get; } = "SI2-102";
		public override string DisplayName { get; } = "Phasing Mine Layer";
		public override string FileName { get; } = "PhasingMineLayer";
	}
}
