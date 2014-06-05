using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Yellow
{
	public class PsionicSatellite : SeriousYellowExternalThreat
	{
		public PsionicSatellite(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(2, 5, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			//TODO: Shift all players (need current turn)
		}

		public override void PerformYAction()
		{
			//TODO: Shift all players (need current turn)
		}

		public override void PerformZAction()
		{
			//TODO: Exclude interceptors?
			sittingDuck.KnockOutPlayers(EnumFactory.All<StationLocation>());
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
