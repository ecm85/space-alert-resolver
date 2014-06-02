using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class CentralReactor : Reactor
	{
		private int storageCapsules = 3;

		public CentralReactor() : base(5, 3)
		{
		}

		public override void PerformBAction(bool isHeroic)
		{
			if (storageCapsules <= 0)
				return;
			var oldEnergy = Energy;
			storageCapsules--;
			Energy = Capacity;
			if (isHeroic && Energy > oldEnergy)
				Energy++;
		}
	}
}
