using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;

namespace BLL
{
	public class Player
	{
		public Player(IEnumerable<PlayerAction> actions, int index, PlayerColor color) :
			this(actions, index, color, null)
		{
		}

		public Player(IEnumerable<PlayerAction> actions, int index, PlayerColor color, PlayerSpecialization? specialization)
		{
			ActionsList = actions.ToList();
			Index = index;
			PlayerColor = color;
			Specialization = specialization;
		}

		public void Initialize(SittingDuck sittingDuck)
		{
			SittingDuck = sittingDuck;
		}

		private SittingDuck SittingDuck { get; set; }

		public bool IsKnockedOut { get; private set; }

		public void KnockOut()
		{
			KnockOut(false);
		}

		public void KnockOutFromOwnAction()
		{
			KnockOut(true);
		}

		private void KnockOut(bool wasCausedByOwnAction)
		{
			var isImmune = HasSpecialOpsProtection ||
				CurrentStation.Players.Any(player => player.PreventsKnockOut) ||
				(wasCausedByOwnAction && HadSpecialOpsProtectionWhenActing);
			if (!isImmune)
				IsKnockedOut = true;
			if (BattleBots != null)
				BattleBots.IsDisabled = true;
			while (!CurrentStation.StationLocation.IsOnShip())
			{
				CurrentStation.Players.Remove(this);
				SittingDuck.RedZone.UpperRedStation.MovePlayerIn(this);
			}
		}

		public int Index { get; }
		public PlayerColor PlayerColor { get; }
		public PlayerSpecialization? Specialization { get; }
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
		private bool HasSpecialOpsProtection { get; set; }
		private bool HadSpecialOpsProtectionWhenActing { get; set; }

		public bool IsCaptain => Index == 0;

		private bool PreventsKnockOut { get; set; }
		public void SetPreventsKnockOut(bool preventsKnockOut)
		{
			PreventsKnockOut = preventsKnockOut;
		}

		public void Shift(int turn)
		{
			if (HasSpecialOpsProtection)
				return;
			var currentAction = GetActionForTurn(turn - 1);
			if (currentAction.FirstActionPerformed && !currentAction.SecondActionPerformed)
			{
				var actionToShift = new PlayerAction(currentAction.SecondActionType, null, null);
				currentAction.SecondActionType = null;
				Shift(turn, actionToShift);
			}
			else
				Shift(turn, null);
		}

		public void ShiftAndRepeatPreviousAction(int turn)
		{
			var actionToRepeat = turn <= 0 ? null : GetActionForTurn(turn - 1);
			Shift(turn, actionToRepeat);
		}

		private void Shift(int turn, PlayerAction actionToInsert)
		{
			if (turn > ActionsList.Count)
				return;
			if (IsPerformingAdvancedSpecialOps(turn))
				return;
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
			return Specialization == PlayerSpecialization.Medic && IsPerformingAdvancedSpecialization(currentTurn);
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
			return Specialization == PlayerSpecialization.Medic && IsPerformingBasicSpecialization(currentTurn);
		}

		public bool IsPerformingAdvancedSpecialOps(int currentTurn)
		{
			return Specialization == PlayerSpecialization.SpecialOps && IsPerformingAdvancedSpecialization(currentTurn);
		}

		public void PadPlayerActions(int numberOfTurns)
		{
			var extraNullActions = Enumerable.Repeat(PlayerActionFactory.CreateEmptyAction(), numberOfTurns - ActionsList.Count);
			ActionsList.AddRange(extraNullActions);
		}

		public void MarkNextActionPerformed(int currentTurn)
		{
			var actionForTurn = GetActionForTurn(currentTurn);
			actionForTurn.MarkNextActionPerformed();
			if (actionForTurn.FirstActionPerformed)
				HasSpecialOpsProtection = false;
		}

		public PlayerActionType? GetNextActionToPerform(int currentTurn)
		{
			return GetActionForTurn(currentTurn).NextActionToPerform;
		}

		public void PerformStartOfPlayerActions(int currentTurn)
		{
			if (IsPerformingAdvancedSpecialOps(currentTurn))
			{
				HasSpecialOpsProtection = true;
				HadSpecialOpsProtectionWhenActing = true;
			}
		}

		public void PerformEndOfTurn()
		{
			SetPreventsKnockOut(false);
			HadSpecialOpsProtectionWhenActing = false;
		}
	}
}
