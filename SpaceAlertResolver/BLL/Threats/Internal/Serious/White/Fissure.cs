using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.White
{
	public class Fissure : SeriousWhiteInternalThreat
	{
		public Fissure()
			: base(2, 2, StationLocation.Interceptor, PlayerAction.BattleBots)
		{
		}

		public override void PerformXAction()
		{
			SittingDuck.AddZoneDebuff(new [] {ZoneLocation.Red}, ZoneDebuff.DoubleDamage, this);
		}

		public override void PerformYAction()
		{
			SittingDuck.AddZoneDebuff(EnumFactory.All<ZoneLocation>(), ZoneDebuff.DoubleDamage, this);
		}

		public override void PerformZAction()
		{
			throw new LoseException(this);
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnHealthReducedToZero();
			SittingDuck.RemoveZoneDebuffForSource(EnumFactory.All<ZoneLocation>(), this);
		}

		public static string GetDisplayName()
		{
			return "Fissure";
		}
	}
}
