using System;
using BLL.Players;

namespace BLL.ShipComponents
{
    public class VisualConfirmationComponent : ICharlieComponent
    {
        public int NumberOfConfirmationsThisTurn { get; private set; }
        private int BestConfirmationTurnThisPhase { get; set; }
        public int TotalVisualConfirmationPoints { get; private set; }

        public void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvancedUsage)
        {
            NumberOfConfirmationsThisTurn += isAdvancedUsage ? 3 : 1;
        }

        public bool CanPerformCAction(Player performingPlayer)
        {
            return true;
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

        private static int GetVisualConfirmationPoints(int numberOfPlayers)
        {
            if (numberOfPlayers <= 3)
                return numberOfPlayers;
            return (numberOfPlayers - 3) * 2 + 3;
        }
    }
}
