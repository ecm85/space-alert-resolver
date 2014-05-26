using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class PulseCannon : Cannon
	{
		public PulseCannon(Reactor source) : base(source, 1, 2, DamageType.Pulse, ZoneTypes.All())
		{
		}
	}
}
