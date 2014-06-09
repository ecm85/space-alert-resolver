using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Yellow
{
	public class PsionicSatellite : SeriousYellowExternalThreat
	{
		public PsionicSatellite()
			: base(2, 5, 2)
		{
		}

		public override void PerformXAction(int currentTurn)
		{
			SittingDuck.ShiftPlayers(new [] {CurrentZone}, currentTurn + 1);
		}

		public override void PerformYAction(int currentTurn)
		{
			SittingDuck.ShiftPlayers(EnumFactory.All<ZoneLocation>(), currentTurn + 1);
		}

		public override void PerformZAction(int currentTurn)
		{
			//TODO: Exclude interceptors?
			SittingDuck.KnockOutPlayers(EnumFactory.All<StationLocation>());
		}

		public static string GetDisplayName()
		{
			return "Psionic Satellite";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.Range != 3 && base.CanBeTargetedBy(damage);
		}
	}
}
