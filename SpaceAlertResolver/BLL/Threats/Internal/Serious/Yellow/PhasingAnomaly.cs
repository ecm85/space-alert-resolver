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
		
		public PhasingAnomaly(int timeAppears, ISittingDuck sittingDuck)
			: base(2, 3, timeAppears, StationLocation.UpperWhite, PlayerAction.C, sittingDuck, 1)
		{
		}

		public static string GetDisplayName()
		{
			return "Phasing Anomaly";
		}

		public override void PeformXAction()
		{
			//TODO: Disrupt upper white cannon optics
		}

		public override void PerformYAction()
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

		public override void PerformZAction()
		{
			sittingDuck.KnockOutPlayers(new[] {StationLocation.LowerWhite, StationLocation.UpperWhite});
			Damage(3);
			//TODO: Disruption effects persists (means have to track whats disrupted)
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
	}
}
