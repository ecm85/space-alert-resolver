using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class Shield : EnergyContainer
	{
		private EnergyContainer Source { get; set; }

		protected Shield(Reactor source, int capacity, int energy) : base(capacity, energy)
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
