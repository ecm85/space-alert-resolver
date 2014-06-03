using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class PulseCannon : Cannon
	{
		public PulseCannon(Reactor source) : base(source, 1, 2, PlayerDamageType.Pulse, EnumFactory.All<ZoneLocation>())
		{
		}

		public override void SetDamaged()
		{
			var wasAlreadyDamaged = IsDamaged;
			IsDamaged = true;
			if (!wasAlreadyDamaged)
				range -= 1;
		}

		public override void Repair()
		{
			var wasAlreadyDamaged = IsDamaged;
			IsDamaged = false;
			if (wasAlreadyDamaged)
				range += 1;
		}
	}
}
