using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class UpperStation : StandardStation
	{
		public Shield Shield { get; set; }
		protected override void RefillEnergy(bool isHeroic)
		{
			Shield.PerformBAction(isHeroic);
		}

		public override void DrainEnergyContainer(int amount)
		{
			Shield.Energy -= amount;
		}

		public override void PerformEndOfTurn()
		{
			base.PerformEndOfTurn();
			Shield.PerformEndOfTurn();
		}
	}
}
