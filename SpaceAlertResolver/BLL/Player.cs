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
		public bool IsUsingInterceptors { get; set; }
		public List<PlayerAction> Actions { get; set; }
		public Station CurrentStation { get; set; }
		public BattleBots BattleBots { get; set; }
		public int Index { get; set; }
		public bool IsPoisoned { get; set; }

		public void Shift(int turn)
		{
			var endTurn = turn;
			while (endTurn + 1 < Actions.Count && Actions[endTurn] != PlayerAction.None)
				endTurn++;
			Actions.Insert(turn, PlayerAction.None);
			Actions.RemoveAt(endTurn + 1);
		}
	}
}
