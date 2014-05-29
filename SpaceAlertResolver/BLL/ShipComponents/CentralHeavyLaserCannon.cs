using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class CentralHeavyLaserCannon : HeavyLaserCannon
	{
		public CentralHeavyLaserCannon(Reactor source, ZoneLocation currentZone) : base(source, 5, currentZone)
		{
		}
	}
}
