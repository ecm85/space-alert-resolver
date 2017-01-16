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

		private readonly IList<StationLocation> mineLocations;

		public PhasingMineLayer()
			: base(2, 2, StationLocation.UpperWhite, PlayerActionType.BattleBots)
		{
			mineLocations = new List<StationLocation>();
		}
		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
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
			mineLocations.Add(CurrentStation);
		}

		private void DetonateMines()
		{
			Damage(2, mineLocations.Select(mineLocation => mineLocation.ZoneLocation()).ToList());
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
