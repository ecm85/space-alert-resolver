using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL
{
	public class Player
	{
		public bool IsKnockedOut { get; set; }
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

		public void Shift(int turn, bool repeatPreviousAction = false)
		{
			var endTurn = turn;
			while (endTurn + 1 < Actions.Count && Actions[endTurn] != PlayerAction.None)
				endTurn++;
			var actionToInsert = repeatPreviousAction ? Actions[turn - 1] : PlayerAction.None;
			Actions.Insert(turn, actionToInsert);
			Actions.RemoveAt(endTurn + 1);
		}
	}
}
