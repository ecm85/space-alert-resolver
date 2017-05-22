using System;

namespace BLL.ShipComponents
{
	public class PlayerMoveEventArgs : EventArgs
	{
		public Player MovingPlayer { get; set; }
		public int? CurrentTurn { get; set; }
	}
}
