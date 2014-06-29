using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL
{
	public class Player
	{
		private bool isKnockedOut;
		public bool IsKnockedOut {
			get { return isKnockedOut; }
			set
			{
				if (value || !CurrentStation.Players.Any(player => player.PreventsKnockOut))
					isKnockedOut = value;
			}
		}
		public Interceptors Interceptors { get; set; }
		public List<PlayerAction> Actions { get; set; }
		public Station CurrentStation { get; set; }
		public BattleBots BattleBots { get; set; }
		public int Index { get; set; }
		public bool IsCaptain { get; set; }
		public PlayerSpecialization? BasicSpecialization { get; set; }
		public PlayerSpecialization? AdvancedSpecialization { get; set; }
		public int BonusPoints { get; set; }
		public bool PlayerToTeleport { get; set; }
		public bool TeleportDestination { get; set; }
		public bool PreventsKnockOut { get; set; }
		public bool HasSpecialOpsProtection { get; set; }

		public void Shift(int turn, bool repeatPreviousAction = false)
		{
			var endTurn = turn;
			while (endTurn + 1 < Actions.Count && Actions[endTurn].ActionType.HasValue)
				endTurn++;
			var actionToInsert = repeatPreviousAction ? Actions[turn - 1].ActionType : null;
			Actions.Insert(turn, new PlayerAction{ActionType = actionToInsert});
			Actions.RemoveAt(endTurn + 1);
		}

		public bool IsPerformingMedic(int currentTurn)
		{
			return IsPerformingBasicMedic(currentTurn) || IsPerformingAdvancedMedic(currentTurn);
		}

		private bool IsPerformingAdvancedMedic(int currentTurn)
		{
			return IsPerformingAdvancedMedicWithMovement(currentTurn) || IsPerformingAdvancedMedicWithoutMovement(currentTurn);
		}

		private bool IsPerformingAdvancedMedicWithoutMovement(int currentTurn)
		{
			return AdvancedSpecialization == PlayerSpecialization.Medic && Actions[currentTurn].ActionType == PlayerActionType.AdvancedSpecialization;
		}

		public bool IsPerformingAdvancedMedicWithMovement(int currentTurn)
		{
			var currentAction = Actions[currentTurn];
			return AdvancedSpecialization == PlayerSpecialization.Medic && currentAction.HasAdvancedSpecializationAttached && currentAction.ActionType.IsBasicMovement();
		}

		private bool IsPerformingBasicMedic(int currentTurn)
		{
			return IsPerformingBasicMedicWithMovement(currentTurn) || IsPerformingBasicMedicWithoutMovement(currentTurn);
		}

		private bool IsPerformingBasicMedicWithoutMovement(int currentTurn)
		{
			return BasicSpecialization == PlayerSpecialization.Medic && Actions[currentTurn].ActionType == PlayerActionType.BasicSpecialization;
		}

		public bool IsPerformingBasicMedicWithMovement(int currentTurn)
		{
			var currentAction = Actions[currentTurn];
			return BasicSpecialization == PlayerSpecialization.Medic && currentAction.HasBasicSpecializationAttached && currentAction.ActionType.IsBasicMovement();
		}

		public bool IsPerformingAdvancedSpecialOps(int currentTurn)
		{
			var currentAction = Actions[currentTurn];
			//TODO: Rules clarification: Can you play advanced spec ops without another card?
			//if so, need to consider both playing as secondary card with a null first card, or playing as a first card
			return AdvancedSpecialization == PlayerSpecialization.SpecialOps && currentAction.HasAdvancedSpecializationAttached;
		}
	}
}
