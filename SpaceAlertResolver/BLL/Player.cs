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

		public void ShiftAfterPlayerActions(int turn)
		{
			if (HasSpecialOpsProtection)
				return;
			Shift(turn + 1, null);
		}

		public void ShiftFromPlayerActions(int turn)
		{
			if (HasSpecialOpsProtection)
				return;
			var currentAction = GetActionForTurn(turn);
			var firstActionWasStarted = currentAction.FirstActionSegment.SegmentStatus != PlayerActionStatus.NotPerformed;
			var secondActionWasStarted = currentAction.SecondActionSegment.SegmentType == null || currentAction.SecondActionSegment.SegmentStatus != PlayerActionStatus.NotPerformed;
			if (!firstActionWasStarted)
			{
				Shift(turn, null);
			}
			else if (!secondActionWasStarted)
			{
				var actionToShift = new PlayerAction(currentAction.SecondActionSegment.SegmentType, null, null);
				currentAction.SecondActionSegment.SegmentType = null;
				Shift(turn + 1, actionToShift);
			}
			else
				Shift(turn + 1, null);
		}

		public void ShiftAndRepeatPreviousActionAfterPlayerActions(int turn)
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
			while (endTurn < ActionsList.Count && GetActionForTurn(endTurn).FirstActionSegment.SegmentType.HasValue)
				endTurn++;
			ActionsList.Insert(turn - 1, new PlayerAction(actionToInsert?.FirstActionSegment.SegmentType, actionToInsert?.SecondActionSegment.SegmentType, actionToInsert?.BonusActionSegment.SegmentType));
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
			return GetActionForTurn(currentTurn).BonusActionSegment.SegmentType == PlayerActionType.AdvancedSpecialization || GetActionForTurn(currentTurn).FirstActionSegment.SegmentType == PlayerActionType.AdvancedSpecialization;
		}

		private bool IsPerformingBasicSpecialization(int currentTurn)
		{
			return GetActionForTurn(currentTurn).BonusActionSegment.SegmentType == PlayerActionType.BasicSpecialization || GetActionForTurn(currentTurn).FirstActionSegment.SegmentType == PlayerActionType.BasicSpecialization;
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
			if (actionForTurn.SecondActionSegment.SegmentStatus == PlayerActionStatus.Performed)
				HasSpecialOpsProtection = false;
		}

		public void MarkNextActionPerforming(int currentTurn)
		{
			var actionForTurn = GetActionForTurn(currentTurn);
			actionForTurn.MarkNextActionPerforming();
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
