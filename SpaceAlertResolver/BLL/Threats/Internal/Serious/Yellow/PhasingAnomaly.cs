using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class PhasingAnomaly : SeriousYellowInternalThreat
	{
		//TODO: To handle this threat, going to have to calculate damage differently, because only after end of actions do we know if anyone has used the interceptors or visual confirmation to allow firing
		private bool isPhased;
		private bool wasPhasedAtStartOfTurn;

		private StationLocation? currentPhasedOutLocation;
		private int numberOfYsCrossed;
		
		public PhasingAnomaly()
			: base(2, 3, StationLocation.UpperWhite, PlayerAction.C, 1)
		{
		}

		public override void Initialize(ISittingDuck sittingDuck, ThreatController threatController, int timeAppears)
		{
			base.Initialize(sittingDuck, threatController, timeAppears);
			BeforeMove += PerformBeforeMove;
			AfterMove += PerformAfterMove;
		}

		public static string GetDisplayName()
		{
			return "Phasing Anomaly";
		}

		protected override void PerformXAction(int currentTurn)
		{
			//TODO: Disrupt upper white cannon optics
		}

		protected override void PerformYAction(int currentTurn)
		{
			switch (numberOfYsCrossed)
			{
				case 0:
					//TODO: Disrupt both red zone cannon optics
					break;
				case 1:
					//TODO: Disrupt both blue zone cannon optics
					break;
				default:
					throw new InvalidOperationException("Invalid number of Y's crossed.");
			}
			numberOfYsCrossed++;
		}

		protected override void PerformZAction(int currentTurn)
		{
			SittingDuck.KnockOutPlayers(new[] {StationLocation.LowerWhite, StationLocation.UpperWhite});
			Damage(3);
			//TODO: Disruption effects persists (means have to track whats disrupted)
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
