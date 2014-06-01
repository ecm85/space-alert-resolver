using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class RocketsComponent : CComponent
	{
		private readonly IList<Rocket> rockets = Enumerable.Range(0, 3).Select(index => new Rocket()).ToList();
		public Rocket RocketFiredThisTurn { get; private set; }
		public Rocket RocketFiredLastTurn { get; private set; }

		public override CResult PerformCAction(Player performingPlayer)
		{
			if (rockets.Any() && RocketFiredThisTurn == null)
			{
				var firedRocket = rockets.First();
				rockets.Remove(firedRocket);
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
