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
		public IList<PlayerAction> Actions { get; set; }
		public Station CurrentStation { get; set; }
		public BattleBots BattleBots { get; set; }

		public void Shift(int currentTurn)
		{
			//TODO: Shift actions
		}
	}
}
