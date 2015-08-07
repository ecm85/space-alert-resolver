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

		public override void PerformCAction(Player performingPlayer, int currentTurn, bool isAdvancedUsage)
		{
			if (CanPerformCAction(performingPlayer))
			{
				var canFireDoubleRocket = RocketCount > 1;
				var firedRocket = Rockets.First();
				Rockets.Remove(firedRocket);
				RocketFiredThisTurn = firedRocket;
				if (isAdvancedUsage && canFireDoubleRocket)
				{
					Rockets.Remove(Rockets.First());
					firedRocket.SetDoubleRocket();
				}
				RocketsModified();
			}
		}

		public override bool CanPerformCAction(Player performingPlayer)
		{
			return RocketFiredLastTurn == null && Rockets.Any();
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
