using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

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

		public override void Initialize(ISittingDuck sittingDuck, ThreatController threatController, int timeAppears)
		{
			base.Initialize(sittingDuck, threatController, timeAppears);
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
			if (isPhased)
			{
				if (!currentPhasedOutLocation.HasValue)
					throw new InvalidOperationException("At phase in, old station wasn't set to phase back into.");
				CurrentStation = currentPhasedOutLocation.Value;
				AddToStation(CurrentStation);
			}
			isPhased = false;
		}

		private void PerformAfterMove()
		{
			isPhased = !wasPhasedAtStartOfTurn;
			if (isPhased)
			{
				currentPhasedOutLocation = CurrentStation;
				RemoveFromStation(CurrentStation);
			}
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

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (!isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}

		protected override void OnHealthReducedToZero()
		{
			BeforeMove += PerformBeforeMove;
			AfterMove += PerformAfterMove;
			base.OnHealthReducedToZero();
		}

		protected override void OnReachingEndOfTrack()
		{
			BeforeMove += PerformBeforeMove;
			AfterMove += PerformAfterMove;
			base.OnReachingEndOfTrack();
		}
	}
}
