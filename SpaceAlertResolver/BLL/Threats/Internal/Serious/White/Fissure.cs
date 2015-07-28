using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.White
{
	public class Fissure : SeriousWhiteInternalThreat
	{
		public Fissure()
			: base(2, 2, StationLocation.Interceptor1, PlayerActionType.BattleBots)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.AddZoneDebuff(new [] {ZoneLocation.Red}, ZoneDebuff.DoubleDamage, this);
		}

		protected override void PerformYAction(int currentTurn)
		{
			SittingDuck.AddZoneDebuff(EnumFactory.All<ZoneLocation>(), ZoneDebuff.DoubleDamage, this);
		}

		protected override void PerformZAction(int currentTurn)
		{
			throw new LoseException(this);
		}

		protected override void OnThreatTerminated()
		{
			SittingDuck.RemoveZoneDebuffForSource(EnumFactory.All<ZoneLocation>(), this);
			base.OnThreatTerminated();
		}
	}
}
