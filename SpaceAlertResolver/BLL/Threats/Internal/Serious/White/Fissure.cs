using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.White
{
	public class Fissure : SeriousWhiteInternalThreat
	{
		public Fissure(int timeAppears, ISittingDuck sittingDuck)
			: base(2, 2, timeAppears, StationLocation.Interceptor, PlayerAction.BattleBots, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			sittingDuck.ZonesByLocation[ZoneLocation.Red].DebuffsBySource[this] = ZoneDebuff.DoubleDamage;
		}

		public override void PerformYAction()
		{
			foreach (var zone in sittingDuck.Zones)
				zone.DebuffsBySource[this] = ZoneDebuff.DoubleDamage;
		}

		public override void PerformZAction()
		{
			throw new LoseException(this);
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();
			foreach (var zone in sittingDuck.Zones)
				zone.DebuffsBySource.Remove(this);
		}

		public static string GetDisplayName()
		{
			return "Fissure";
		}
	}
}
