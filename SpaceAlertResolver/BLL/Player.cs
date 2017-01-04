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
			Actions = actions.ToList();
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
		public List<PlayerAction> Actions { get; }

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
			Shift(turn, null);
		}

		public void ShiftAndRepeatPreviousAction(int turn)
		{
			var actionToRepeat = turn <= 0 ? null : Actions[turn - 1].ActionType;
			Shift(turn, actionToRepeat);
		}

		private void Shift(int turn, PlayerActionType? actionToInsert)
		{
			var endTurn = turn;
			while (endTurn + 1 < Actions.Count && Actions[endTurn].ActionType.HasValue)
				endTurn++;
			Actions.Insert(turn, PlayerActionFactory.CreateSingleAction(BasicSpecialization, AdvancedSpecialization, actionToInsert));
			Actions.RemoveAt(endTurn + 1);
		}

		public bool IsPerformingMedic(int currentTurn)
		{
			return IsPerformingAdvancedMedic(currentTurn) || IsPerformingBasicMedic(currentTurn);
		}

		public bool IsPerformingAdvancedMedic(int currentTurn)
		{
			return AdvancedSpecialization == PlayerSpecialization.Medic && (Actions[currentTurn].HasAdvancedSpecializationAttached);
		}

		public bool IsPerformingBasicMedic(int currentTurn)
		{
			return AdvancedSpecialization == PlayerSpecialization.Medic && (Actions[currentTurn].HasBasicSpecializationAttached);
		}

		public bool IsPerformingAdvancedSpecialOps(int currentTurn)
		{
			var currentAction = Actions[currentTurn];
			return AdvancedSpecialization == PlayerSpecialization.SpecialOps && currentAction.HasAdvancedSpecializationAttached;
		}
	}
}
