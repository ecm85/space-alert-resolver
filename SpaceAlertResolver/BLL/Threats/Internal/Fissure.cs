using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class Fissure : SeriousWhiteInternalThreat
	{
		public Fissure(int timeAppears, SittingDuck sittingDuck)
			: base(2, 2, timeAppears, sittingDuck.InterceptorStation1, PlayerAction.Interceptors, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			CurrentZone.DebuffsBySource[this] = ZoneDebuff.DoubleDamage;
		}

		public override void PerformYAction()
		{
			foreach (var zone in sittingDuck.Zones)
				zone.DebuffsBySource[this] = ZoneDebuff.DoubleDamage;
		}

		public override void PerformZAction()
		{
			//TODO: Lose.
		}

		public override void OnDestroyed()
		{
			foreach (var zone in sittingDuck.Zones)
				zone.DebuffsBySource.Remove(this);
		}
	}
}
