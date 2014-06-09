using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class PhasingMineLayer : SeriousYellowInternalThreat
	{
		private bool isPhased;
		private bool wasPhasedAtStartOfTurn;

		private readonly IList<StationLocation> mineLocations;
		private StationLocation? currentPhasedOutLocation;

		public PhasingMineLayer(PlayerAction actionType)
			: base(2, 2, StationLocation.UpperWhite, actionType)
		{
			mineLocations = new List<StationLocation>();
		}

		public static string GetDisplayName()
		{
			return "Phasing Mine Layer";
		}

		public override void PerformXAction(int currentTurn)
		{
			LayMine();
			if (wasPhasedAtStartOfTurn)
				MoveBlue();
			else
				MoveRed();
		}

		public override void PerformYAction(int currentTurn)
		{
			LayMine();
			ChangeDecks();
		}

		public override void PerformZAction(int currentTurn)
		{
			LayMine();
			DetonateMines();
		}

		protected override void BeforeMove()
		{
			base.BeforeMove();
			if (isPhased)
			{
				if (!currentPhasedOutLocation.HasValue)
					throw new InvalidOperationException("At phase in, old station wasn't set to phase back into.");
				CurrentStation = currentPhasedOutLocation.Value;
				AddToStation(CurrentStation);
			}
			isPhased = false;
		}

		protected override void AfterMove()
		{
			base.AfterMove();
			isPhased = !wasPhasedAtStartOfTurn;
			if (isPhased)
			{
				currentPhasedOutLocation = CurrentStation;
				RemoveFromStation(CurrentStation);
			}
		}

		public override void PerformEndOfTurn()
		{
			if (IsOnTrack() || isPhased)
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

		protected override void TakeDamageOnTrack(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			base.TakeDamageOnTrack(damage, performingPlayer, isHeroic, stationLocation);
			if (!isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}
	}
}
