using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Yellow
{
	public class Behemoth : SeriousYellowExternalThreat
	{
		public Behemoth()
			: base(4, 7, 2)
		{
		}

		public static string GetDisplayName()
		{
			return "Behemoth";
		}

		public override void PerformXAction()
		{
			var damageTaken = TotalHealth - RemainingHealth;
			if (damageTaken < 2)
				Attack(2);
		}

		public override void PerformYAction()
		{
			var damageTaken = TotalHealth - RemainingHealth;
			if (damageTaken < 3)
				Attack(3);
		}

		public override void PerformZAction()
		{
			var damageTaken = TotalHealth - RemainingHealth;
			if (damageTaken < 6)
				Attack(6);
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			//TODO: Rules clarification: Heroic action turns 9 into 10?
			var interceptorDamages = damages.SingleOrDefault(damage => damage.PlayerDamageType == PlayerDamageType.InterceptorsSingle);
			if (interceptorDamages != null)
			{
				var strongerInterceptorDamages = new PlayerDamage(
					9,
					PlayerDamageType.InterceptorsSingle,
					interceptorDamages.Range,
					interceptorDamages.ZoneLocations);
				damages.Remove(interceptorDamages);
				damages.Add(strongerInterceptorDamages);
				SittingDuck.KnockOutPlayers(new [] {StationLocation.Interceptor});
			}
			base.TakeDamage(damages);
		}
	}
}
