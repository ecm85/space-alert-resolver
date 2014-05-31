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
		public IStation CurrentStation { get; set; }
		public BattleBots BattleBots { get; set; }
	}
}
