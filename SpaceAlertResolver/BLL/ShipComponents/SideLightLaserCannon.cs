using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class SideLightLaserCannon : LightLaserCannon
	{
		public SideLightLaserCannon(BatteryPack source, ZoneType currentZoneType) : base(source, currentZoneType)
		{
		}
	}
}
