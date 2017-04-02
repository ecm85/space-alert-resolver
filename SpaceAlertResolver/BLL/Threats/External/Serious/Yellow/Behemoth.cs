using System.Collections.Generic;
using System.Linq;

namespace BLL.Threats.External.Serious.Yellow
{
	public class Behemoth : SeriousYellowExternalThreat
	{
		public Behemoth()
			: base(4, 7, 2)
		{
		}
		protected override void PerformXAction(int currentTurn)
		{
			var damageTaken = TotalHealth - RemainingHealth;
			if (damageTaken < 2)
				AttackCurrentZone(2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			var damageTaken = TotalHealth - RemainingHealth;
			if (damageTaken < 3)
				AttackCurrentZone(3);
		}

		protected override void PerformZAction(int currentTurn)
		{
			var damageTaken = TotalHealth - RemainingHealth;
			if (damageTaken < 6)
				AttackCurrentZone(6);
		}

		public override string Id { get; } = "SE2-01";
		public override string DisplayName { get; } = "Behemoth";
		public override string FileName { get; } = "Behemoth";

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			var modifiedDamages = damages.ToList();
			var interceptorDamages = modifiedDamages.SingleOrDefault(damage => damage.PlayerDamageType == PlayerDamageType.InterceptorsSingle);
			if (interceptorDamages != null)
			{
				var strongerInterceptorDamages = new PlayerDamage(interceptorDamages)
				{
					Amount = interceptorDamages.Amount + 6
				};
				interceptorDamages.PerformingPlayer.KnockOutFromOwnAction();
				modifiedDamages.Remove(interceptorDamages);
				modifiedDamages.Add(strongerInterceptorDamages);
			}
			base.TakeDamage(modifiedDamages);
		}
	}
}
