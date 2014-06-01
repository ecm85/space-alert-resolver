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

		public void PerformBAction()
		{
			throw new NotImplementedException();
		}

		public PlayerDamage PerformAAction()
		{
			throw new NotImplementedException();
		}

		public CResult PerformCAction(Player performingPlayer)
		{
			throw new NotImplementedException();
		}

		public InternalPlayerDamageResult UseBattleBots()
		{
			throw new NotImplementedException();
		}
	}
}
