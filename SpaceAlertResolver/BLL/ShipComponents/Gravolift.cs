using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class Gravolift
	{
		private bool Occupied { get; set; }

		public void SetOccupied()
		{
			Occupied = true;
		}

		public void PerformEndOfTurn()
		{
			Occupied = false;
		}

		public bool ShiftsPlayers { get { return Occupied || IsDamaged; } }
		private bool IsDamaged { get; set; }

		public void SetDamaged()
		{
			IsDamaged = true;
		}

		public void Repair()
		{
			IsDamaged = false;
		}
	}
}
