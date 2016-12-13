using System.Collections.Generic;
using System.Linq;
using BLL.Common;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class PhasingMineLayer : SeriousYellowInternalThreat
	{
		private bool isPhased;
		private bool wasPhasedAtStartOfTurn;

		private readonly IList<StationLocation> mineLocations;

		public PhasingMineLayer()
			: base(2, 2, StationLocation.UpperWhite, PlayerActionType.BattleBots)
		{
			mineLocations = new List<StationLocation>();
		}
		public override void PlaceOnBoard(Track track, int? trackPosition)
		{
			base.PlaceOnBoard(track, trackPosition);
			BeforeMove += PerformBeforeMove;
			AfterMove += PerformAfterMove;
		}

		protected override void PerformXAction(int currentTurn)
		{
			LayMine();
			if (wasPhasedAtStartOfTurn)
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

		private void PerformBeforeMove()
		{
			isPhased = false;
		}

		private void PerformAfterMove()
		{
			isPhased = !wasPhasedAtStartOfTurn;
		}

		public override bool IsDamageable
		{
			get { return base.IsDamageable && !isPhased; }
		}

		public override bool IsMoveable
		{
			get { return base.IsDamageable && !isPhased; }
		}

		protected override void PerformEndOfTurn()
		{
			if (isPhased)
				wasPhasedAtStartOfTurn = isPhased;
			base.PerformEndOfTurn();
		}

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
			BeforeMove += PerformBeforeMove;
			AfterMove += PerformAfterMove;
			base.OnThreatTerminated();
		}
	}
}
