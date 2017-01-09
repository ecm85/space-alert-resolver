using System;

namespace BLL
{
	public enum ResolutionPhase
	{
		StartGame,
		AddNewThreats,
		PerformPlayerActions,
		ResolveDamage,
		MoveThreats,
		ComputerCheck,
		EndTurn
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
				case ResolutionPhase.StartGame:
					return "Start of Game";
				case ResolutionPhase.MoveThreats:
					return "Threats Move";
				case ResolutionPhase.EndTurn:
					return "End of Turn";
				default:
					throw new InvalidOperationException("Invalid resolution phase encountered.");
			}
		}
	}
}
