using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class RocketsComponent : CComponent
	{
		private readonly IList<Rocket> rockets = Enumerable.Range(0, 3).Select(index => new Rocket()).ToList();
		public override CResult PerformCAction(Player performingPlayer)
		{
			if (!rockets.Any())
				return new CResult();
			var firedRocket = rockets.First();
			rockets.Remove(firedRocket);
			return new CResult
			{
				RocketFired = firedRocket
			};
		}
	}
}
