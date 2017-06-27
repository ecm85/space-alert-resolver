using System;

namespace BLL
{
    public enum ResolutionPhase
    {
        AddNewThreats,
        PerformPlayerActions,
        ResolveDamage,
        MoveThreats,
        ComputerCheck,
		FinalRocketMove,
		InterceptorsReturnToShip,
		JumpToHyperspace
    }

    public static class ResolutionPhaseExtensions
    {
        public static string GetDescription(this ResolutionPhase phase)
        {
            switch (phase)
            {
                case ResolutionPhase.ComputerCheck:
                    return "Check Computer Maintenance";
                case ResolutionPhase.AddNewThreats:
                    return "Threats Appear";
                case ResolutionPhase.PerformPlayerActions:
                    return "Player Actions";
                case ResolutionPhase.ResolveDamage:
                    return "Resolve Damage";
                case ResolutionPhase.MoveThreats:
                    return "Threats Move";
				case ResolutionPhase.FinalRocketMove:
		            return "Final Rocket Moves";
				case ResolutionPhase.InterceptorsReturnToShip:
		            return "Interceptors Return to Ship";
				case ResolutionPhase.JumpToHyperspace:
		            return "Jump to Hyperspace";
				default:
                    throw new InvalidOperationException("Invalid resolution phase encountered.");
            }
        }
    }
}
