using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class LowerStation : StandardStation
	{
		public Reactor Reactor { get; set; }
		protected override void RefillEnergy(bool isHeroic)
		{
			Reactor.PerformBAction(isHeroic);
		}
	}
}
