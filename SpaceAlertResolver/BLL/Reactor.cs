using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL
{
	public abstract class Reactor : EnergyContainer
	{
		protected Reactor(int capacity, int energy) : base(capacity, energy)
		{
		}
	}
}
