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
			sittingDuck.AddZoneDebuff(new [] {ZoneLocation.Red}, ZoneDebuff.DoubleDamage, this);
		}

		public override void PerformYAction()
		{
			sittingDuck.AddZoneDebuff(EnumFactory.All<ZoneLocation>(), ZoneDebuff.DoubleDamage, this);
		}

		public override void PerformZAction()
		{
			throw new LoseException(this);
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();
			sittingDuck.RemoveZoneDebuffForSource(EnumFactory.All<ZoneLocation>(), this);
		}

		public static string GetDisplayName()
		{
			return "Fissure";
		}
	}
}
