using BLL.Common;

namespace BLL.Threats.External.Minor.Yellow
{
	public class Jellyfish : MinorYellowExternalThreat
	{
		public Jellyfish()
			: base(-2, 13, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackAllZones(1);
			HealHalfDamage();
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackAllZones(1);
			HealHalfDamage();
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackAllZones(2);
		}

		private void HealHalfDamage()
		{
			Repair((TotalHealth - RemainingHealth) / 2);
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			Check.ArgumentIsNotNull(damage, "damage");
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
