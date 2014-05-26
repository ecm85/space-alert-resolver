using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class CentralHeavyLaserCannon : HeavyLaserCannon
	{
		public CentralHeavyLaserCannon(Reactor source, ZoneType currentZoneType) : base(source, 5, currentZoneType)
		{
		}
	}
}
