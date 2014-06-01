using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal
{
	public abstract class HackedShields : MinorWhiteInternalThreat
	{
		protected HackedShields(int timeAppears, IStation station, SittingDuck sittingDuck)
			: base(3, 2, timeAppears, station, PlayerAction.B, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			CurrentStation.EnergyContainer.Energy = 0;
		}

		public override void PerformYAction()
		{
			CurrentStation.OppositeDeckStation.EnergyContainer.Energy = 0;
		}

		public override void PerformZAction()
		{
			Damage(2);
		}
	}
}
