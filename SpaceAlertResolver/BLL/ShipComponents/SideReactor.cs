using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class SideReactor : Reactor
	{
		private CentralReactor Source { get; set; }
		public SideReactor(CentralReactor source) : base(3, 2)
		{
			Source = source;
		}

		public override void PerformBAction(bool isHeroic)
		{
			var energyToPull = Capacity - Energy;
			Source.Energy -= energyToPull;
			Energy += energyToPull;
			if (energyToPull > 0 && isHeroic)
				Energy++;
		}
	}
}
