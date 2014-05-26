using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL
{
	public class Player
	{
		public IList<PlayerAction> Actions { get; set; }
		public Station CurrentStation { get; set; }
	}
}
