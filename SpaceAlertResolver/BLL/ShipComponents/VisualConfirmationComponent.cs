using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class VisualConfirmationComponent : CComponent
	{
		public int NumberOfConfirmationsThisTurn { get; private set; }
		private int BestConfirmationTurnThisPhase { get; set; }
		public int TotalVisualConfirmationPoints { get; private set; }

		public override void PerformCAction(Player performingPlayer)
		{
			NumberOfConfirmationsThisTurn++;
		}

		public void PerformEndOfTurn()
		{
			BestConfirmationTurnThisPhase = Math.Max(BestConfirmationTurnThisPhase, NumberOfConfirmationsThisTurn);
			NumberOfConfirmationsThisTurn = 0;
		}

		public void PerformEndOfPhase()
		{
			TotalVisualConfirmationPoints += GetVisualConfirmationPoints(BestConfirmationTurnThisPhase);
			BestConfirmationTurnThisPhase = 0;
		}

		private int GetVisualConfirmationPoints(int numberOfPlayers)
		{
			if (numberOfPlayers <= 3)
				return numberOfPlayers;
			return (numberOfPlayers - 3) * 2 + 3;
		}
	}
}
