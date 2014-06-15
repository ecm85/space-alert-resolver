using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class PulseCannon : Cannon
	{
		public PulseCannon(Reactor source) : base(source, 1, new [] {2}, PlayerDamageType.Pulse, EnumFactory.All<ZoneLocation>())
		{
		}

		public override void SetDamaged()
		{
			IsDamaged = true;
			distancesAffected = new[] {1};
		}

		public override void Repair()
		{
			IsDamaged = false;
			distancesAffected = new [] {1, 2};
		}
	}
}
