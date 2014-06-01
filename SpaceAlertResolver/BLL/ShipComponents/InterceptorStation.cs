using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats.Internal;

namespace BLL.ShipComponents
{
	public class InterceptorStation : IStation
	{
		public Station RedwardStation { get; set; }
		public Station BluewardStation { get; set; }
		public Station OppositeDeckStation { get; set; }
		public EnergyContainer EnergyContainer { get; set; }
		public ZoneLocation ZoneLocation { get; set; }
		public ISet<InternalThreat> Threats { get; private set; }
		public IList<Player> Players { get; private set; }

		public void PerformBAction(Player performingPlayer, int currentTurn)
		{
			performingPlayer.Shift(currentTurn);
		}

		public PlayerDamage PerformAAction(Player performingPlayer, int currentTurn)
		{
			performingPlayer.Shift(currentTurn);
			return null;
		}

		public CResult PerformCAction(Player performingPlayer, int currentTurn)
		{
			performingPlayer.Shift(currentTurn);
			return null;
			//TODO: Change to a further inteceptor station
		}

		public InternalPlayerDamageResult UseBattleBots(Player performingPlayer)
		{
			//TODO: Change to attack with interceptors
			return null;
		}
	}
}
