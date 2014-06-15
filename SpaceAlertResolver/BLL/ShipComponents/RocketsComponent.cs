using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class RocketsComponent : CComponent
	{
		private List<Rocket> Rockets { get; set; } 
		private Rocket RocketFiredThisTurn { get; set; }
		public Rocket RocketFiredLastTurn { get; private set; }

		public int RocketCount
		{
			get { return Rockets.Count; }
		}

		public event Action RocketsModified = () => { };

		public RocketsComponent()
		{
			Rockets = Enumerable.Range(0, 3).Select(index => new Rocket()).ToList();
		}

		public override void PerformCAction(Player performingPlayer, int currentTurn)
		{
			if (Rockets.Any() && RocketFiredThisTurn == null)
			{
				var firedRocket = Rockets.First();
				Rockets.Remove(firedRocket);
				RocketFiredThisTurn = firedRocket;
				RocketsModified();
			}
		}

		public void PerformEndOfTurn()
		{
			RocketFiredLastTurn = RocketFiredThisTurn;
			RocketFiredThisTurn = null;
		}

		public void RemoveRocket()
		{
			Rockets.Remove(Rockets.First());
			RocketsModified();
		}

		public void RemoveAllRockets()
		{
			Rockets.Clear();
			RocketsModified();
		}
	}
}
