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

		public PhasingMineLayer(int health, int speed, int timeAppears, StationLocation currentStation, PlayerAction actionType, ISittingDuck sittingDuck) : base(health, speed, timeAppears, currentStation, actionType, sittingDuck)
		{
			mineLocations = new List<StationLocation>();
		}

		public static string GetDisplayName()
		{
			return "Phasing Mine Layer";
		}

		public override void PeformXAction()
		{
			LayMine();
			if (wasPhasedAtStartOfTurn)
				MoveBlue();
			else
				MoveRed();
		}

		public override void PerformYAction()
		{
			LayMine();
			ChangeDecks();
		}

		public override void PerformZAction()
		{
			LayMine();
			DetonateMines();
		}

		public override void BeforeMove()
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

		public override void AfterMove()
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

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (!isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}
	}
}
