using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class CentralReactor : Reactor
	{
		private int fuelCapsules = 3;

		public int FuelCapsules
		{
			get { return fuelCapsules; }
			set { fuelCapsules = value < 0 ? 0 : value; }
		}

		public CentralReactor() : base(5, 3)
		{
		}

		public override void PerformBAction(bool isHeroic)
		{
			if (fuelCapsules <= 0)
				return;
			var oldEnergy = Energy;
			fuelCapsules--;
			Energy = Capacity;
			if (isHeroic && Energy > oldEnergy)
				Energy++;
		}
	}
}
