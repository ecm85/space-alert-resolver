using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class SideLightLaserCannon : LightLaserCannon
	{
		public SideLightLaserCannon(BatteryPack source, ZoneLocation currentZone) : base(source, currentZone)
		{
		}
	}
}
