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

		protected override void PerformXAction(int currentTurn)
		{
			var damageTaken = TotalHealth - RemainingHealth;
			if (damageTaken < 2)
				Attack(2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			var damageTaken = TotalHealth - RemainingHealth;
			if (damageTaken < 3)
				Attack(3);
		}

		protected override void PerformZAction(int currentTurn)
		{
			var damageTaken = TotalHealth - RemainingHealth;
			if (damageTaken < 6)
				Attack(6);
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			var interceptorDamages = damages.SingleOrDefault(damage => damage.PlayerDamageType == PlayerDamageType.InterceptorsSingle);
			if (interceptorDamages != null)
			{
				var strongerInterceptorDamages = new PlayerDamage(
					interceptorDamages.Amount + 6,
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
