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

		public override void PerformBAction()
		{
			var energyToPull = capacity - Energy;
			Source.Energy -= energyToPull;
			Energy += energyToPull;
		}
	}
}
