using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;

namespace BLL
{
	public class Player
	{
		public Player(IEnumerable<PlayerAction> actions, int index, PlayerColor color) :
			this(actions, index, color, null, null)
		{
		}

		public Player(IEnumerable<PlayerAction> actions, int index, PlayerColor color, PlayerSpecialization? basicSpecialization, PlayerSpecialization? advancedSpecialization)
		{
			ActionsList = actions.ToList();
			Index = index;
			PlayerColor = color;
			BasicSpecialization = basicSpecialization;
			AdvancedSpecialization = advancedSpecialization;
		}

		private bool isKnockedOut;
		public bool IsKnockedOut {
			get { return isKnockedOut; }
			set
			{
				if (value || !CurrentStation.Players.Any(player => player.PreventsKnockOut))
					isKnockedOut = value;
			}
		}

		public int Index { get; }
		public PlayerColor PlayerColor { get; }
		public PlayerSpecialization? BasicSpecialization { get; }
		public PlayerSpecialization? AdvancedSpecialization { get; }
		public IEnumerable<PlayerAction> Actions => ActionsList;
		private List<PlayerAction> ActionsList { get; }

		public PlayerAction GetActionForTurn(int turn)
		{
			return ActionsList[turn - 1];
		}

		public Interceptors Interceptors { get; set; }
		public Station CurrentStation { get; set; }
		public BattleBots BattleBots { get; set; }
		public int BonusPoints { get; set; }
		public bool PlayerToTeleport { get; set; }
		public bool TeleportDestination { get; set; }
		public bool HasSpecialOpsProtection { get; set; }

		public bool IsCaptain => Index == 0;

		private bool PreventsKnockOut { get; set; }
		public void SetPreventsKnockOut(bool preventsKnockOut)
		{
			PreventsKnockOut = preventsKnockOut;
		}

		public void Shift(int turn)
		{
			//TODO: Shift partial double action if only one part was performed (do i have to track what was performed)
			Shift(turn, null);
		}

		public void ShiftAndRepeatPreviousAction(int turn)
		{
			var actionToRepeat = turn <= 0 ? null : GetActionForTurn(turn - 1);
			Shift(turn, actionToRepeat);
		}

		private void Shift(int turn, PlayerAction actionToInsert)
		{
			var endTurn = turn;
			while (endTurn < ActionsList.Count && GetActionForTurn(endTurn).FirstActionType.HasValue)
				endTurn++;
			ActionsList.Insert(turn - 1, new PlayerAction(actionToInsert?.FirstActionType, actionToInsert?.SecondActionType, actionToInsert?.BonusActionType));
			ActionsList.RemoveAt(endTurn);
		}

		public bool IsPerformingMedic(int currentTurn)
		{
			return IsPerformingAdvancedMedic(currentTurn) || IsPerformingBasicMedic(currentTurn);
		}

		private bool IsPerformingAdvancedMedic(int currentTurn)
		{
			return AdvancedSpecialization == PlayerSpecialization.Medic && IsPerformingAdvancedSpecialization(currentTurn);
		}

		private bool IsPerformingAdvancedSpecialization(int currentTurn)
		{
			return GetActionForTurn(currentTurn).BonusActionType == PlayerActionType.AdvancedSpecialization || GetActionForTurn(currentTurn).FirstActionType == PlayerActionType.AdvancedSpecialization;
		}

		private bool IsPerformingBasicSpecialization(int currentTurn)
		{
			return GetActionForTurn(currentTurn).BonusActionType == PlayerActionType.BasicSpecialization || GetActionForTurn(currentTurn).FirstActionType == PlayerActionType.BasicSpecialization;
		}

		private bool IsPerformingBasicMedic(int currentTurn)
		{
			return AdvancedSpecialization == PlayerSpecialization.Medic && IsPerformingBasicSpecialization(currentTurn);
		}

		public bool IsPerformingAdvancedSpecialOps(int currentTurn)
		{
			return AdvancedSpecialization == PlayerSpecialization.SpecialOps && IsPerformingAdvancedSpecialization(currentTurn);
		}

		public void PadPlayerActions(int numberOfTurns)
		{
			var extraNullActions = Enumerable.Repeat(PlayerActionFactory.CreateEmptyAction(), numberOfTurns - ActionsList.Count);
			ActionsList.AddRange(extraNullActions);
		}
	}
}
