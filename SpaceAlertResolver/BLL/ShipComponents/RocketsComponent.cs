using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class RocketsComponent : CComponent
	{
		public IList<Rocket> Rockets { get; private set; } 
		private Rocket RocketFiredThisTurn { get; set; }
		public Rocket RocketFiredLastTurn { get; private set; }

		public RocketsComponent()
		{
			Rockets = Enumerable.Range(0, 3).Select(index => new Rocket()).ToList();
		}

		public override CResult PerformCAction(Player performingPlayer)
		{
			if (Rockets.Any() && RocketFiredThisTurn == null)
			{
				var firedRocket = Rockets.First();
				Rockets.Remove(firedRocket);
				RocketFiredThisTurn = firedRocket;
			}
			return new CResult();
		}

		public void PerformEndOfTurn()
		{
			RocketFiredLastTurn = RocketFiredThisTurn;
			RocketFiredThisTurn = null;
		}
	}
}
